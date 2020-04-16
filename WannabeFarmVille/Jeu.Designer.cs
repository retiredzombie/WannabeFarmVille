namespace WannabeFarmVille
{
    partial class Jeu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Jeu));
            this.menu_haut = new System.Windows.Forms.MenuStrip();
            this.dateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.animauxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mouton20ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grizzly30ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lion35ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buffle30ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rhinocéros40ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buffle40ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.déchetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conciergesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.embaucherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_haut.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu_haut
            // 
            this.menu_haut.BackColor = System.Drawing.Color.Peru;
            this.menu_haut.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("menu_haut.BackgroundImage")));
            this.menu_haut.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menu_haut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateToolStripMenuItem,
            this.toolStripMenuItem1,
            this.animauxToolStripMenuItem,
            this.déchetsToolStripMenuItem,
            this.conciergesToolStripMenuItem});
            this.menu_haut.Location = new System.Drawing.Point(0, 0);
            this.menu_haut.Name = "menu_haut";
            this.menu_haut.Size = new System.Drawing.Size(1264, 32);
            this.menu_haut.TabIndex = 0;
            this.menu_haut.Text = "menuStrip1";
            // 
            // dateToolStripMenuItem
            // 
            this.dateToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.dateToolStripMenuItem.Name = "dateToolStripMenuItem";
            this.dateToolStripMenuItem.Size = new System.Drawing.Size(142, 28);
            this.dateToolStripMenuItem.Text = "24 mai 2020";
            this.dateToolStripMenuItem.Click += new System.EventHandler(this.dateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(70, 28);
            this.toolStripMenuItem1.Text = "100$";
            // 
            // animauxToolStripMenuItem
            // 
            this.animauxToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouton20ToolStripMenuItem,
            this.grizzly30ToolStripMenuItem,
            this.lion35ToolStripMenuItem,
            this.buffle30ToolStripMenuItem,
            this.rhinocéros40ToolStripMenuItem,
            this.buffle40ToolStripMenuItem});
            this.animauxToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.animauxToolStripMenuItem.Name = "animauxToolStripMenuItem";
            this.animauxToolStripMenuItem.Size = new System.Drawing.Size(128, 28);
            this.animauxToolStripMenuItem.Text = "0 Animaux";
            // 
            // mouton20ToolStripMenuItem
            // 
            this.mouton20ToolStripMenuItem.Enabled = false;
            this.mouton20ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mouton20ToolStripMenuItem.Name = "mouton20ToolStripMenuItem";
            this.mouton20ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.mouton20ToolStripMenuItem.Text = "+ Mouton 20$";
            // 
            // grizzly30ToolStripMenuItem
            // 
            this.grizzly30ToolStripMenuItem.Enabled = false;
            this.grizzly30ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grizzly30ToolStripMenuItem.Name = "grizzly30ToolStripMenuItem";
            this.grizzly30ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.grizzly30ToolStripMenuItem.Text = "+ Grizzly 30$";
            // 
            // lion35ToolStripMenuItem
            // 
            this.lion35ToolStripMenuItem.Enabled = false;
            this.lion35ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lion35ToolStripMenuItem.Name = "lion35ToolStripMenuItem";
            this.lion35ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.lion35ToolStripMenuItem.Text = "+ Lion 35$";
            // 
            // buffle30ToolStripMenuItem
            // 
            this.buffle30ToolStripMenuItem.Enabled = false;
            this.buffle30ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffle30ToolStripMenuItem.Name = "buffle30ToolStripMenuItem";
            this.buffle30ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.buffle30ToolStripMenuItem.Text = "+ Licorne 50$";
            // 
            // rhinocéros40ToolStripMenuItem
            // 
            this.rhinocéros40ToolStripMenuItem.Enabled = false;
            this.rhinocéros40ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rhinocéros40ToolStripMenuItem.Name = "rhinocéros40ToolStripMenuItem";
            this.rhinocéros40ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.rhinocéros40ToolStripMenuItem.Text = "+ Rhinocéros 40$";
            // 
            // buffle40ToolStripMenuItem
            // 
            this.buffle40ToolStripMenuItem.Enabled = false;
            this.buffle40ToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buffle40ToolStripMenuItem.Name = "buffle40ToolStripMenuItem";
            this.buffle40ToolStripMenuItem.Size = new System.Drawing.Size(241, 26);
            this.buffle40ToolStripMenuItem.Text = "+ Buffle 40$";
            // 
            // déchetsToolStripMenuItem
            // 
            this.déchetsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.déchetsToolStripMenuItem.Name = "déchetsToolStripMenuItem";
            this.déchetsToolStripMenuItem.Size = new System.Drawing.Size(123, 28);
            this.déchetsToolStripMenuItem.Text = "0 Déchets";
            // 
            // conciergesToolStripMenuItem
            // 
            this.conciergesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.embaucherToolStripMenuItem});
            this.conciergesToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.conciergesToolStripMenuItem.Name = "conciergesToolStripMenuItem";
            this.conciergesToolStripMenuItem.Size = new System.Drawing.Size(155, 28);
            this.conciergesToolStripMenuItem.Text = "0 Concierges";
            // 
            // embaucherToolStripMenuItem
            // 
            this.embaucherToolStripMenuItem.Enabled = false;
            this.embaucherToolStripMenuItem.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.embaucherToolStripMenuItem.Name = "embaucherToolStripMenuItem";
            this.embaucherToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
            this.embaucherToolStripMenuItem.Text = "Embaucher";
            this.embaucherToolStripMenuItem.Click += new System.EventHandler(this.embaucherToolStripMenuItem_Click);
            // 
            // Jeu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 921);
            this.Controls.Add(this.menu_haut);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menu_haut;
            this.Name = "Jeu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TIGER KING: THE GAME";
            this.Load += new System.EventHandler(this.Jeu_Load);
            this.menu_haut.ResumeLayout(false);
            this.menu_haut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu_haut;
        private System.Windows.Forms.ToolStripMenuItem dateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem animauxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem déchetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conciergesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem embaucherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mouton20ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grizzly30ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lion35ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buffle30ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rhinocéros40ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buffle40ToolStripMenuItem;
    }
}

