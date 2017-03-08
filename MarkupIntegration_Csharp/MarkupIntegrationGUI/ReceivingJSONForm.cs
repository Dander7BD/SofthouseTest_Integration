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
    public partial class ReceivingJSONForm : Form
    {
        public ReceivingJSONForm()
        {
            InitializeComponent();
        }

        public void FeedLundgrenLB(string text)
        {
            using( LundgrenLBMReader source = new LundgrenLBMReader( new StringReader( text ) ) )
            {
                StringBuilder xml = new StringBuilder();
                using( JSONWriter translator = new JSONWriter( new StringWriter( xml ) ) )
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
