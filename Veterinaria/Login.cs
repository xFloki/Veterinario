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
    public partial class Login : UserControl
    {
        int tipo = 1;

        public Login()
        {
            InitializeComponent();
            Fillcombo();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        //parametros de la conexion
        private string connStr;
        //variable que maneja la conexion
        private MySqlConnection conn;
        //consulta que quiero hacer a la base de datos
        private String sentencia_SQL;
        //guarda el resultado de la consultam, es un arrayList
        private MySqlDataReader resultado;
        //variable que sirve para crear la conexion
        private static MySqlCommand comando;

        private DataTable datos = new DataTable();

        

        //int n = 0;

       

        private void Fillcombo()
        {

            connStr = "Server=localhost; Database=veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);

            try
            {
                conn.Open();
                sentencia_SQL = "Select * from usuario";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();


                //cierra la conexion

                while (resultado.Read())
                {
                    string sName = resultado.GetString("login");
                    comboBox1.Items.Add(sName);
                }
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Proporcione un Usuario y Contraseña Por Favor");
                return;
            }
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            //Reseteamos el valor de Datatable para poder aceptar más de una vez(en la practica no tiene mucho sentido amenos que exista la posibilidad de deslogearse)
            datos.Clear();
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from usuario where login='" + comboBox1.Text + "' and pass='" + textBox1.Text + "';", conn);
            sda.Fill(datos);
            conn.Close();
            if (datos.Rows.Count == 1)
            {
                //MessageBox.Show("Bien");
                //MainForm contenido = new MainForm();

                //lo llamo con this porque con Form1 no lo cosnigo 
                this.Hide();
                //for (int index = Application.OpenForms.Count - 1; index >= 0; index--)
                //{
                //    if (Application.OpenForms[1].Name == "Form2")
                //    {
                //        Application.OpenForms[1].Close;
                //    }
                //}
                if (comboBox1.Text == "anna66")
                {
                    tipo = 1;
                }
                
                Application.OpenForms[0].Hide();
                Fondo fondo = new Fondo(tipo);
                fondo.StartPosition = FormStartPosition.Manual;
                fondo.Location = new Point(0, 0);
                fondo.Show();



            }
            else
            {
                //MessageBox.Show("Mal");
            }

        }
    }
}
