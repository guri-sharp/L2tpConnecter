namespace L2TPConnecter
{
    partial class ConnectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionDialog));
            this.closeButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.logPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.collapseButton = new System.Windows.Forms.Button();
            this.layoutPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.logPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.layoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // closeButton
            // 
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // logTextBox
            // 
            this.logTextBox.BackColor = System.Drawing.Color.DimGray;
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.logTextBox, "logTextBox");
            this.logTextBox.ForeColor = System.Drawing.Color.White;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            // 
            // logPanel
            // 
            this.logPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logPanel.Controls.Add(this.logTextBox);
            resources.ApplyResources(this.logPanel, "logPanel");
            this.logPanel.Name = "logPanel";
            // 
            // titleLabel
            // 
            resources.ApplyResources(this.titleLabel, "titleLabel");
            this.titleLabel.Name = "titleLabel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.collapseButton);
            this.panel4.Name = "panel4";
            // 
            // collapseButton
            // 
            resources.ApplyResources(this.collapseButton, "collapseButton");
            this.collapseButton.Name = "collapseButton";
            this.collapseButton.UseVisualStyleBackColor = true;
            this.collapseButton.Click += new System.EventHandler(this.collapseButton_Click);
            // 
            // layoutPanel
            // 
            resources.ApplyResources(this.layoutPanel, "layoutPanel");
            this.layoutPanel.Controls.Add(this.panel1);
            this.layoutPanel.Controls.Add(this.panel2);
            this.layoutPanel.Controls.Add(this.panel3);
            this.layoutPanel.Controls.Add(this.panel4);
            this.layoutPanel.Controls.Add(this.titleLabel);
            this.layoutPanel.Name = "layoutPanel";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.closeButton);
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.progressBar1);
            this.panel2.Name = "panel2";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.logPanel);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // ConnectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.layoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConnectionDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectionDialog_FormClosing);
            this.Load += new System.EventHandler(this.ConnectionDialog_Load);
            this.Shown += new System.EventHandler(this.ConnectionDialog_Shown);
            this.logPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.Panel logPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button collapseButton;
        private System.Windows.Forms.Panel layoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}