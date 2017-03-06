using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    public class XMLWriter : IMLWriter
    {
        private TextWriter ostream;

        public XMLWriter(TextWriter ostream)
        {
            this.ostream = ostream;
        }

        public string CurrentObject
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string IndentationSymbol { get; set; }
        private string IndentedNewLine(int levels)
        {
            StringBuilder output = new StringBuilder('\n');
            for(int i = 0; i < levels; ++i)
                output.Append(this.IndentationSymbol);
            return output.ToString();
        }

        public void AddProperty(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void CloseAll()
        {
            throw new NotImplementedException();
        }

        public void CloseObject()
        {
            throw new NotImplementedException();
        }

        public void CreateObject(string name)
        {
            throw new NotImplementedException();
        }

        public void CreateHeader()
        {
            throw new NotImplementedException();
        }

        public void CreateFooter()
        {
            throw new NotImplementedException();
        }
    }
}
