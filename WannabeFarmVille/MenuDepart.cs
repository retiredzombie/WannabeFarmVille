using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannabeFarmVille
{
    public partial class MenuDepart : Form
    {
        public MenuDepart()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Jeu jeu = new Jeu(this);
            Hide();
            jeu.ShowDialog();
        }

        private void MenuDepart_Load(object sender, EventArgs e)
        {

        }
    }
}
