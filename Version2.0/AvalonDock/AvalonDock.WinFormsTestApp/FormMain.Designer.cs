namespace AvalonDock.WinFormsTestApp
{
    partial class FormMain
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
            this.dockingManagerHost = new System.Windows.Forms.Integration.ElementHost();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.themeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAero = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemVS2010 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockingManagerHost
            // 
            this.dockingManagerHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockingManagerHost.Location = new System.Drawing.Point(0, 24);
            this.dockingManagerHost.Name = "dockingManagerHost";
            this.dockingManagerHost.Size = new System.Drawing.Size(673, 423);
            this.dockingManagerHost.TabIndex = 0;
            this.dockingManagerHost.Text = "elementHost1";
            this.dockingManagerHost.Child = null;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.themeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(673, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // themeToolStripMenuItem
            // 
            this.themeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemAero,
            this.menuItemVS2010,
            this.menuItemDefault});
            this.themeToolStripMenuItem.Name = "themeToolStripMenuItem";
            this.themeToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.themeToolStripMenuItem.Text = "Theme";
            // 
            // menuItemAero
            // 
            this.menuItemAero.Name = "menuItemAero";
            this.menuItemAero.Size = new System.Drawing.Size(152, 22);
            this.menuItemAero.Text = "Aero";
            this.menuItemAero.Click += new System.EventHandler(this.menuItemAero_Click);
            // 
            // menuItemVS2010
            // 
            this.menuItemVS2010.Name = "menuItemVS2010";
            this.menuItemVS2010.Size = new System.Drawing.Size(152, 22);
            this.menuItemVS2010.Text = "VS2010";
            this.menuItemVS2010.Click += new System.EventHandler(this.menuItemVS2010_Click);
            // 
            // menuItemDefault
            // 
            this.menuItemDefault.Name = "menuItemDefault";
            this.menuItemDefault.Size = new System.Drawing.Size(152, 22);
            this.menuItemDefault.Text = "Default";
            this.menuItemDefault.Click += new System.EventHandler(this.menuItemDefault_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 447);
            this.Controls.Add(this.dockingManagerHost);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost dockingManagerHost;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem themeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemAero;
        private System.Windows.Forms.ToolStripMenuItem menuItemVS2010;
        private System.Windows.Forms.ToolStripMenuItem menuItemDefault;
    }
}

