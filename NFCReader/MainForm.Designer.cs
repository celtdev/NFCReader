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
            this.comList = new System.Windows.Forms.ToolStripComboBox();
            this.startButton = new System.Windows.Forms.ToolStripButton();
            this.stopButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.getReaderInfo = new System.Windows.Forms.ToolStripButton();
            this.getAID = new System.Windows.Forms.ToolStripButton();
            this.updateCOM = new System.Windows.Forms.ToolStripButton();
            this.msgDetails = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateCOM,
            this.comList,
            this.startButton,
            this.stopButton,
            this.toolStripSeparator1,
            this.getReaderInfo,
            this.getAID});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // comList
            // 
            this.comList.Name = "comList";
            this.comList.Size = new System.Drawing.Size(121, 25);
            // 
            // startButton
            // 
            this.startButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.startButton.Image = ((System.Drawing.Image)(resources.GetObject("startButton.Image")));
            this.startButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(86, 22);
            this.startButton.Text = "Start Listening";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stopButton.Image = ((System.Drawing.Image)(resources.GetObject("stopButton.Image")));
            this.stopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(86, 22);
            this.stopButton.Text = "Stop Listening";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // getReaderInfo
            // 
            this.getReaderInfo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.getReaderInfo.Image = ((System.Drawing.Image)(resources.GetObject("getReaderInfo.Image")));
            this.getReaderInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.getReaderInfo.Name = "getReaderInfo";
            this.getReaderInfo.Size = new System.Drawing.Size(92, 22);
            this.getReaderInfo.Text = "Get Reader Info";
            this.getReaderInfo.Click += new System.EventHandler(this.getReaderInfo_Click);
            // 
            // getAID
            // 
            this.getAID.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.getAID.Image = ((System.Drawing.Image)(resources.GetObject("getAID.Image")));
            this.getAID.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.getAID.Name = "getAID";
            this.getAID.Size = new System.Drawing.Size(51, 22);
            this.getAID.Text = "Get AID";
            this.getAID.Click += new System.EventHandler(this.getAID_Click);
            // 
            // updateCOM
            // 
            this.updateCOM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.updateCOM.Image = ((System.Drawing.Image)(resources.GetObject("updateCOM.Image")));
            this.updateCOM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.updateCOM.Name = "updateCOM";
            this.updateCOM.Size = new System.Drawing.Size(101, 22);
            this.updateCOM.Text = "Update COM List";
            this.updateCOM.Click += new System.EventHandler(this.updateCOM_Click);
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
        private System.Windows.Forms.ToolStripComboBox comList;
        private System.Windows.Forms.ToolStripButton startButton;
        private System.Windows.Forms.ToolStripButton stopButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton getReaderInfo;
        private System.Windows.Forms.ToolStripButton getAID;
        private System.Windows.Forms.ToolStripButton updateCOM;
        private System.Windows.Forms.RichTextBox msgDetails;
    }
}

