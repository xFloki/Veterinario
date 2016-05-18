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
using System.Security.Cryptography;

namespace Veterinaria
{
    public partial class NuevoCliente : UserControl
    {
        //parametros de la conexion
        private string connStr;
        //variable que maneja la conexion
        private MySqlConnection conn;
        //variable que sirve para crear la conexion
        private static MySqlCommand comando;
        private DataTable datos = new DataTable();

        public NuevoCliente()
        {
            InitializeComponent();
        }

        private void addClient()
        {
            string nombre = textBox1.Text;
            string apellido = textBox2.Text;
            string dni = textBox3.Text;
            string email = textBox4.Text;
            string telefono = textBox5.Text;
            string direccion = textBox6.Text;
            string fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();

            //var salt = System.Text.Encoding.UTF8.GetBytes("salero");
            //var password = System.Text.Encoding.UTF8.GetBytes(telefono);
            //var hmacMD5 = new HMACMD5(salt);
            //var saltedHash = hmacMD5.ComputeHash(password);

            comando = new MySqlCommand("INSERT INTO `cliente` VALUES ('" + dni + "','" + nombre + "','" + apellido + "','" + email + "','" + telefono + "','" + direccion + "','" + fecha + "')", conn);
            comando.ExecuteNonQuery();
            conn.Close();
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente", conn);
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            //MySqlDataAdapter sda = new MySqlDataAdapter("INSERT INTO `cliente` VALUES ('"+dni+"','"+nombre+ "','" + apellido + "','" + email + "','" + telefono + "','" + direccion + "','" + fecha + "')", conn);
           
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            addClient();
        }
    }
}
