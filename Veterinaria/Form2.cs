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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            Form1 formLogin = new Form1();
            formLogin.Show();
            formLogin.TopMost = true;
            
        }
    }
}
