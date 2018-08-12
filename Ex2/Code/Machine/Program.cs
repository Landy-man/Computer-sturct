using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SimpleComponents;

namespace Machine
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Machine16 machine = new Machine16(false, true);
            machine.Code.LoadFromFile(@"Fibonacci.hack");
            machine.Data[0] = 12;
            machine.Data[1] = 0;
            DateTime dtStart = DateTime.Now;
            machine.Reset();
            for (int i = 0; i < 12000; i++)
            {
                //machine.CPU.PrintState();
                Console.WriteLine();
                Clock.ClockDown();
                Clock.ClockUp();
            }

            for (int i = 100; i < 120;i++ )
                                Console.WriteLine(machine.Data[i] + ",");
            Console.ReadLine();

            //Console.WriteLine("Done " + (DateTime.Now - dtStart).TotalSeconds);
            //Console.ReadLine();
        }
    }
}
