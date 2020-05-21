namespace WannabeFarmVille
{
    partial class InfoVisiteur
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoVisiteur));
            this.PnlVisiteurs = new System.Windows.Forms.Panel();
            this.lblTemps = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblVisNom = new System.Windows.Forms.Label();
            this.IconVisFem = new System.Windows.Forms.PictureBox();
            this.IconVisHom = new System.Windows.Forms.PictureBox();
            this.PnlVisiteurs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconVisFem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconVisHom)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlVisiteurs
            // 
            this.PnlVisiteurs.BackColor = System.Drawing.Color.Transparent;
            this.PnlVisiteurs.Controls.Add(this.IconVisFem);
            this.PnlVisiteurs.Controls.Add(this.lblTemps);
            this.PnlVisiteurs.Controls.Add(this.lblGenre);
            this.PnlVisiteurs.Controls.Add(this.lblVisNom);
            this.PnlVisiteurs.Controls.Add(this.IconVisHom);
            this.PnlVisiteurs.Location = new System.Drawing.Point(0, 0);
            this.PnlVisiteurs.Name = "PnlVisiteurs";
            this.PnlVisiteurs.Size = new System.Drawing.Size(162, 72);
            this.PnlVisiteurs.TabIndex = 11;
            // 
            // lblTemps
            // 
            this.lblTemps.AutoSize = true;
            this.lblTemps.BackColor = System.Drawing.Color.Transparent;
            this.lblTemps.ForeColor = System.Drawing.Color.Black;
            this.lblTemps.Location = new System.Drawing.Point(63, 45);
            this.lblTemps.Name = "lblTemps";
            this.lblTemps.Size = new System.Drawing.Size(53, 13);
            this.lblTemps.TabIndex = 3;
            this.lblTemps.Text = "2 Minutes";
            // 
            // lblGenre
            // 
            this.lblGenre.AutoSize = true;
            this.lblGenre.BackColor = System.Drawing.Color.Transparent;
            this.lblGenre.ForeColor = System.Drawing.Color.Black;
            this.lblGenre.Location = new System.Drawing.Point(63, 32);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(43, 13);
            this.lblGenre.TabIndex = 2;
            this.lblGenre.Text = "Homme";
            // 
            // lblVisNom
            // 
            this.lblVisNom.AutoSize = true;
            this.lblVisNom.BackColor = System.Drawing.Color.Transparent;
            this.lblVisNom.ForeColor = System.Drawing.Color.Black;
            this.lblVisNom.Location = new System.Drawing.Point(63, 19);
            this.lblVisNom.Name = "lblVisNom";
            this.lblVisNom.Size = new System.Drawing.Size(63, 13);
            this.lblVisNom.TabIndex = 1;
            this.lblVisNom.Text = "Scott Ryder";
            // 
            // IconVisFem
            // 
            this.IconVisFem.Image = ((System.Drawing.Image)(resources.GetObject("IconVisFem.Image")));
            this.IconVisFem.Location = new System.Drawing.Point(26, 19);
            this.IconVisFem.Name = "IconVisFem";
            this.IconVisFem.Size = new System.Drawing.Size(31, 35);
            this.IconVisFem.TabIndex = 4;
            this.IconVisFem.TabStop = false;
            // 
            // IconVisHom
            // 
            this.IconVisHom.Image = ((System.Drawing.Image)(resources.GetObject("IconVisHom.Image")));
            this.IconVisHom.Location = new System.Drawing.Point(26, 19);
            this.IconVisHom.Name = "IconVisHom";
            this.IconVisHom.Size = new System.Drawing.Size(31, 35);
            this.IconVisHom.TabIndex = 0;
            this.IconVisHom.TabStop = false;
            // 
            // InfoVisiteur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(162, 72);
            this.Controls.Add(this.PnlVisiteurs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InfoVisiteur";
            this.Text = "Form1";
            this.PnlVisiteurs.ResumeLayout(false);
            this.PnlVisiteurs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IconVisFem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconVisHom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PnlVisiteurs;
        private System.Windows.Forms.Label lblTemps;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblVisNom;
        private System.Windows.Forms.PictureBox IconVisHom;
        private System.Windows.Forms.PictureBox IconVisFem;
    }
}