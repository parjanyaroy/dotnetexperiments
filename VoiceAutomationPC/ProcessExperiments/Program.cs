using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExperiments
{
    class Program
    {
        static void Main(string[] args)
        {
            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                if (theprocess.ProcessName.Contains("lync"))
                {
                    Console.WriteLine("Process: {0} ID: {1}", theprocess.ProcessName, theprocess.Id);
                    
                }
            }
            Console.ReadKey();
        }
    }
}
