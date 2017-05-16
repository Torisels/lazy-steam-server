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
            this.SuspendLayout();
            // 
            // btnUdpStart
            // 
            this.btnUdpStart.Location = new System.Drawing.Point(12, 235);
            this.btnUdpStart.Name = "btnUdpStart";
            this.btnUdpStart.Size = new System.Drawing.Size(75, 23);
            this.btnUdpStart.TabIndex = 0;
            this.btnUdpStart.Text = "Start UDP";
            this.btnUdpStart.UseVisualStyleBackColor = true;
            this.btnUdpStart.Click += new System.EventHandler(this.btnUdpStart_Click);
            // 
            // btnTcpStart
            // 
            this.btnTcpStart.Location = new System.Drawing.Point(223, 235);
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
            this.textBoxLog.Location = new System.Drawing.Point(12, 12);
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.Size = new System.Drawing.Size(294, 217);
            this.textBoxLog.TabIndex = 2;
            this.textBoxLog.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 235);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 270);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.btnTcpStart);
            this.Controls.Add(this.btnUdpStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "App";
            this.Text = "App";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUdpStart;
        private System.Windows.Forms.Button btnTcpStart;
        private RichTextBox textBoxLog;
        private Button button1;
        private NotifyIcon notifyIcon1;
    }
}