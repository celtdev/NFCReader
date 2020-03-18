namespace NFCReader
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.prmSelector = new System.Windows.Forms.ToolStripComboBox();
            this.checkReaders = new System.Windows.Forms.ToolStripButton();
            this.msgDetails = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.prmSelector,
            this.checkReaders});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // prmSelector
            // 
            this.prmSelector.Items.AddRange(new object[] {
            "00 CA 00 00 00"});
            this.prmSelector.Name = "prmSelector";
            this.prmSelector.Size = new System.Drawing.Size(121, 25);
            // 
            // checkReaders
            // 
            this.checkReaders.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.checkReaders.Image = ((System.Drawing.Image)(resources.GetObject("checkReaders.Image")));
            this.checkReaders.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.checkReaders.Name = "checkReaders";
            this.checkReaders.Size = new System.Drawing.Size(56, 22);
            this.checkReaders.Text = "Get Data";
            this.checkReaders.Click += new System.EventHandler(this.checkReaders_Click);
            // 
            // msgDetails
            // 
            this.msgDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.msgDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msgDetails.Location = new System.Drawing.Point(0, 25);
            this.msgDetails.Name = "msgDetails";
            this.msgDetails.ReadOnly = true;
            this.msgDetails.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.msgDetails.Size = new System.Drawing.Size(800, 425);
            this.msgDetails.TabIndex = 1;
            this.msgDetails.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.msgDetails);
            this.Controls.Add(this.toolStrip1);
            this.Name = "MainForm";
            this.Text = "NFC Reader";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.RichTextBox msgDetails;
        private System.Windows.Forms.ToolStripButton checkReaders;
        private System.Windows.Forms.ToolStripComboBox prmSelector;
    }
}

