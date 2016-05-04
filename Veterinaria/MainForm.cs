using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

        private string busquedaCliente = "SUUUUUUU";


        private DataTable datos = new DataTable();
        private int id_Mascota = 2;
        

        public MainForm()
        {
            InitializeComponent();
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            cargaClientes();
            cargarMascota();
            autoCompletar();
        }
        

        private void autoCompletar()
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();


            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            try
            {
                conn.Open();
                sentencia_SQL = "select * from cliente";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();
                while (resultado.Read())
                {
                    string sName = resultado.GetString("nombre");
                    coll.Add(sName);

                }
                conn.Close();

            }
            catch (Exception)
            {

                throw;
            }

            textBox1.AutoCompleteCustomSource = coll;

        }

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

            dataGridView1.Columns["Borrar"].DisplayIndex = 7;
            //dataGridView1.Columns["ContactTitle"].DisplayIndex = 1;
            //dataGridView1.Columns["City"].DisplayIndex = 2;
            //dataGridView1.Columns["Country"].DisplayIndex = 3;
            //dataGridView1.Columns["CompanyName"].DisplayIndex = 4;
            deshabilitarDatosCliente();
            
        }

        private void cambiarDatosMascota() {
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
            comando = new MySqlCommand("UPDATE mascota SET nombre='"+ nombre_Mascota + "',raza='" + raza_Mascota + "',chip='" + chip_Mascota +
                "',especie='" + especie_Mascota + "',sexo='" + sexo_Mascota + "',fecha_nacimiento='" + fechaNacimiento_Mascota + "'WHERE id = 1 and pasport = 'Lolo' ", conn);
            comando.ExecuteNonQuery();
            comando = new MySqlCommand("SET SQL_SAFE_UPDATES = 1", conn);
            comando.ExecuteNonQuery();
            conn.Close();
            deshabilitarDatosMascota();
        }

        private void cargarMascota(){
            
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select * from mascota where id = "+id_Mascota, conn);
            resultado = comando.ExecuteReader();
            //datos.Load(resultado);
            if(resultado.Read())
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
        
        //Carga los datos del cliente que has seleccionado en el DataGridView en los texbox del tabcontrol3, el que se encuentra junto a este
        private void cargarCliente()
        {
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select * from cliente where dni = '" + busquedaCliente+"'", conn);
            resultado = comando.ExecuteReader();
            //datos.Load(resultado);
            if (resultado.Read())
            {
               
                //El deshabilitar que se puedan modificar el id y el pasaporte de la mascota lo colocamos aqui en vez de en el metodo
                //deshabilitarDatosMascota() puesto que solo lo vamos a poner una vez, estos datos nunca los vamos a modificar
                nombreCliente.Text = resultado.GetString("nombre");
                dateTimePicker1.Text = resultado.GetString("fecha_nacimiento");
                apellidoCliente.Text = resultado.GetString("apellido");
                clienteDni.Text = resultado.GetString("dni");
                emailCliente.Text = resultado.GetString("email");
                telefonoCliente.Text = resultado.GetString("telefono");
                direccionCliente.Text = resultado.GetString("direccion");
               }
            resultado.Close();
            try
            {

                sentencia_SQL = "select mascota.nombre,mascota.id   from cliente, mascota where cliente.dni = mascota.propietario and cliente.dni = '" +
                busquedaCliente + "'";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();
                mascotaCliente.Items.Clear();
                while (resultado.Read())
                {
                    string sName = resultado.GetString("id");
                    mascotaCliente.Items.Add(sName);
                    mascotaCliente.Text = sName;
                    
                }
    
               
            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
            deshabilitarDatosCliente();
            //dataGridView1.DataSource = datos;
        }

      
        //Se habilitan los texBox de las mascotas para que puedan ser modificados y posteriormente guardados los datos
        private void habilitarDatos()
        {
            nombreMascota.ReadOnly = false;            
            sexoMascota.ReadOnly = false;
            especieMascota.ReadOnly = false;
            chipMascota.ReadOnly = false;
            nacimientoMascota.Enabled = true;
            razaMascota.ReadOnly = false;
        }

        private void habilitarDatosCliente()
        {
            nombreCliente.ReadOnly = false;
            apellidoCliente.ReadOnly = false;
            emailCliente.ReadOnly = false;
            dateTimePicker1.Enabled = true;
            direccionCliente.ReadOnly = false;
            telefonoCliente.ReadOnly = false;
        }

        private void deshabilitarDatosMascota() {
            nombreMascota.ReadOnly = true;
            sexoMascota.ReadOnly = true;
            especieMascota.ReadOnly = true;
            chipMascota.ReadOnly = true;
            nacimientoMascota.Enabled = false;
            razaMascota.ReadOnly = true;
        }

        private void deshabilitarDatosCliente()
        {

            nombreCliente.ReadOnly = true;
            apellidoCliente.ReadOnly = true;
            clienteDni.ReadOnly = true;
            emailCliente.ReadOnly = true;
            dateTimePicker1.Enabled = false;
            direccionCliente.ReadOnly = true;
            telefonoCliente.ReadOnly = true;
            
        }

        //Creamos las ventana para añadir un nuevo cliente
        private void button1_Click(object sender, EventArgs e)
        {
            NewClient nuevoCliente = new NewClient();
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

        private void button4_Click(object sender, EventArgs e)
        {
            NewPet nuevaMascota = new NewPet();
            nuevaMascota.Show();
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

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show( "¿Esta seguro que desea sobreescribir los datos?", "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

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

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        //Cuando haces click sobre el dataGridView se guarda el valor del dni del cliente sobre el que has seleccionado para luego usarlo en la consulta 
        //en la que te cargara los datos del cliente, también te carga el tab donde se encuentra
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //he tenido que usar el numero de celda 1 ya que al añadir la fila de la imagen se me ha puesto la 0 
            var item = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
            busquedaCliente = item.ToString();
           
            tabControl1.SelectedTab = tabPage4;
            cargarCliente();

        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Esta seguro que desea sobreescribir los datos?", "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                //cambiarDatosCliente();
                saveButtonCliente.Hide();
            }
            else if (dr == DialogResult.No)
            {
                cargarCliente();
                deshabilitarDatosCliente();
                saveButtonCliente.Hide();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            saveButtonCliente.Show();
            habilitarDatosCliente();
        }

        private void button8_Click_2(object sender, EventArgs e)
        {
            Clientes.SelectedTab = tabPage2;
            //id_Mascota = mascotaCliente.Text;
            cargarMascota();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1 != null && !string.IsNullOrWhiteSpace(textBox1.Text))
            {

                connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
                conn = new MySqlConnection(connStr);
                //abre la conexion
                conn.Open();
                //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
                MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente where nombre REGEXP '" + textBox1.Text + "'", conn);
                conn.Close();
                datos.Clear();
                sda.Fill(datos);
                dataGridView1.DataSource = datos;

            }
            else
            {
                cargaClientes();
            }
            
        }
    }
}
