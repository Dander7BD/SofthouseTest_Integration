using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarkupIntegrationTest
{
    [TestClass]
    public class LundgrenToXMLTest
    {
        private static readonly string
            LundgrenLBMData =
                "P|Carl Gustaf|Bernadotte"
            +   "\nT|0768-101801|08-101801"
            +   "\nA|Drottningholms slott|Stockholm|10001"
            +   "\nF|Victoria|1977"
            +   "\nA|Haga Slott|Stockholm|10002"
            +   "\nF|Carl Philip|1979"
            +   "\nT|0768-101802|08-101802"
            +   "\nP|Barack|Obama"
            +   "\nA|1600 Pennsylvania Avenue|Washington, D.C",
            XMLData =
                "<people>"
            +   "\n  <person>"
            +   "\n    <firstname>Carl Gustaf</firstname>"
            +   "\n    <lastname>Bernadotte</lastname>"
            +   "\n    <phone>"
            +   "\n      <mobile>0768-101801</mobile>"
            +   "\n      <landline>08-101801</landline>"
            +   "\n    </phone>"
            +   "\n    <address>"
            +   "\n      <street>Drottningholms slott</street>"
            +   "\n      <city>Stockholm</city>"
            +   "\n      <zipcode>10001</zipcode>"
            +   "\n    </address>"
            +   "\n    <family>"
            +   "\n      <name>Victoria</name>"
            +   "\n      <born>1977</born>"
            +   "\n      <address>"
            +   "\n        <street>Haga Slott</street>"
            +   "\n        <city>Stockholm</city>"
            +   "\n        <zipcode>10002</zipcode>"
            +   "\n      </address>"
            +   "\n    </family>"
            +   "\n    <family>"
            +   "\n      <name>Carl Philip</name>"
            +   "\n      <born>1979</born>"
            +   "\n      <phone>"
            +   "\n        <mobile>0768-101802</mobile>"
            +   "\n        <landline>08-101802</landline>"
            +   "\n      </phone>"
            +   "\n    </family>"
            +   "\n  </person>"
            +   "\n  <person>"
            +   "\n    <firstname>Barack</firstname>"
            +   "\n    <lastname>Obama</lastname>"
            +   "\n    <address>"
            +   "\n      <street>1600 Pennsylvania Avenue</street>"
            +   "\n      <city>Washington, D.C</city>"
            +   "\n    </address>"
            +   "\n</people>";

        [TestMethod]
        public void Test()
        {
            string probe = LundgrenLBMData;
            probe = XMLData;
            probe = "";
        }
    }
}
