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
    public partial class AdmineUsers : UserControl
    {
        public event EventHandler StatusUpdated;

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

        public string busquedaUsuario = "SUUUUUUU";
        private DataTable datos = new DataTable();


        public AdmineUsers()
        {

            try
            {
                connStr = ConexionBDD.rutaConexion;
                conn = new MySqlConnection(connStr);
                conn.Open();
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }

            InitializeComponent();
            autoCompletar();
            cargarUsuarios();
        }

        private void autoCompletar()
        {
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();


            //abre la conexion
            try
            {
                conn.Open();
                // no mostramos las contraseñas, una vez dada la contraseña por defecto por el administrador los usuarios pueden cambiarla 
                // y será anonima y encriptada en la bdd
                sentencia_SQL = "select dni, login,  nombre, apellido, email, telefono, direccion, fecha_nacimiento from usuario";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();
                while (resultado.Read())
                {
                    string sName = resultado.GetString("dni");
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

        private void eliminarUsuario()
        {


            //abre la conexion
            conn.Open();

            comando = new MySqlCommand("delete  from usuario where dni = '" + busquedaUsuario + "';", conn);
            comando.ExecuteNonQuery();
            MessageBox.Show("Usuario Eliminado Correctamente");
            conn.Close();
        }

        private void cargarUsuarios()
        {

            //abre la conexion
            conn.Open();
            //Ponemos en la consulta una condicion para que no se muestra el ADMINISTRADOR ya que no queremos que sea posible eliminarlo 
            //y para evitarnos que el propio administrador se elimine sin querere no le sacamos
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from usuario where dni != 'ADMIN'", conn);
            conn.Close();
            datos.Clear();
            sda.Fill(datos);
            dataGridView1.DataSource = datos;

            dataGridView1.Columns["Borrar"].DisplayIndex = 10;
            
            //deshabilitarDatosCliente();

        }

        //Cargar los datos del usuario que hemos seleccionado en el datagrid en los diferentes tebox de la pantalla 
        private void cargarUsuario()
        {

            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select * from usuario where dni = '" + busquedaUsuario + "'", conn);
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
                textBox2.Text = resultado.GetString("login");
            }
            resultado.Close();
            conn.Close();
             

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.StatusUpdated != null)
                this.StatusUpdated(new object(), new EventArgs());
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //he tenido que usar el numero de celda 1 ya que al añadir la fila de la imagen se me ha puesto la 0 
            var item = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
            busquedaUsuario = item.ToString();
            cargarUsuario();
            //commpruebo que se haya clickeado el boton de borrar usuario
            if (e.ColumnIndex == 0)
            {
                DialogResult dr = MessageBox.Show("¿Esta seguro que desea elimar este usuario? Los datos no se podran recuperar",
                    "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                {
                    eliminarUsuario();
                    cargarUsuarios();
                }
                else if (dr == DialogResult.No)
                {

                    
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cargarUsuarios();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1 != null && !string.IsNullOrWhiteSpace(textBox1.Text))
            {

                //abre la conexion
                conn.Open();
                //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
                MySqlDataAdapter sda = new MySqlDataAdapter("Select * from usuario where dni REGEXP '" + textBox1.Text + "'", conn);
                conn.Close();
                datos.Clear();
                sda.Fill(datos);
                dataGridView1.DataSource = datos;

            }
            else
            {
                cargarUsuarios();
            }
        }
    }
}
