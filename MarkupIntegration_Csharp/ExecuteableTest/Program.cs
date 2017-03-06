using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkupIntegrationTest;

namespace ExecuteableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LundgrenToXMLTest examiner = new LundgrenToXMLTest();
            examiner.Test();
        }
    }
}
