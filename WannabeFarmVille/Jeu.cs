﻿using System;
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
    public partial class Jeu : Form
    {
        TilesetImageGenerator TilesetIG;

        public Jeu()
        {
            InitializeComponent();

            TilesetIG = new TilesetImageGenerator();
        }

        private void Jeu_Load(object sender, EventArgs e)
        {

        }
    }
}
