using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkupIntegration
{
    /*
     * Lundgren's Line Based Markup language Reader
     *  P|firstname|lastname        -> person
     *  T|mobile|landline           -> phone
     *  A|street|city|zipcode       -> adress
     *  F|name|born                 -> family
     *  P can be followed by T, A and F
     *  F can be followed by T and A
     */
    public class LundgrenLBMReader : IMLReader
    {
        private struct PersonLine
        {
            public PersonLine(string line)
            {
                string[] split = line.Split('|');
                this.firstname = split.Length > 1 ? split[1] : String.Empty;
                this.lastname  = split.Length > 2 ? split[2] : String.Empty;
            }

            public string firstname;
            public string lastname;
        }
        private struct PhoneLine
        {
            public PhoneLine(string line)
            {
                string[] split  = line.Split('|');
                this.mobile     = split.Length > 1 ? split[1] : String.Empty;
                this.landline   = split.Length > 2 ? split[2] : String.Empty;
            }

            public string mobile;
            public string landline;
        }
        private struct AddressLine
        {
            public AddressLine(string line)
            {
                string[] split = line.Split('|');
                this.street     = split.Length > 1 ? split[1] : String.Empty;
                this.city       = split.Length > 2 ? split[2] : String.Empty;
                this.zipcode    = split.Length > 3 ? split[3] : String.Empty;
            }

            public string street;
            public string city;
            public string zipcode;
        }
        private struct FamilyLine
        {
            public FamilyLine(string line)
            {
                string[] split = line.Split('|');
                this.name = split.Length > 1 ? split[1] : String.Empty;
                this.born = float.NaN;
                if( split.Length > 2 ) float.TryParse(split[2], out this.born);
            }

            public string name;
            public float born;
        }

        private TextReader istream;

        public LundgrenLBMReader(TextReader istream)
        {
            this.istream = istream;
        }

        public void Dispose()
        {
            this.istream.Dispose();
        }

        public IMLWriter TranslateTo(IMLWriter output)
        {
            using( this.istream )
            {
                string line = string.Empty;
                output.WriteHeader();
                output.PushElement( "people" );
                while( this.istream.Peek() != -1 )
                {
                    line = this.istream.ReadLine().Trim();
                    if( line.Length > 2 && line[1] == '|' )
                    {
                        switch( line[0] )
                        {
                            case 'P':
                                if( output.WithinElement( ElementType.List, "person" ) )
                                    output.PopUntilMatch( ElementType.List, "person" );
                                else
                                    output.PushList( "person" );
                                output.PushListElement();
                                PersonLine person = new PersonLine(line);
                                if( person.firstname.Length > 0 ) output.AttachProperty( "firstname", person.firstname );
                                if( person.lastname.Length > 0 ) output.AttachProperty( "lastname", person.lastname );
                                break;
                            case 'T':
                                PhoneLine phone = new PhoneLine(line);
                                output.PushElement( "phone" );
                                if( phone.mobile.Length > 0 ) output.AttachProperty( "mobile", phone.mobile );
                                if( phone.landline.Length > 0 ) output.AttachProperty( "landline", phone.landline );
                                output.PopLast();
                                break;
                            case 'A':
                                AddressLine adress = new AddressLine(line);
                                output.PushElement( "address" );
                                if( adress.street.Length > 0 ) output.AttachProperty( "street", adress.street );
                                if( adress.city.Length > 0 ) output.AttachProperty( "city", adress.city );
                                if( adress.zipcode.Length > 0 ) output.AttachProperty( "zipcode", adress.zipcode );
                                output.PopLast();
                                break;
                            case 'F':
                                if( output.WithinElement( ElementType.List, "family" ) )
                                    output.PopUntilMatch( ElementType.List, "family" );
                                else
                                    output.PushList( "family" );

                                output.PushListElement();
                                FamilyLine family = new FamilyLine(line);
                                if( family.name.Length > 0 ) output.AttachProperty( "name", family.name );
                                if( family.born != float.NaN ) output.AttachProperty( "born", (int)family.born );
                                break;
                        }
                    }
                }
                output.PopUntilMatch(ElementType.Base);
                output.WriteFooter();
            }

            return output;
        }
    }
}
