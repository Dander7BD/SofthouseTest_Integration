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
                this.born = split.Length > 2 ? split[2] : String.Empty;
            }

            public string name;
            public string born;
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
                output.CreateHeader();
                output.CreateObject( "people" );
                while( this.istream.Peek() != -1 )
                {
                    line = this.istream.ReadLine().Trim();
                    switch(line[0])
                    {
                        case 'P':
                            while( output.CurrentObject != "people" )
                                output.CloseObject();
                            output.CreateObject( "person" );
                            PersonLine person = new PersonLine(line);
                            if(person.firstname.Length > 0)  output.AddProperty("firstname", person.firstname);
                            if(person.lastname.Length > 0)   output.AddProperty("lastname", person.lastname);
                            break;
                        case 'T':
                            PhoneLine phone = new PhoneLine(line);
                            if( phone.mobile.Length > 0 )   output.AddProperty( "mobile", phone.mobile );
                            if( phone.landline.Length > 0 ) output.AddProperty( "landline", phone.landline );
                            break;
                        case 'A':
                            AddressLine adress = new AddressLine(line);
                            output.CreateObject( "address" );
                            if( adress.street.Length > 0 )  output.AddProperty( "street", adress.street );
                            if( adress.city.Length > 0 )    output.AddProperty( "city", adress.city );
                            if( adress.zipcode.Length > 0 ) output.AddProperty( "zipcode", adress.zipcode );
                            output.CloseObject();
                            break;
                        case 'F':
                            while( output.CurrentObject != "person" )
                                output.CloseObject();
                            output.CreateObject( "family" );
                            FamilyLine family = new FamilyLine(line);
                            if( family.name.Length > 0 ) output.AddProperty( "name", family.name );
                            if( family.born.Length > 0 ) output.AddProperty( "born", family.born );
                            break;
                    }
                }
                output.CloseAll();
                output.CreateFooter();
            }

            return output;
        }
    }
}
