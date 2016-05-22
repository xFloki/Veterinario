using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;



namespace Veterinaria
{
    public partial class NuevaVisita : UserControl
    {
        //parametros de la conexion
        private string connStr;
        //variable que maneja la conexion
        private MySqlConnection conn;
        //consulta que quiero hacer a la base de datos
        private String sentencia_SQL;
        //variable que sirve para crear la conexion
        private static MySqlCommand comando;
        //guarda el resultado de la consultam, es un arrayList
        private MySqlDataReader resultado;


        public event EventHandler StatusUpdated;

        public NuevaVisita()
        {
            InitializeComponent();
        }
        public string motivo()
        {
            return textBox2.Text;
        }

        public string observaciones()
        {
            return textBox3.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            this.SendToBack();
            this.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.StatusUpdated != null)
                this.StatusUpdated(new object(), new EventArgs());
        }
    }
}
