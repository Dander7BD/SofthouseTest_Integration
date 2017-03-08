using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkupIntegrationGUI
{
    public partial class SendingLundgrenLBForm : Form
    {
        private ReceivingXMLForm xmlReciever = null;
        private ReceivingJSONForm jsonReciever = null;

        private ReceivingXMLForm XMLReciever
        {
            get
            {
                if( this.xmlReciever == null || this.xmlReciever.IsDisposed )
                {
                    this.xmlReciever = new ReceivingXMLForm();
                    this.xmlReciever.Show();
                    this.Focus();
                }
                return this.xmlReciever;
            }
        }

        private ReceivingJSONForm JSONReciever
        {
            get
            {
                if( this.jsonReciever == null || this.jsonReciever.IsDisposed )
                {
                    this.jsonReciever = new ReceivingJSONForm();
                    this.jsonReciever.Show();
                    this.Focus();
                }
                return this.jsonReciever;
            }
        }

        public SendingLundgrenLBForm()
        {
            InitializeComponent();
        }

        private void updateRecievers(string text)
        {
            this.XMLReciever.FeedLundgrenLB( text );
            this.JSONReciever.FeedLundgrenLB( text );
        }

        private void edit_TextChanged(object sender, EventArgs e)
        {
            this.updateRecievers( this.edit.Text );
        }
    }
}
