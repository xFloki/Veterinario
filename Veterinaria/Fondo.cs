using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Veterinaria
{
    public partial class Fondo : Form
    {
        public Fondo()
        {
            InitializeComponent();
            pictureBox2.SendToBack();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm contenido = new MainForm();
            contenido.Show();
            Button btn = (Button)sender;
            btn.ForeColor = Color.White;
        }
    }
}
