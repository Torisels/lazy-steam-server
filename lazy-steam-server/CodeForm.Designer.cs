namespace lazy_steam_server
{
    partial class CodeForm
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
            this.CodeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CodeLabel
            // 
            this.CodeLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CodeLabel.Font = new System.Drawing.Font("Tahoma", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.CodeLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CodeLabel.Location = new System.Drawing.Point(40, 20);
            this.CodeLabel.Name = "CodeLabel";
            this.CodeLabel.Size = new System.Drawing.Size(92, 40);
            this.CodeLabel.TabIndex = 0;
            this.CodeLabel.Text = "1234";
            this.CodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CodeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(173, 81);
            this.Controls.Add(this.CodeLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CodeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Security code";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CodeLabel;
    }
}