using System.Windows.Forms;

namespace lazy_steam_server
{
    partial class App
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.btnUdpStart = new System.Windows.Forms.Button();
            this.btnTcpStart = new System.Windows.Forms.Button();
            this.textBoxLog = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.steamTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUdpStart
            // 
            this.btnUdpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUdpStart.Location = new System.Drawing.Point(12, 325);
            this.btnUdpStart.Name = "btnUdpStart";
            this.btnUdpStart.Size = new System.Drawing.Size(75, 23);
            this.btnUdpStart.TabIndex = 0;
            this.btnUdpStart.Text = "Start UDP";
            this.btnUdpStart.UseVisualStyleBackColor = true;
            this.btnUdpStart.Click += new System.EventHandler(this.btnUdpStart_Click);
            // 
            // btnTcpStart
            // 
            this.btnTcpStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTcpStart.Location = new System.Drawing.Point(387, 327);
            this.btnTcpStart.Name = "btnTcpStart";
            this.btnTcpStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnTcpStart.Size = new System.Drawing.Size(83, 23);
            this.btnTcpStart.TabIndex = 1;
            this.btnTcpStart.Text = "Start TCP";
            this.btnTcpStart.UseVisualStyleBackColor = true;
            this.btnTcpStart.Click += new System.EventHandler(this.btnTcpStart_Click);
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.Location = new System.Drawing.Point(12, 12);
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(462, 219);
            this.textBoxLog.TabIndex = 2;
            this.textBoxLog.Text = "";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(204, 327);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Send Code";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // steamTextBox
            // 
            this.steamTextBox.Location = new System.Drawing.Point(16, 278);
            this.steamTextBox.Name = "steamTextBox";
            this.steamTextBox.Size = new System.Drawing.Size(100, 20);
            this.steamTextBox.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 27);
            this.label1.TabIndex = 5;
            this.label1.Text = "You can enter\r\nSteam code below:";
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(486, 360);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.steamTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.btnTcpStart);
            this.Controls.Add(this.btnUdpStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "App";
            this.Text = "App";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUdpStart;
        private System.Windows.Forms.Button btnTcpStart;
        private RichTextBox textBoxLog;
        private Button button1;
        private NotifyIcon notifyIcon1;
        private TextBox steamTextBox;
        private Label label1;
    }
}