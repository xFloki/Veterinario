using MySql.Data.MySqlClient;
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
    public partial class MainForm : Form
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


        private DataTable datos = new DataTable();

        public MainForm()
        {
            InitializeComponent();
            cargaClientes();        }

        private void cargaClientes() {
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente", conn);
            conn.Close();
            sda.Fill(datos);
            dataGridView1.DataSource = datos;
            //comando = new MySqlCommand("Select * from cliente", conn);
            //resultado = comando.ExecuteReader();
            //datos.Load(resultado);
            //conn.Close();
            //dataGridView1.DataSource = datos;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NewClient nuevoCliente = new NewClient(dataGridView1);
            nuevoCliente.Show();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente", conn);
            conn.Close();
            datos.Clear();
            sda.Fill(datos);
            dataGridView1.DataSource = datos;
        }
    }
}
