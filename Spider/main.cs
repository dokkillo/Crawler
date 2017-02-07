using System.Threading;
using Spider.BLL;

namespace Spider
{
    class Program
    {
        static int SLEEP_TIME = 30000;

        static void Main(string[] args)
        {          

            do
            {
                System.Console.WriteLine("--------------------------------------");
                var link = DokkiBot.GetLink();
                DokkiBot.ExtractLinks(link.UrlName, link.ID);
                DokkiBot.EditLink(link);
                 
               Thread.Sleep(SLEEP_TIME);

            } while (true);

           
        }

 

       
    }
}
