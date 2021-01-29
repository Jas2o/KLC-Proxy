namespace KLCProxy
{
    partial class FormKaseyaScreenshot
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCapture = new System.Windows.Forms.Button();
            this.linkKaseya = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listKaseya = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.timerKaseya = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCapture);
            this.groupBox1.Controls.Add(this.linkKaseya);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.listKaseya);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 250);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screenshot";
            // 
            // btnCapture
            // 
            this.btnCapture.Location = new System.Drawing.Point(234, 16);
            this.btnCapture.Name = "btnCapture";
            this.btnCapture.Size = new System.Drawing.Size(53, 56);
            this.btnCapture.TabIndex = 13;
            this.btnCapture.Text = "Capture";
            this.btnCapture.UseVisualStyleBackColor = true;
            this.btnCapture.Click += new System.EventHandler(this.btnCapture_Click);
            // 
            // linkKaseya
            // 
            this.linkKaseya.Location = new System.Drawing.Point(6, 16);
            this.linkKaseya.Name = "linkKaseya";
            this.linkKaseya.Size = new System.Drawing.Size(75, 30);
            this.linkKaseya.TabIndex = 12;
            this.linkKaseya.TabStop = true;
            this.linkKaseya.Text = "Open Folder";
            this.linkKaseya.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkKaseya_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox1.Location = new System.Drawing.Point(6, 78);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(281, 166);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // listKaseya
            // 
            this.listKaseya.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listKaseya.FormattingEnabled = true;
            this.listKaseya.Location = new System.Drawing.Point(90, 16);
            this.listKaseya.Name = "listKaseya";
            this.listKaseya.Size = new System.Drawing.Size(138, 56);
            this.listKaseya.TabIndex = 5;
            this.listKaseya.SelectedIndexChanged += new System.EventHandler(this.listKaseya_SelectedIndexChanged);
            this.listKaseya.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listKaseya_MouseDoubleClick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(6, 49);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // timerKaseya
            // 
            this.timerKaseya.Interval = 10000;
            this.timerKaseya.Tick += new System.EventHandler(this.timerKaseya_Tick);
            // 
            // FormKaseyaScreenshot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 271);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormKaseyaScreenshot";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Screenshot Tool";
            this.Load += new System.EventHandler(this.FormKaseyaScreenshot_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCapture;
        private System.Windows.Forms.LinkLabel linkKaseya;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListBox listKaseya;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Timer timerKaseya;
    }
}