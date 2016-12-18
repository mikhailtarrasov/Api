using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkDatabaseDll;

namespace VkDatabaseWorker
{
    class Program
    {
        static void Main(string[] args)
        {
            var efClient = new EFDatabaseClient();
            efClient.FillInDatabase("chelyabinskfw");

            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
