using System;
using System.Runtime.Serialization;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace MarkupIntegration
{
    /*
     * Lundgren's Line Based syntax
     *  P|firstname|lastname        -> person
     *  T|mobile|landline           -> phone
     *  A|street|city|zipcode       -> adress
     *  F|name|born                 -> family
     *  P can be followed by T, A and F
     *  F can be followed by T and A
     */

    public class LundgrenLBReserializer : ISerializable, IDisposable, IXmlSerializable
    {
        internal class ReaderWrapper
        {
            private TextReader istream;
            private string nextLine;

            internal ReaderWrapper(TextReader istream)
            {
                this.istream = istream;
                this.ReadLine();
            }

            internal void Dispose()
            {
                this.istream.Dispose();
            }

            internal bool AtEnd()
            {
                return Object.ReferenceEquals( this.nextLine, null );
            }

            internal string PeekLine()
            {
                return this.nextLine;
            }

            internal string ReadLine()
            {
                string line = this.nextLine;

                if( this.istream.Peek() != -1 )
                    this.nextLine = this.istream.ReadLine().Trim();
                else
                    this.nextLine = null;

                return line;
            }
        }

        public class Phone : ISerializable, IXmlSerializable
        {
            private string mobile;
            private string landline;

            internal Phone(string line)
            {
                string[] split  = line.Split('|');
                this.mobile = split.Length > 1 ? split[1] : String.Empty;
                this.landline = split.Length > 2 ? split[2] : String.Empty;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if(this.mobile.Length > 0) info.AddValue( "mobile", this.mobile );
                if(this.landline.Length > 0) info.AddValue( "landline", this.landline );
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement( "phone" );
                if( this.mobile.Length > 0 ) writer.WriteElementString( "mobile", this.mobile );
                if( this.landline.Length > 0 ) writer.WriteElementString( "landline", this.landline );
                writer.WriteEndElement();
            }
        }

        public class Adress : ISerializable, IXmlSerializable
        {
            private string street;
            private string city;
            private string zipcode;

            internal Adress(string line)
            {
                string[] split  = line.Split('|');
                this.street     = split.Length > 1 ? split[1] : String.Empty;
                this.city       = split.Length > 2 ? split[2] : String.Empty;
                this.zipcode    = split.Length > 3 ? split[3] : String.Empty;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if( this.street.Length > 0 )    info.AddValue( "street", this.street );
                if( this.city.Length > 0 )      info.AddValue( "city", this.city );
                if( this.zipcode.Length > 0 )   info.AddValue( "zipcode", this.zipcode );
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement( "adress" );
                if( this.street.Length > 0 ) writer.WriteElementString( "street", this.street );
                if( this.city.Length > 0 ) writer.WriteElementString( "city", this.city );
                if( this.zipcode.Length > 0 ) writer.WriteElementString( "zipcode", this.zipcode );
                writer.WriteEndElement();
            }
        }

        public class Family : ISerializable, IXmlSerializable
        {
            private ReaderWrapper istream;
            private string name;
            private int born = 0;
            private bool bornIsRead = false;
            private Phone phone = null;
            private Adress adress = null;

            internal Family(ReaderWrapper istream)
            {
                this.istream = istream;
                string line = this.istream.ReadLine();
                string[] split = line.Split('|');
                this.name = split.Length > 1 ? split[1] : String.Empty;
                if( split.Length > 2 ) this.bornIsRead = int.TryParse( split[2], out this.born );
            }

            private void AddTo(SerializationInfo info)
            {
                if( this.name.Length > 0 ) info.AddValue( "name", this.name );
                if( this.bornIsRead ) info.AddValue( "born", this.born );
                if( !Object.ReferenceEquals( this.adress, null ) ) info.AddValue( "adress", this.adress );
                if( !Object.ReferenceEquals( this.phone, null ) ) info.AddValue( "phone", this.phone );
            }

            private void AddTo(XmlWriter writer)
            {
                if( this.name.Length > 0 ) writer.WriteElementString( "name", this.name );
                if( this.bornIsRead ) writer.WriteElementString( "born", this.born.ToString() );
                if( !Object.ReferenceEquals( this.adress, null ) ) writer.WriteValue( this.adress );
                if( !Object.ReferenceEquals( this.phone, null ) ) writer.WriteValue( this.phone );
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                while( !this.istream.AtEnd() )
                {
                    string line = this.istream.PeekLine();
                    if( line.Length > 2 && line[1] == '|' )
                    {
                        switch( line[0] )
                        {
                            case 'a':
                            case 'A':
                                this.adress = new Adress( line );
                                break;
                            case 't':
                            case 'T':
                                this.phone = new Phone( line );
                                break;
                            default:
                                this.AddTo( info );
                                return;
                        }
                    }
                }
                this.AddTo( info );
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement( "family" );
                while( !this.istream.AtEnd() )
                {
                    string line = this.istream.PeekLine();
                    if( line.Length > 2 && line[1] == '|' )
                    {
                        switch( line[0] )
                        {
                            case 'a':
                            case 'A':
                                this.adress = new Adress( line );
                                break;
                            case 't':
                            case 'T':
                                this.phone = new Phone( line );
                                break;
                            default:
                                this.AddTo( writer );
                                writer.WriteEndElement();
                                return;
                        }
                    }
                }
                this.AddTo( writer );
                writer.WriteEndElement();
            }
        }

        public class Person : ISerializable, IXmlSerializable
        {
            private ReaderWrapper istream;
            private string firstname;
            private string surname;
            private Adress adress = null;
            private Phone phone = null;

            internal Person(ReaderWrapper istream)
            {
                this.istream = istream;
                string line = this.istream.ReadLine();
                string[] split = line.Split('|');
                this.firstname = split.Length > 1 ? split[1] : String.Empty;
                this.surname = split.Length > 2 ? split[2] : String.Empty;
            }

            private void AddSinglesTo(SerializationInfo info)
            {
                if( this.firstname.Length > 0 ) info.AddValue( "firstname", this.firstname );
                if( this.surname.Length > 0 ) info.AddValue( "lastname", this.surname );
                if( !Object.ReferenceEquals( this.adress, null ) ) info.AddValue( "adress", this.adress );
                if( !Object.ReferenceEquals( this.phone, null ) ) info.AddValue( "phone", this.phone );
            }

            private void AddSinglesTo(XmlWriter writer)
            {
                if( this.firstname.Length > 0 ) writer.WriteElementString( "firstname", this.firstname );
                if( this.surname.Length > 0 ) writer.WriteElementString( "lastname", this.surname );
                if( !Object.ReferenceEquals( this.adress, null ) ) writer.WriteValue( this.adress );
                if( !Object.ReferenceEquals( this.phone, null ) ) writer.WriteValue( this.phone );
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                string line;
                while( !this.istream.AtEnd() )
                {
                    line = this.istream.PeekLine();
                    if( line.Length > 2 && line[1] == '|' )
                    {
                        switch( line[0] )
                        {
                            case 'a':
                            case 'A':
                                this.adress = new Adress( line );
                                break;
                            case 't':
                            case 'T':
                                this.phone = new Phone( line );
                                break;
                            case 'f':
                            case 'F':
                                info.AddValue( "family", new Family( this.istream ) );
                                break;
                            default:
                                this.AddSinglesTo( info );
                                return;
                        }
                    }
                }
                this.AddSinglesTo( info );
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement( "person" );
                string line;
                while( !this.istream.AtEnd() )
                {
                    line = this.istream.PeekLine();
                    if( line.Length > 2 && line[1] == '|' )
                    {
                        switch( line[0] )
                        {
                            case 'a':
                            case 'A':
                                this.adress = new Adress( line );
                                break;
                            case 't':
                            case 'T':
                                this.phone = new Phone( line );
                                break;
                            case 'f':
                            case 'F':
                                writer.WriteValue( new Family( this.istream ) );
                                break;
                            default:
                                this.AddSinglesTo( writer );
                                writer.WriteEndElement();
                                return;
                        }
                    }
                }
                this.AddSinglesTo( writer );
                writer.WriteEndElement();
            }
        }

        public class People : ISerializable, IXmlSerializable
        {
            private ReaderWrapper istream;

            internal People(ReaderWrapper istream)
            {
                this.istream = istream;
            }

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                while( !this.istream.AtEnd() )
                {
                    string line = this.istream.PeekLine();
                    if( line.Length > 2
                     && line[1] == '|'
                     && (line[0] == 'p' || line[0] == 'P'))
                        info.AddValue("person", new Person(this.istream));
                }
            }

            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                throw new NotImplementedException();
            }

            public void WriteXml(XmlWriter writer)
            {
                writer.WriteStartElement( "people" );
                while( !this.istream.AtEnd() )
                {
                    string line = this.istream.PeekLine();
                    if( line.Length > 2
                     && line[1] == '|'
                     && (line[0] == 'p' || line[0] == 'P') )
                        writer.WriteValue( new Person( this.istream ) );
                }
                writer.WriteEndElement();
            }
        }

        private ReaderWrapper istream;

        public LundgrenLBReserializer(TextReader istream)
        {
            Assert.IsNotNull( istream );
            this.istream = new ReaderWrapper( istream );
        }

        public void Dispose()
        {
            this.istream.Dispose();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue( "people", new People( this.istream ) );
        }

        public XmlSchema GetSchema()
        {
            return null; // recommended implementation according to documentation
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException("This is a readonly stream and thus will never implement this function.");
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteValue( new People( this.istream ) );
        }
    }
}
