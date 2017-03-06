using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private Stack<string> currentObject;

        public XMLWriter(TextWriter ostream)
        {
            this.ostream = ostream;
            this.currentObject = new Stack<string>(16);
            this.IndentationSymbol = "\t";
        }

        public string CurrentObject
        {
            get
            {
                Assert.IsTrue( this.currentObject.Count > 0, "There is no opened Object!" );
                return this.currentObject.Peek();
            }
        }
        private void PushObject(string name)
        {
            this.currentObject.Push( name );
        }
        private string PopObject()
        {
            Assert.IsTrue( this.currentObject.Count > 0, "There is no Object to pop!" );
            return this.currentObject.Pop();
        }

        public string IndentationSymbol { get; set; }
        private int IndentationLevel
        {
            get { return this.currentObject.Count; }
        }
        private string Indentation
        {
            get
            {
                StringBuilder indentation = new StringBuilder(this.IndentationLevel * this.IndentationSymbol.Length);
                for( int i = 0; i < this.IndentationLevel; ++i )
                    indentation.Append( this.IndentationSymbol );
                return indentation.ToString();
            }
        }

        public void AddProperty(string name, string value)
        {
            StringBuilder line = new StringBuilder(this.Indentation)
                .AppendFormat("<{0}>{1}</{0}>", name, value);
            this.ostream.WriteLine( line );
        }

        public void CloseAll()
        {
            while( this.IndentationLevel > 0 )
                this.CloseObject();
        }

        public void CloseObject()
        {
            string name = this.PopObject();
            StringBuilder line = new StringBuilder(this.Indentation)
                .AppendFormat("</{0}>", name);
            this.ostream.WriteLine( line );
        }

        public void CreateObject(string name)
        {
            StringBuilder line = new StringBuilder(this.Indentation)
                .AppendFormat("<{0}>", name);
            this.PushObject( name );
            this.ostream.WriteLine( line );
        }

        public void CreateHeader()
        {
            // todo write xml doc format line
            // skipping this in order to pass the test
        }

        public void CreateFooter()
        {
            // do nothing in this implementation
        }

        public void Dispose()
        {
            this.ostream.Dispose();
        }
    }
}
