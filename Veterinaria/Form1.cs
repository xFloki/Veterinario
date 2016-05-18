using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
////añadimos los import de MySql
using System.Data.Odbc;
using MySql.Data.Types;
using MySql.Data.MySqlClient;


namespace Veterinaria
{
    public partial class Form1 : Form
    {

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

        //publico para que desde el form principal podamos acceder a esta variable y saber el tipo de usuario que acab de logear
        public int tipo;

        //int n = 0;

       
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.CenterToScreen();
            Fillcombo();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            
        }

        private void button1_Click(object sender, EventArgs e)
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
                if (comboBox1.Text == "anna66") {
                    tipo = 1;
                } else { tipo = 2; }
                Application.OpenForms[1].Hide();
                Fondo fondo = new Fondo();
                fondo.StartPosition = FormStartPosition.Manual;
                fondo.Location = new Point(0, 0);
                fondo.Show();
                
                

            }
            else {
                //MessageBox.Show("Mal");
            }
    
            
        }

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

       

       
    }
}


////añadimos los import de MySql
//using System.Data.Odbc;
//using MySql.Data.Types;
//using MySql.Data.MySqlClient;


//namespace EjemploMysql
//{
//    public partial class Form1 : Form
//    {
//        //este ejemplo conectará con una base de datos Mysql

//        //necesito 5 parámetros:
//        //Server : la ip o nombre del servidor
//        //Database: nombre de la base de datos
//        //Uid: usuario (ojo, no se puede dejar en blanco)
//        //Pwd: clave de acceso si la tuviera
//        //Port: default = 3306

//        //parametros de la conexion
//        private string connStr;
//        //variable que maneja la conexion
//        private MySqlConnection conn;
//        //consulta que quiero hacer a la base de datos
//        private String sentencia_SQL;
//        //variable que sirve para crear la conexion
//        private static MySqlCommand comando;
//        //guarda el resultado de la consultam, es un arrayList
//        private MySqlDataReader resultado;

//        private DataTable datos = new DataTable();

//        int n = 0;

//        public Form1()
//        {
//            InitializeComponent();

//            connStr = "Server=localhost; Database=test; Uid=root; Pwd=root ; Port=3306";
//            conn = new MySqlConnection(connStr);
//            //abre la conexion
//            conn.Open();
//            sentencia_SQL = "Select * from Pokemon";
//            comando = new MySqlCommand(sentencia_SQL, conn);
//            resultado = comando.ExecuteReader();
//            datos.Load(resultado);

//            //cierra la conexion
//            conn.Close();

//        }

//        private String consulta(String cl, int numFila)
//        {
//            //si consigue leer un dato de la base de datos lo devuelve
//            DataRow fila = datos.Rows[numFila];
//            if (fila != null)
//            {
//                return fila[cl].ToString();
//            }
//            else return "No existe";

//        }
//        private void button1_Click(object sender, EventArgs e)
//        {

//            //label1.text = consulta("name", n);
//            //label2.text = consulta("color", n);
//            //n = n + 1;
//            dataGridView1.DataSource = datos;

//        }



//    }