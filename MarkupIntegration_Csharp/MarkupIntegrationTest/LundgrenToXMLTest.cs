using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using MarkupIntegration;
using System.Text;

namespace MarkupIntegrationTest
{
    [TestClass]
    public class LundgrenToXMLTest
    {
        private static readonly string
            LundgrenLBMData =
                "P|Carl Gustaf|Bernadotte\n"
            +   "T|0768-101801|08-101801\n"
            +   "A|Drottningholms slott|Stockholm|10001\n"
            +   "F|Victoria|1977\n"
            +   "A|Haga Slott|Stockholm|10002\n"
            +   "F|Carl Philip|1979\n"
            +   "T|0768-101802|08-101802\n"
            +   "P|Barack|Obama\n"
            +   "A|1600 Pennsylvania Avenue|Washington, D.C\n",
            ExpectedXML =
                "<?xml version=\"1.0\" encoding=\"utf-16\"?>\n"
            +   "<people>\n"
            +   "  <person>\n"
            +   "    <firstname>Carl Gustaf</firstname>\n"
            +   "    <lastname>Bernadotte</lastname>\n"
            +   "    <phone>\n"
            +   "      <mobile>0768-101801</mobile>\n"
            +   "      <landline>08-101801</landline>\n"
            +   "    </phone>\n"
            +   "    <address>\n"
            +   "      <street>Drottningholms slott</street>\n"
            +   "      <city>Stockholm</city>\n"
            +   "      <zipcode>10001</zipcode>\n"
            +   "    </address>\n"
            +   "    <family>\n"
            +   "      <name>Victoria</name>\n"
            +   "      <born>1977</born>\n"
            +   "      <address>\n"
            +   "        <street>Haga Slott</street>\n"
            +   "        <city>Stockholm</city>\n"
            +   "        <zipcode>10002</zipcode>\n"
            +   "      </address>\n"
            +   "    </family>\n"
            +   "    <family>\n"
            +   "      <name>Carl Philip</name>\n"
            +   "      <born>1979</born>\n"
            +   "      <phone>\n"
            +   "        <mobile>0768-101802</mobile>\n"
            +   "        <landline>08-101802</landline>\n"
            +   "      </phone>\n"
            +   "    </family>\n"
            +   "  </person>\n"
            +   "  <person>\n"
            +   "    <firstname>Barack</firstname>\n"
            +   "    <lastname>Obama</lastname>\n"
            +   "    <address>\n"
            +   "      <street>1600 Pennsylvania Avenue</street>\n"
            +   "      <city>Washington, D.C</city>\n"
            +   "    </address>\n"
            +   "  </person>\n"
            +   "</people>\n";

        [TestMethod]
        public void Test()
        {
            StringReader istream = new StringReader(LundgrenLBMData);
            StringWriter ostream = new StringWriter() { NewLine = "\n" };

            IMLReader mlReader = new LundgrenLBMReader(istream);
            IMLWriter mlWriter = new XMLWriter(ostream) { IndentationSymbol = "  " };
            mlReader.TranslateTo( mlWriter );

            string xmlResult = ostream.ToString();
            StringBuilder tracker = (new StringBuilder()).Append('#', ExpectedXML.Length);

            for( int i = 0; i < ExpectedXML.Length; ++i )
            {
                tracker[i] = xmlResult[i];
                Assert.IsTrue( ExpectedXML[i] == xmlResult[i], tracker.ToString() );
            }
        }
    }
}
