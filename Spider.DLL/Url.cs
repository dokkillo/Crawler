using System;

namespace Spider.DLL
{
    public class Url: IEntity
    {

        public Url()
        {

        }

        public Url(string UrlName)
        {
            this.UrlName = UrlName;
            this.UrlParentId = 0;
            this.LastTime = DateTime.Now;
            this.IsVisited = false;
        }

        public Url(string UrlName, int ParentId)
        {
            this.UrlName = UrlName;
            this.UrlParentId = ParentId;
            this.LastTime = DateTime.Now;
            this.IsVisited = false;
        }

        

        public int ID { get; set; }
        public string UrlName { get; set; }
        public bool IsVisited { get; set; }
        public bool IsWrong { get; set; }
        public DateTime LastTime { get; set; }
        public int UrlParentId { get; set; }

    }
}
