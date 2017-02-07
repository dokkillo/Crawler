using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.DLL
{
    public class ForbiddenUrl : IEntity
    {
        public int ID { get; set; }

        public string Url { get; set; }

    }

}
