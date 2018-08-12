using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembler a = new Assembler();
            a.TranslateAssemblyFile(@"Max.asm", @"Max.mc");
            a.ToString();
        }
    }
}
