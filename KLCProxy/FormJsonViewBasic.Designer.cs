namespace KLCProxy {
    partial class FormJsonViewBasic {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.textBoxJson = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // textBoxJson
            // 
            this.textBoxJson.DetectUrls = false;
            this.textBoxJson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxJson.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxJson.Location = new System.Drawing.Point(0, 0);
            this.textBoxJson.Name = "textBoxJson";
            this.textBoxJson.Size = new System.Drawing.Size(475, 308);
            this.textBoxJson.TabIndex = 0;
            this.textBoxJson.Text = "";
            // 
            // FormJsonViewBasic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 308);
            this.Controls.Add(this.textBoxJson);
            this.Name = "FormJsonViewBasic";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FormJsonViewBasic";
            this.Shown += new System.EventHandler(this.FormJsonViewBasic_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBoxJson;
    }
}