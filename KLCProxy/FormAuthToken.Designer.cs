namespace KLCProxy
{
    partial class FormAuthToken
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtAuthToken = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAuthSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Token:";
            // 
            // txtAuthToken
            // 
            this.txtAuthToken.Location = new System.Drawing.Point(59, 58);
            this.txtAuthToken.Name = "txtAuthToken";
            this.txtAuthToken.Size = new System.Drawing.Size(100, 20);
            this.txtAuthToken.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(268, 45);
            this.label2.TabIndex = 3;
            this.label2.Text = "Your authorization token is loaded in when any VSA command is sent to KLCProxy. Y" +
    "ou can use this screen to apply your last token without relaunching from VSA.";
            // 
            // btnAuthSave
            // 
            this.btnAuthSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAuthSave.Location = new System.Drawing.Point(165, 56);
            this.btnAuthSave.Name = "btnAuthSave";
            this.btnAuthSave.Size = new System.Drawing.Size(100, 23);
            this.btnAuthSave.TabIndex = 4;
            this.btnAuthSave.Text = "Save for session";
            this.btnAuthSave.UseVisualStyleBackColor = true;
            this.btnAuthSave.Click += new System.EventHandler(this.btnAuthSave_Click);
            // 
            // FormAuthToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 89);
            this.Controls.Add(this.btnAuthSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAuthToken);
            this.Controls.Add(this.label1);
            this.Name = "FormAuthToken";
            this.Text = "Auth Token";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAuthToken;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAuthSave;
    }
}