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
            using( LundgrenLBMReader source = new LundgrenLBMReader( new StringReader( text ) ) )
            {
                StringBuilder xml = new StringBuilder();
                using( XMLWriter translator = new XMLWriter( new StringWriter( xml ) ) )
                {
                    try
                    {
                        //translator.NewLineSymbol = "\n";
                        translator.IndentationSymbol = "  ";
                        source.TranslateTo( translator );
                        this.output.Text = xml.ToString();
                    }
                    catch( Exception ex )
                    {
                        this.output.Text = ex.Message;
                    }
                }
            }
        }
    }
}
