using System;
using Microsoft.Owin.Hosting;

namespace SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080"))
            {
                Console.WriteLine("Server is running at http://localhost:8080/.");
                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
