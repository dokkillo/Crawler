using HtmlAgilityPack;
using Spider.DAL;
using Spider.DLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.BLL
{
    public static class DokkiBot
    {
        static DBContext db = new DBContext();
        static Repository<Url> Repository = new Repository<Url>(db);
        static Repository<Site> RepositorySite = new Repository<Site>(db);


        #region db methods

        private static void Init()
        {
            string MainUrl =AppData.Default.InitWebsite;
            var urlObject = new Url(MainUrl);
            Repository.Save(urlObject);
        }

        public static Url GetLink()
        {
           var link = Repository.Find(x => x.IsVisited == false).FirstOrDefault();
            if(link == null)
            {
                Init();
                return GetLink();
            }
            return link;
        }

        public static void EditLink(Url link)
        {
            link.IsVisited = true;
            link.LastTime = DateTime.Now;
            Repository.Edit(link);
        }

        private static void WrongUrl(string MainUrl, Exception ex)
        {
            Console.WriteLine(ex.InnerException);
            var result = Repository.FindOne(x => x.UrlName == MainUrl);
            result.IsVisited = true;
            result.IsWrong = true;
            result.LastTime = DateTime.Now;
            Repository.Edit(result);
        }

        #endregion

        #region utils methods

        private static string CleanUrl(string url)
        {
            int lastSlash = url.LastIndexOf('/');
            url = (lastSlash > -1) ? url.Substring(0, lastSlash) : url;
            return url;
        }

        private static bool IsNotBigWebsite(string url)
        {
            var urlLower = url.ToLower();
            if (urlLower.Contains("youtube") || urlLower.Contains("linkedin")
               || urlLower.Contains("facebook") || urlLower.Contains("google") || urlLower.Contains("gmail") || urlLower.Contains("microsoft") || urlLower.Contains("youtu")
               || urlLower.Contains("twitter") || urlLower.Contains("gplus") || urlLower.Contains("github") || urlLower.Contains("@")
                )
            {
                return false;
            }
            return true;
        }

        private static bool IsRightUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion

        #region site methods

        private static void GetSitesData(string Url)
        {
            var uri = new Uri(Url);
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(Url);
            var result = RepositorySite.FindOne(x => x.Url == uri.Host);

            if (result == null)
            {
                RepositorySite.Save(new Site(uri.Host, doc.DocumentNode.OuterHtml, GetTtitle(doc), GetDescription(doc)));
            }
        }

        private static string  GetDescription(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (node != null)
            {
                string desc = node.GetAttributeValue("content", "");
                Console.Write("DESCRIPTION: " + desc);
                return desc;

            }
            else
            {
                Console.Write("DESCRIPTION: No ");
                return string.Empty;
            }
        }

        private static string GetTtitle(HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode("//head/title");

            if (node != null)
            {
                string desc = node.InnerHtml;
                Console.Write("Title: " + desc);
                return desc;

            }
            else
            {
                Console.Write("Title: No ");
                return string.Empty;
            }
        }

        #endregion

        public static void ExtractLinks(string MainUrl, int parentId)
        {
            var linksForVisit = ScrapWebsite(MainUrl, parentId);
            System.Console.WriteLine(DateTime.Now.ToShortTimeString() + "visitados " + linksForVisit.Count());
            foreach (var item in linksForVisit)
            {
                var result = Repository.FindOne(x => x.UrlName == item.UrlName || x.UrlName == item.UrlName+ "#" || x.UrlName == item.UrlName + "/#");
                if (result == null && !(item.UrlName.Contains("@")) && !(item.UrlName.EndsWith(".pdf") && !(item.UrlName.EndsWith("#")) && !(item.UrlName.EndsWith("/#"))))
                {
                    if (IsRightUrl(item.UrlName))
                    {
                        System.Console.WriteLine("guardado:" + item.UrlName);
                        Repository.Save(item);
                        GetSitesData(item.UrlName);
                    }
                }
            }
        }

        private static List<Url> ScrapWebsite(string MainUrl, int ParentId)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(MainUrl);
                var root = document.DocumentNode;

                var html = document.DocumentNode.InnerHtml;



                var sb = new StringBuilder();
                foreach (var node in root.DescendantsAndSelf())
                {
                    if (!node.HasChildNodes)
                    {
                        string text = node.InnerText;
                        if (!string.IsNullOrEmpty(text))
                            sb.AppendLine(text.Trim());
                    }
                }
                var select = root.SelectNodes("//meta[contains(@content, 'URL')]");
                try
                {
                    Console.WriteLine("has redirect.." + MainUrl);
                    var redirectPage = select[0].Attributes["content"].Value.Split('=')[1];
                    document = web.Load(MainUrl + "/" + redirectPage);
                    return CrawPage(MainUrl, document, ParentId);

                }
                catch
                {
                    Console.WriteLine("have not redirect using HTML" + MainUrl);
                    return CrawPage(MainUrl, document, ParentId);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error: file wrong " + MainUrl);

                return new List<Url>();
            }

        }

        private static List<Url> CrawPage(string MainUrl, HtmlDocument document, int ParentId)
        {
            List<Url> linkList = new List<Url>();

            try
            {
                HtmlNode[] nodes = document.DocumentNode.SelectNodes("//a").ToArray();

                foreach (HtmlNode link in nodes)
                {
                    // Get the value of the HREF attribute
                    string hrefValue = link.GetAttributeValue("href", string.Empty);
                    if(!(hrefValue.StartsWith("http")))
                    {
                        Uri uriMain = new Uri(MainUrl);
                        Uri uriTotal = new Uri(uriMain, hrefValue);
                        linkList.Add(new Url(uriTotal.ToString(), ParentId));
                      //  GetSitesData(uriTotal.AbsoluteUri);

                    }
                    else
                    {
                        if (IsNotBigWebsite(hrefValue))
                        {
                            linkList.Add(new Url(hrefValue, ParentId));
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                WrongUrl(MainUrl, ex);

            }

            return linkList;
        }     

        static void GetData(string url, int width, int height, string nameFile)
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "Crawler.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = url + " " + width + " " + height + " " + nameFile;
            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit(60000);
                }
            }
            catch
            {
                // Log error.
            }

        }

      

    }
}
