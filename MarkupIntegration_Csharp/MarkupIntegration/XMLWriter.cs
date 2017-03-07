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
        private struct Element
        {
            public Element(string name, ElementType type)
            {
                this.Name = name;
                this.Type = type;
                this.ChildrenCount = 0;
            }
            public string Name;
            public ElementType Type;
            public int ChildrenCount;
            public override bool Equals(object obj)
            {
                if( obj is Element )
                {
                    Element e = (Element)obj;
                    return this.Type.Equals( e.Type ) && this.Name.Equals( e.Name );
                }
                return false;
            }
        }

        private TextWriter ostream;
        private Stack<Element> elementStack;
        private int noneIndents;

        public XMLWriter(TextWriter ostream)
        {
            this.ostream = ostream;
            this.elementStack = new Stack<Element>(16);
            this.elementStack.Push( new Element( "", ElementType.Base ) );
            this.noneIndents = 1;
            this.IndentationSymbol = "\t";
        }

        public string CurrentName
        {
            get { return this.elementStack.Peek().Name; }
        }
        public ElementType CurrentType
        {
            get { return this.elementStack.Peek().Type; }
        }
        private int ChildrenCount
        {
            get { return this.elementStack.Peek().ChildrenCount; }
            set
            {
                Element o = this.elementStack.Pop();
                o.ChildrenCount = value;
                this.elementStack.Push( o );
            }
        }

        public string IndentationSymbol { get; set; }
        private int IndentationLevel
        {
            get { return this.elementStack.Count - this.noneIndents; }
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

        public string NewLineSymbol
        {
            get { return this.ostream.NewLine; }
            set { this.ostream.NewLine = value; }
        }

        private string NewLine
        { get { return this.NewLineSymbol + this.Indentation; } }

        public void PushList(string name)
        {
            ++this.ChildrenCount;
            this.elementStack.Push( new Element( name, ElementType.List ) );
            ++this.noneIndents;
        }

        public void PushListElement()
        {
            Assert.IsTrue( this.CurrentType == ElementType.List, "Trying to push a list element into a none-list.\nUse PopUntilMatch(ElementType.List, \"listName\")" );
            
            string name = this.elementStack.Peek().Name;
            StringBuilder line = new StringBuilder(this.NewLine)
                .AppendFormat("<{0}>", name);

            this.elementStack.Push( new Element( name, ElementType.ListElement ) );
            this.ostream.Write( line );
        }

        public void PushElement(string name)
        {
            Assert.IsTrue( this.elementStack.Peek().Type != ElementType.List, "Close List "+ this.elementStack.Peek().Name + " before appending new element " + name + "." );

            StringBuilder line = new StringBuilder(this.NewLine)
                .AppendFormat("<{0}>", name);
            ++this.ChildrenCount;
            this.elementStack.Push( new Element( name, ElementType.Element ) );
            this.ostream.Write( line );
        }

        public void AttachProperty(string name, string value)
        {
            Assert.IsTrue( this.elementStack.Peek().Type != ElementType.List, "Only Elements and ListElements may have properties." );

            StringBuilder line = new StringBuilder( this.NewLine )
                .AppendFormat("<{0}>{1}</{0}>", name, value);
            ++this.ChildrenCount;
            this.ostream.Write( line );
        }

        public void AttachProperty(string name, int value)
        {
            this.AttachProperty( name, value.ToString() );
        }

        public bool WithinElement(ElementType type)
        {
            foreach( Element e in this.elementStack )
                if( e.Type == type ) return true;
            return false;
        }

        public bool WithinElement(ElementType type, string name)
        {
            return this.elementStack.Contains( new Element( name, type ) );
        }

        public void PopLast()
        {
            if( this.CurrentType != ElementType.Base )
            {
                Element e = this.elementStack.Pop();
                switch( e.Type )
                {
                    case ElementType.Element:
                    case ElementType.ListElement:
                        StringBuilder line = new StringBuilder(e.ChildrenCount > 0 ? this.NewLineSymbol + this.Indentation : string.Empty )
                            .AppendFormat("</{0}>", e.Name);
                        this.ostream.Write( line );
                        break;
                    case ElementType.List:
                        --this.noneIndents;
                        break;
                }
            }
        }

        public void PopUntilMatch(ElementType type)
        {
            Assert.IsTrue( this.WithinElement( type ), string.Format( "[{0}] not found", type ) );

            while( this.CurrentType != type )
                this.PopLast();
        }

        public void PopUntilMatch(ElementType type, string name)
        {
            Assert.IsTrue( this.WithinElement( type, name ), string.Format( "[{0} : {1}] not found", name, type ) );
            
            while( this.CurrentType != type || this.CurrentName != name )
                this.PopLast();
        }

        public void WriteHeader()
        {
            StringBuilder line = new StringBuilder()
                .AppendFormat("<?xml version=\"1.0\" encoding=\"{0}\"?>", this.ostream.Encoding.HeaderName);
            this.ostream.Write( line );
        }

        public void WriteFooter()
        {
            // do nothing in this implementation
        }

        public void Dispose()
        {
            this.ostream.Dispose();
        }
    }
}
