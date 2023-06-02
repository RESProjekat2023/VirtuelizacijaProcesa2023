using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ServiceHost host = new ServiceHost(typeof(FileHandler));
            host.Open();

            Console.WriteLine("Service is open, press any key to close it.");
            Console.ReadKey();

            host.Close();
            Console.WriteLine("Service is closed");

        }
    }
}
