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
    }
}
