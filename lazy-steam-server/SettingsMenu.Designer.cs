namespace lazy_steam_server
{
    partial class SettingsMenu
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
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSteamRunning = new System.Windows.Forms.TextBox();
            this.textBoxSteamNotRunning = new System.Windows.Forms.TextBox();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.checkBoxAutoPasteToSteam = new System.Windows.Forms.CheckBox();
            this.checkBoxRunAtStartUp = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Balloon tip tool duration (steam is running):\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(223, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Balloon tip tool duration (steam is not running):\r\n\r\n";
            // 
            // textBoxSteamRunning
            // 
            this.textBoxSteamRunning.Location = new System.Drawing.Point(239, 10);
            this.textBoxSteamRunning.MaxLength = 5;
            this.textBoxSteamRunning.Name = "textBoxSteamRunning";
            this.textBoxSteamRunning.Size = new System.Drawing.Size(83, 20);
            this.textBoxSteamRunning.TabIndex = 2;
            this.textBoxSteamRunning.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSteamRunning_KeyPress);
            // 
            // textBoxSteamNotRunning
            // 
            this.textBoxSteamNotRunning.Location = new System.Drawing.Point(239, 41);
            this.textBoxSteamNotRunning.MaxLength = 5;
            this.textBoxSteamNotRunning.Name = "textBoxSteamNotRunning";
            this.textBoxSteamNotRunning.Size = new System.Drawing.Size(83, 20);
            this.textBoxSteamNotRunning.TabIndex = 3;
            this.textBoxSteamNotRunning.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxSteamNotRunning_KeyPress);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSaveSettings.Location = new System.Drawing.Point(133, 127);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveSettings.TabIndex = 4;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // checkBoxAutoPasteToSteam
            // 
            this.checkBoxAutoPasteToSteam.AutoSize = true;
            this.checkBoxAutoPasteToSteam.Location = new System.Drawing.Point(16, 73);
            this.checkBoxAutoPasteToSteam.Name = "checkBoxAutoPasteToSteam";
            this.checkBoxAutoPasteToSteam.Size = new System.Drawing.Size(120, 17);
            this.checkBoxAutoPasteToSteam.TabIndex = 5;
            this.checkBoxAutoPasteToSteam.Text = "Auto-paste to steam";
            this.checkBoxAutoPasteToSteam.UseVisualStyleBackColor = true;
            this.checkBoxAutoPasteToSteam.CheckedChanged += new System.EventHandler(this.checkBoxAutoPasteToSteam_CheckedChanged);
            // 
            // checkBoxRunAtStartUp
            // 
            this.checkBoxRunAtStartUp.AutoSize = true;
            this.checkBoxRunAtStartUp.Location = new System.Drawing.Point(16, 96);
            this.checkBoxRunAtStartUp.Name = "checkBoxRunAtStartUp";
            this.checkBoxRunAtStartUp.Size = new System.Drawing.Size(93, 17);
            this.checkBoxRunAtStartUp.TabIndex = 6;
            this.checkBoxRunAtStartUp.Text = "Run at startup";
            this.checkBoxRunAtStartUp.UseVisualStyleBackColor = true;
            this.checkBoxRunAtStartUp.CheckedChanged += new System.EventHandler(this.checkBoxRunAtStartUp_CheckedChanged);
            // 
            // SettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 162);
            this.Controls.Add(this.checkBoxRunAtStartUp);
            this.Controls.Add(this.checkBoxAutoPasteToSteam);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.textBoxSteamNotRunning);
            this.Controls.Add(this.textBoxSteamRunning);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsMenu";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSteamRunning;
        private System.Windows.Forms.TextBox textBoxSteamNotRunning;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.CheckBox checkBoxAutoPasteToSteam;
        private System.Windows.Forms.CheckBox checkBoxRunAtStartUp;
    }
}