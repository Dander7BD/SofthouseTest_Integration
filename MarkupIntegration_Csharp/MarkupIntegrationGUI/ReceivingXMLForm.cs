using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MarkupIntegration;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace MarkupIntegrationGUI
{
    public partial class ReceivingXMLForm : Form
    {
        public ReceivingXMLForm()
        {
            InitializeComponent();
        }

        public void FeedLundgrenLB(string text)
        {
            using( LundgrenLBReserializer reader = new LundgrenLBReserializer( new StringReader( text ) ) )
            {
                XmlSerializer serializer = new XmlSerializer(reader.GetType(), new XmlRootAttribute("people"));
                StringBuilder xml = new StringBuilder();
                using( StringWriter writer = new StringWriter( xml ) )
                {
                    try
                    {
                        serializer.Serialize( writer, reader );
                        this.output.Text = xml.ToString();
                    }
                    catch( Exception e )
                    {
                        this.output.Text = e.Message;
                    }
                }
            }
            //using( LundgrenLBMReader source = new LundgrenLBMReader( new StringReader( text ) ) )
            //{
            //    StringBuilder xml = new StringBuilder();
            //    using( XMLWriter translator = new XMLWriter( new StringWriter( xml ) ) )
            //    {
            //        try
            //        {
            //            //translator.NewLineSymbol = "\n";
            //            translator.IndentationSymbol = "  ";
            //            source.TranslateTo( translator );
            //            this.output.Text = xml.ToString();
            //        }
            //        catch( Exception ex )
            //        {
            //            this.output.Text = ex.Message;
            //        }
            //    }
            //}
        }
    }
}
