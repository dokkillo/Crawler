
namespace WebBrowserScreenshotSample
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;

    class Program
    {
        public static int width;
        public static int height;
        public static string url;
        public static string filename ="";
        public static string Path = @"C:\gabi\Imagenes\";

        [STAThread]
        static void Main(string[] args)
        {
            url = args[0].ToString();
            width = Convert.ToInt32(args[1].ToString());
            height = Convert.ToInt32(args[2].ToString());
            filename = args[3].ToString();


            using (WebBrowser browser = new WebBrowser())
            {
                browser.ScriptErrorsSuppressed = true;
                browser.Width = width;
                browser.Height = height;
                browser.ScrollBarsEnabled = true;
                

                browser.DocumentCompleted += Program.OnDocumentCompleted;
                GetWeb(browser);
            }
        }

        private static void GetWeb(WebBrowser browser)
        {
            browser.Navigate(url, null, null, "User-Agent: Dokkobot");           
           Application.Run();
        }

        static void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // Now that the page is loaded, save it to a bitmap
            WebBrowser browser = (WebBrowser)sender;
            var res = browser.DocumentText;
            using (Graphics graphics = browser.CreateGraphics())
            using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height, graphics))
            {
                Rectangle bounds = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                browser.DrawToBitmap(bitmap, bounds);
                var urlName =  browser.Url.ToString();
                string file = Path + ""+ filename;
                bitmap.Save(file , ImageFormat.Jpeg);
                Console.WriteLine(browser.Url.ToString() );               
            }         
            System.Environment.Exit(1);   
        }

        private static string CleanFileName(string urlName)
        {
            string filename = urlName.Replace(".", "").Replace("=", "").Replace("?", "").Replace("/", "")
                .Replace(":", "")
                .Replace("_", "").Replace("&", "") + ".jpg";
            return filename;
        }
    }
}