using Spider.DLL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DAL
{
    public class DBContext : DbContext
    {

        public DBContext() : base(ConnectionStringSetting.Default.cs)
        {

        }

        public IDbSet<Url> Urls { get; set; }
        public IDbSet<Site> Sites { get; set; }
        public IDbSet<Email> Emails { get; set; }

        public IDbSet<ForbiddenUrl> ForbiddenUrls { get; set; }


        




    }
}
