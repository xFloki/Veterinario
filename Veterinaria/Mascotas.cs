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
using System.IO;
using System.Net;



namespace Veterinaria
{
    public partial class Mascotas : UserControl
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

        private string busquedaCliente = "SUUUUUUU";


        private DataTable datos = new DataTable();
        public int id_Mascota = 1;


        public Mascotas()
        {
            InitializeComponent();
            cargarMascota();
           
        }

        
        private void cambiarDatosMascota()
        {
            string nombre_Mascota = nombreMascota.Text;
            string raza_Mascota = razaMascota.Text;
            string chip_Mascota = chipMascota.Text;
            string sexo_Mascota = sexoMascota.Text;
            string especie_Mascota = especieMascota.Text;
            string fechaNacimiento_Mascota = nacimientoMascota.Value.ToString("yyyy-MM-dd");

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();

            comando = new MySqlCommand("SET SQL_SAFE_UPDATES = 0", conn);
            comando.ExecuteNonQuery();
            comando = new MySqlCommand("UPDATE mascota SET nombre='" + nombre_Mascota + "',raza='" + raza_Mascota + "',chip='" + chip_Mascota +
                "',especie='" + especie_Mascota + "',sexo='" + sexo_Mascota + "',fecha_nacimiento='" + fechaNacimiento_Mascota + "'WHERE id = 1 and pasport = 'Lolo' ", conn);
            comando.ExecuteNonQuery();
            comando = new MySqlCommand("SET SQL_SAFE_UPDATES = 1", conn);
            comando.ExecuteNonQuery();
            conn.Close();
            deshabilitarDatosMascota();
        }

        //Publico para cuando estemos en el userControl de usuarios y clickeemos en su mascota la podemos cargar desde ahi 
        public void cargarMascota()
        {

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select * from mascota where id = " + id_Mascota, conn);
            resultado = comando.ExecuteReader();
            //datos.Load(resultado);
            if (resultado.Read())
            {
                //El deshabilitar que se puedan modificar el id y el pasaporte de la mascota lo colocamos aqui en vez de en el metodo
                //deshabilitarDatosMascota() puesto que solo lo vamos a poner una vez, estos datos nunca los vamos a modificar
                string fotoMascotaUrl;
                idMascota.Text = resultado.GetString("id");
                idMascota.ReadOnly = true;
                nombreMascota.Text = resultado.GetString("nombre");
                sexoMascota.Text = resultado.GetString("sexo");
                especieMascota.Text = resultado.GetString("especie");
                chipMascota.Text = resultado.GetString("chip");
                pasaporteMascota.Text = resultado.GetString("pasport");
                pasaporteMascota.ReadOnly = true;
                nacimientoMascota.Text = resultado.GetString("fecha_nacimiento");
                razaMascota.Text = resultado.GetString("raza");
                fotoMascotaUrl = resultado.GetString("foto");

                //Para que el programa no se quede esperando al principio mientras descarga la primera imagen la de la mascota 
                //la descargamos 
                WebClient wc = new WebClient();
                wc.Proxy = null;
                byte[] bFile = wc.DownloadData((String)resultado.GetString("foto"));
                MemoryStream ms = new MemoryStream(bFile);
                Image img = Image.FromStream(ms);
                fotoMascota.Image = img;
                //fotoMascota.Load("http://fress.co/wp-content/uploads/2014/05/Animales-cansados-15.jpg");
                //Scalamos la imagen para que quede bien en nuestro pictureBox
                fotoMascota.SizeMode = PictureBoxSizeMode.Zoom;
                deshabilitarDatosMascota();
            }
            resultado.Close();
            conn.Close();
            //dataGridView1.DataSource = datos;

        }

        private void habilitarDatos()
        {
            nombreMascota.ReadOnly = false;
            sexoMascota.ReadOnly = false;
            especieMascota.ReadOnly = false;
            chipMascota.ReadOnly = false;
            nacimientoMascota.Enabled = true;
            razaMascota.ReadOnly = false;
        }


        private void deshabilitarDatosMascota()
        {
            nombreMascota.ReadOnly = true;
            sexoMascota.ReadOnly = true;
            especieMascota.ReadOnly = true;
            chipMascota.ReadOnly = true;
            nacimientoMascota.Enabled = false;
            razaMascota.ReadOnly = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("¿Esta seguro que desea sobreescribir los datos?", "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                cambiarDatosMascota();
                saveButton.Hide();
            }
            else if (dr == DialogResult.No)
            {
                cargarMascota();
                deshabilitarDatosMascota();
                saveButton.Hide();
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {

            saveButton.Show();
            habilitarDatos();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            --id_Mascota;
            cargarMascota();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ++id_Mascota;
            cargarMascota();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NuevaMascota prueba = new NuevaMascota();
            prueba.Show();
            prueba.Enabled = true;
            prueba.BringToFront();
            prueba.Visible = true;
            
        }

    }
}