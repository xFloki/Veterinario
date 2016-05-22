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
                //Consulta para que no nos muestre al administrador a la hora de seleccionar uno de los usuarios
                sentencia_SQL = "Select * from usuario where dni !=  'ADMIN';";
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

        private void logear() {
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
            string pass = textBox1.Text;

            //ENCRIPTAMOS LA CONTRASEÑA QUE SE INTRODUCE PARA VER SI COINCIDE CON LA ENCRIPTADA QUE TENEMOS EN LA BASE DE DATOS
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(pass));
            //get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }
            pass = strBuilder.ToString();

            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from usuario where login='" + comboBox1.Text + "' and pass='" + pass + "';", conn);

            sda.Fill(datos);




            if (datos.Rows.Count == 1)
            {

                //Guardamos el tipo del usuario si es correcta la contraseña para despues pasarselo al form principal y de este modo saber si es un empleado 
                //o el administrador
                comando = new MySqlCommand("Select tipo from usuario where login = '" + comboBox1.Text + "'", conn);
                resultado.Close();
                resultado = comando.ExecuteReader();

                resultado.Read();
                tipo = resultado.GetInt32(0);


                //MessageBox.Show("Bien");
                //MainForm contenido = new MainForm();

                this.Hide();



                Application.OpenForms[0].Hide();
                //Pasamos el tipo por el constructor
                Fondo fondo = new Fondo(tipo);
                fondo.StartPosition = FormStartPosition.Manual;
                fondo.Location = new Point(0, 0);
                fondo.Show();



            }
            else
            {
                MessageBox.Show("Los datos introducidos son incorrectos");
            }
            conn.Close();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            logear();
        }

        //Para que puedas logear una vez hayas puesto la contraseña y tengas el foco en el textbox de esta, si pulsas enter entras al programa
        //del mismo modo que si pulsas el boton, un simple atajo
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                logear();
            }
        }
    }
}
