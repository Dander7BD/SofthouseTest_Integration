using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    public class JSONWriter : IMLWriter
    {
        private struct Property
        {
            public Property(string name, ElementType type)
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
                if( obj is Property )
                {
                    Property p = (Property)obj;
                    return this.Type.Equals(p.Type) && this.Name.Equals( p.Name );
                }
                return false;
            }
        }

        private TextWriter ostream;
        private Stack<Property> propertyStack;

        public JSONWriter(TextWriter ostream)
        {
            this.ostream = ostream;
            this.propertyStack = new Stack<Property>( 16 );
            this.propertyStack.Push( new Property( "", ElementType.Base ) );
            this.IndentationSymbol = "\t";
        }

        public string CurrentName
        {
            get { return this.propertyStack.Peek().Name; }
        }
        public ElementType CurrentType
        {
            get { return this.propertyStack.Peek().Type; }
        }
        public string NewLineSymbol
        {
            get { return this.ostream.NewLine; }
            set { this.ostream.NewLine = value; }
        }
        public string IndentationSymbol { get; set; }

        private int Count
        {
            get { return this.propertyStack.Peek().ChildrenCount; }
            set
            {
                Property p = this.propertyStack.Pop();
                p.ChildrenCount = value;
                this.propertyStack.Push( p );
            }
        }
        private int IndentationLevel
        {
            get { return this.propertyStack.Count; }
        }
        private string Indentation
        {
            get
            {
                StringBuilder indentation = new StringBuilder(this.IndentationSymbol.Length * this.IndentationLevel);
                for( int i = 0; i < this.IndentationLevel; ++i )
                    indentation.Append( this.IndentationSymbol );
                return indentation.ToString();
            }
        }
        private string NewLine
        {
            get
            {
                if( this.Count > 0 )
                    return string.Format( ",{0}{1}", this.NewLineSymbol, this.Indentation );
                else
                    return string.Format( "{0}{1}", this.NewLineSymbol, this.Indentation );
            }
        }

        public void PushElement(string name)
        {
            Assert.IsTrue( this.CurrentType != ElementType.List, "Lists only takes ListElements." );

            StringBuilder line = new StringBuilder(this.NewLine).
                AppendFormat("\"{0}\" :{1}{2}{{", name, this.NewLineSymbol, this.Indentation);
            ++this.Count;
            this.propertyStack.Push( new Property( name, ElementType.Element ) );
            this.ostream.Write( line );
        }

        public void PushList(string name)
        {
            Assert.IsTrue( this.CurrentType != ElementType.List, "Lists only takes ListElements." );

            StringBuilder line = new StringBuilder(this.NewLine).
                AppendFormat("\"{0}\" :{1}{2}[", name, this.NewLineSymbol, this.Indentation);
            ++this.Count;
            this.propertyStack.Push( new Property( name, ElementType.List ) );
            this.ostream.Write( line );
        }

        public void PushListElement()
        {
            Assert.IsTrue( this.CurrentType == ElementType.List, "ListElements can only be within Lists." );
            
            string line = this.NewLine + '{';
            ++this.Count;
            this.propertyStack.Push( new Property( this.CurrentName, ElementType.ListElement ) );
            this.ostream.Write( line );
        }

        public void AttachProperty(string name, string value)
        {
            Assert.IsTrue( this.CurrentType != ElementType.List, "Can not apply properties on List." );

            StringBuilder line = new StringBuilder( this.NewLine )
                .AppendFormat("\"{0}\" : \"{1}\"", name, value);
            ++this.Count;
            this.ostream.Write( line );
        }

        public void AttachProperty(string name, int value)
        {
            Assert.IsTrue( this.CurrentType != ElementType.List, "Can not apply properties on List." );

            StringBuilder line = new StringBuilder( this.NewLine )
                .AppendFormat("\"{0}\" : {1}", name, value.ToString());
            ++this.Count;
            this.ostream.Write( line );
        }

        public bool WithinElement(ElementType type)
        {
            foreach( Property p in this.propertyStack )
                if( p.Type == type ) return true;
            return false;
        }

        public bool WithinElement(ElementType type, string name)
        {
            return this.propertyStack.Contains( new Property( name, type ) );
        }

        public void PopLast()
        {
            if( this.CurrentType != ElementType.Base )
            {
                Property p = this.propertyStack.Pop();
                StringBuilder line = new StringBuilder(p.ChildrenCount > 0 ? this.NewLineSymbol + this.Indentation : string.Empty);
                switch( p.Type )
                {
                    case ElementType.Element:
                    case ElementType.ListElement:
                        line.Append( "}" );
                        break;
                    case ElementType.List:
                        line.Append( "]" );
                        break;
                }
                this.ostream.Write( line );
            }
        }

        public void PopUntilMatch(ElementType type)
        {
            Assert.IsTrue( this.WithinElement( type ), string.Format("[{0}] was not found.", type) );

            while( this.CurrentType != type )
                this.PopLast();
        }

        public void PopUntilMatch(ElementType type, string name)
        {
            Assert.IsTrue( this.WithinElement( type, name ), string.Format( "[{0} : {1}] was not found.", name, type ) );

            while( this.CurrentType != type || this.CurrentName != name)
                this.PopLast();
        }

        public void WriteHeader()
        {
            this.ostream.Write( '{' );
        }

        public void WriteFooter()
        {
            this.ostream.Write( this.NewLineSymbol + '}' );
        }        

        public void Dispose()
        {
            this.ostream.Dispose();
        }
    }
}
