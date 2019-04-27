using LyncAvailability.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LyncAvailability
{
    class Program
    {
        static void Main(string[] args)
        {
           String userEmail = "soumchoudhury@deloitte.com";
            //if (args.Count() != 1)
            //{
            //    Console.WriteLine(Resources.Usage);
            //    return;
            //}
            for(;;)
            { 
            LyncManagerStatus lyncManager = new LyncManagerStatus(userEmail);

            //wait for callback to set lm to true
            while (!lyncManager.Done)
            {
                //Console.WriteLine("running");
            }
                Thread.Sleep(3000);
            }
        }
    }
}
