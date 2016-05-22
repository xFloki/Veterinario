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

        private int mascotasTotales;

        public event EventHandler StatusUpdated;


        private DataTable datos = new DataTable();
        public int id_Mascota = 1;
        private string idObservacion;


        public Mascotas()
        {
            InitializeComponent();
            nuevaVisita1.StatusUpdated += new EventHandler(addNuevaVisita);
            cargarMascota();
            autoCompletar();
            mascotasMaximas();
           




        }

        public void addNuevaVisita(object sender, EventArgs e)
        {

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();

            comando = new MySqlCommand("INSERT INTO  visita (mascota, motivo, fecha, observaciones) VALUES ('"
                + idMascota.Text + "','" + nuevaVisita1.motivo() + "', current_date() ,'" + nuevaVisita1.observaciones() + "' );", conn);
            comando.ExecuteNonQuery();
            conn.Close();
            nuevaVisita1.Enabled = false;
            nuevaVisita1.SendToBack();
            nuevaVisita1.Visible = false;

            cargarVisitas();
        }

        //metodo public para pasarle al form el propietario actual
        public string clienteActual() {

            return propietarioMascota.Text;
        }

        

        private void ocultarVisitas()
        {
            nuevaVisita1.Enabled = false;
            nuevaVisita1.Visible = false;
            nuevaVisita1.SendToBack();

        }

        private void autoCompletar()
        {
            textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();


            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            try
            {
                conn.Open();
                sentencia_SQL = "select * from mascota";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();
                while (resultado.Read())
                {
                    string sName = resultado.GetString("pasport");
                    coll.Add(sName);

                }
                conn.Close();

            }
            catch (Exception)
            {

                throw;
            }

            textBox2.AutoCompleteCustomSource = coll;

        }

        private void cargarVisitas()
        {

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            MySqlDataAdapter sda = new MySqlDataAdapter("Select fecha, motivo, id from visita where mascota = '" + idMascota.Text + "' order by fecha desc", conn);
            conn.Close();
            datos.Clear();
            sda.Fill(datos);
            dataGridView2.DataSource = datos;
            //Ocultamos el id porque es algo que no necesitamos que vea el usuario simplemente lo vamos a usar nosotro en el codigo para
            //realizar la busqueda de esa visita a la hora de mostrar las observaciones
            dataGridView2.Columns[2].Visible = false;

            //Si la mascota tiene almenos una visita nos cargara las observaciones de la ultima, de lo contrario nos podran las observaciones vacias
            try
            {
                var item = dataGridView2.Rows[0].Cells[2].Value;
                idObservacion = item.ToString();
                datosVisitas();
            }
            catch (Exception)
	{
                textBox1.Text = "";
               
            }
            


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

        private void mascotasMaximas() {

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select count(*) from mascota", conn);
            resultado = comando.ExecuteReader();
            //datos.Load(resultado);
            if (resultado.Read()) {
                mascotasTotales = resultado.GetInt32("count(*)");
            }
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
                propietarioMascota.Text = resultado.GetString("propietario");

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
            cargarVisitas();

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

        private void  datosVisitas()
        {
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select observaciones from visita where id = '" + idObservacion + "'", conn);
            resultado = comando.ExecuteReader();
            resultado.Read();
            textBox1.Text = resultado.GetString("observaciones") ;

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

        //Los dos siguientes metodos son para recorre a las mascotas, en el caso de que llegue a la ultima de todas empieza por la primer
        //Mecanismo a mejorar puesto que si se eliminan mascotas quedan huecos vacios y va a seguir pasando por ello aun no habiendo nada
        private void button6_Click(object sender, EventArgs e)
        {
            
            --id_Mascota;
            if (id_Mascota > 1) { id_Mascota = mascotasTotales; }
            cargarMascota();
            ocultarVisitas();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ++id_Mascota;
            if(id_Mascota> mascotasTotales) { id_Mascota = 1; }
            cargarMascota();
            ocultarVisitas();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NuevaMascota prueba = new NuevaMascota();
            prueba.Show();
            prueba.Enabled = true;
            prueba.BringToFront();
            prueba.Visible = true;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.StatusUpdated != null)
                this.StatusUpdated(new object(), new EventArgs());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            
            if (textBox2 != null && !string.IsNullOrWhiteSpace(textBox2.Text))
            {

                connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
                conn = new MySqlConnection(connStr);
                //abre la conexion
                conn.Open();
                comando = new MySqlCommand("Select * from mascota where pasport = '" + textBox2.Text + "'", conn);
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
                    propietarioMascota.Text = resultado.GetString("propietario");

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
                ocultarVisitas();
                //dataGridView1.DataSource = datos;

            }
            else { 
                id_Mascota = 1;
              cargarMascota();
                ocultarVisitas();
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if para que el evento solo se ejectue cuando se hace click sobre una de las celdas que no sean cabeceras
            if (e.RowIndex != -1)
            {
                var item = dataGridView2.Rows[e.RowIndex].Cells[2].Value;
                idObservacion = item.ToString();
                datosVisitas();
            }
                       
            //commpruebo que se haya clickeado el boton de borrar usuario
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            nuevaVisita1.Enabled = true;
            nuevaVisita1.Visible = true;
            nuevaVisita1.BringToFront();
        }
    }
}