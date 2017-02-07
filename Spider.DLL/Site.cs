using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DLL
{
   public class Site: IEntity
    {

        public Site()
        {

        }

        public Site(string _Url, string _Html, string _Title,string _Description)
        {
            this.Url = _Url;
            this.Html = _Html;
            this.Title = _Title;
            this.Description = _Description;
        }


        public int ID { get; set; }
        public string Url { get; set; }
        public string Html { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

     /*   public bool IsVisited { get; set; }
        public bool IsRoot { get; set; }
        public bool IsTrash { get; set; }

        public string srcImage { get; set; }
        public string srcImageIphonePortrait { get; set; }
        public string srcImageIphoneLandscape { get; set; }
        public string srcImageIpadPortrait { get; set; }
        public string srcImageIpadLandscape { get; set; } */

    }
}
