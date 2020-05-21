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
    public partial class InfoAnimal : Form
    {
        public InfoAnimal()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.CenterToScreen();
        }

        public void SetInformation(String race, String genre, String age, String nourris, String grosse)
        {
            IconBuffle.Visible = false;
            IconGrizzly.Visible = false;
            IconLicorne.Visible = false;
            IconMouton.Visible = false;
            IconRhino.Visible = false;
            IconLion.Visible = false;
            lblRace.Text = race;
            lblSexe.Text = genre;
            lblAge.Text = age;
            lblNourissement.Text = nourris;
            lblGrossesse.Text = grosse;
            switch (race)
            {
                case "Lion": IconLion.Visible = true;
                    break;
                case "Buffle": IconBuffle.Visible = true;
                    break;
            }
        }
    }
}
