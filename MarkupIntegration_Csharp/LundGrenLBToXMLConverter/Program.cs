using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkupIntegration;
using System.IO;

namespace LundGrenLBToXMLConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if( args.Length < 2 )
            {
                Console.WriteLine( "usage: LundGrenLBToXMLConverter path-source-file path-output-file." );
                Console.ReadKey();
                return;
            }

            try
            {
                using( IMLReader lundgrenReader = new LundgrenLBMReader( new StreamReader( args[0] ) ) )
                {
                    using( IMLWriter xmlWriter = new XMLWriter( new StreamWriter( args[1] ) { NewLine = "\n" } ) { IndentationSymbol = "  " } )
                    {
                        lundgrenReader.TranslateTo( xmlWriter );
                    }
                }
            }
            catch( Exception e )
            {
                Console.Error.Write( e.Message );
                Console.ReadKey();
            }
        }
    }
}
