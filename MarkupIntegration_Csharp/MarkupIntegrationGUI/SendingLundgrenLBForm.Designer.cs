namespace MarkupIntegrationGUI
{
    partial class SendingLundgrenLBForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if( disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendingLundgrenLBForm));
            this.edit = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // edit
            // 
            this.edit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.edit.Location = new System.Drawing.Point(0, 0);
            this.edit.Multiline = true;
            this.edit.Name = "edit";
            this.edit.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.edit.Size = new System.Drawing.Size(782, 553);
            this.edit.TabIndex = 0;
            this.edit.Text = resources.GetString("edit.Text");
            this.edit.WordWrap = false;
            this.edit.TextChanged += new System.EventHandler(this.edit_TextChanged);
            // 
            // SendingLundgrenLBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.edit);
            this.Name = "SendingLundgrenLBForm";
            this.Text = "LundgrenLBM Editor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox edit;
    }
}

