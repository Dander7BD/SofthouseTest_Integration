using MarkupIntegrationTest;

namespace ExecuteableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TranslateTest examiner = new TranslateTest();
            examiner.LundgrenToXMLTest();
            examiner.LundgrenToJSONTest();
        }
    }
}
