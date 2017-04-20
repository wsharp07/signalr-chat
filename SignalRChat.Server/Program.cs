using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace SignalRChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8081";
            using (WebApp.Start(url))
            {
                Console.WriteLine($"Server running on {url}");
                Console.ReadLine();
            }
        }
    }
}
