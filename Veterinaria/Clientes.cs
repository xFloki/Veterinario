﻿using System;
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
    public partial class Clientes : UserControl
        
    {
        //parametros de la conexion
        private static string connStr = ConexionBDD.rutaConexion;
        //variable que maneja la conexion
        private MySqlConnection conn = new MySqlConnection(connStr);
        //consulta que quiero hacer a la base de datos
        private String sentencia_SQL;
        //variable que sirve para crear la conexion
        private static MySqlCommand comando;
        //guarda el resultado de la consultam, es un arrayList
        private MySqlDataReader resultado;
        public string busquedaCliente = "SUUUUUUU";
        private DataTable datos = new DataTable();
        
      

        public event EventHandler StatusUpdated;
       


        public Clientes()
        {

          
            try
            {
                conn.Open();
                conn.Close();
            }
            catch (Exception)
            {
                throw;
            }
            InitializeComponent();
            autoCompletar();
            cargaClientes();
            




            }

        

        //metodo para poder seleccionar la mascota del usuario que estemos viendo y con esto pasarle el ide al usercontrol de las mascotas para que lo cargue
        public int mascotaClienteAhorita() {
      
            int fid = 0;
            try
            {
                fid = int.Parse(mascotaCliente.Text.ToString());
            }
            catch (Exception e)
            {
                //Whatever you want to do when it is not an int
            }
            return fid;
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

        private void cambiarDatosCliente()
        {

            //abre la conexion
            
            string nombre_Cliente = nombreCliente.Text;
            string apellido_Cliente = apellidoCliente.Text;
            string email_Cliente = emailCliente.Text;
            string telefono_Cliente = telefonoCliente.Text;
            string direccion_Cliente = direccionCliente.Text; 
            string fechaNacimiento_Cliente = dateTimePicker1.Value.ToString("yyyy-MM-dd");

            if (!string.IsNullOrEmpty(nombre_Cliente) && !string.IsNullOrEmpty(apellido_Cliente) && !string.IsNullOrEmpty(email_Cliente) && 
                !string.IsNullOrEmpty(telefono_Cliente) && !string.IsNullOrEmpty(direccion_Cliente))
            {
                //abre la conexion
                conn.Open();

                comando = new MySqlCommand("SET SQL_SAFE_UPDATES = 0", conn);
                comando.ExecuteNonQuery();
                comando = new MySqlCommand("UPDATE cliente SET nombre='" + nombre_Cliente + "',apellido='" + apellido_Cliente + "',email='" + email_Cliente +
                    "',telefono='" + telefono_Cliente + "',direccion='" + direccion_Cliente + "',fecha_nacimiento='" + fechaNacimiento_Cliente + "'where dni = '" + clienteDni.Text+"'" , conn);
                comando.ExecuteNonQuery();
                comando = new MySqlCommand("SET SQL_SAFE_UPDATES = 1", conn);
                comando.ExecuteNonQuery();
                conn.Close();
            }
            else {
                cargarCliente();
                MessageBox.Show("Debe introducir todos los datos del cliente");

            }
            
            deshabilitarDatosCliente();
            

        }

        private void cargaClientes()
        {

            //abre la conexion
            conn.Open();
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente", conn);
            conn.Close();
            datos.Clear();
            sda.Fill(datos);
            dataGridView1.DataSource = datos;

            dataGridView1.Columns["Borrar"].DisplayIndex = 7;
            //dataGridView1.Columns["ContactTitle"].DisplayIndex = 1;
            //dataGridView1.Columns["City"].DisplayIndex = 2;
            //dataGridView1.Columns["Country"].DisplayIndex = 3;
            //dataGridView1.Columns["CompanyName"].DisplayIndex = 4;
            deshabilitarDatosCliente();
            conn.Close();

        }

        //Carga los datos del cliente que has seleccionado en el DataGridView en los texbox del tabcontrol3, el que se encuentra junto a este
        public void cargarCliente()
        {
            //abre la conexion
            conn.Open();
            comando = new MySqlCommand("Select * from cliente where dni = '" + busquedaCliente + "'", conn);
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
                mascotaCliente.Text = "";
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

        private void eliminarCliente() {
          
            //abre la conexion
            conn.Open();

           
            sentencia_SQL = "Select * from mascota where propietario = '" + busquedaCliente + "'";
            comando = new MySqlCommand(sentencia_SQL, conn);
            resultado = comando.ExecuteReader();


          
            //Condicion para que no se puedan eliminat clientes que tienen asociadas mascotas
            if (resultado.Read())
            {
                MessageBox.Show("Elimine antes las mascotas de este cliente");
                resultado.Close();
            }
            else {
                resultado.Close();
                comando = new MySqlCommand("delete  from veterinario.cliente where dni = '" + busquedaCliente + "';", conn);
                comando.ExecuteNonQuery();
                MessageBox.Show("Cliente Eliminado Correctamente");
            }
            conn.Close();
            
          
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1 != null && !string.IsNullOrWhiteSpace(textBox1.Text))
            {

                //abre la conexion
                conn.Open();
                //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
                MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente where nombre REGEXP '" + textBox1.Text + "'", conn);
                conn.Close();
                datos.Clear();
                sda.Fill(datos);
                dataGridView1.DataSource = datos;
                conn.Close();

            }
            else
            {
                cargaClientes();
            }
        }

     
        private void button2_Click(object sender, EventArgs e)
        {
          
            cargaClientes();
  
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //he tenido que usar el numero de celda 1 ya que al añadir la fila de la imagen se me ha puesto la 0 
            var item = dataGridView1.Rows[e.RowIndex].Cells[1].Value;
            busquedaCliente = item.ToString();
            cargarCliente();
            //commpruebo que se haya clickeado el boton de borrar usuario
            if (e.ColumnIndex == 0) {
                DialogResult dr = MessageBox.Show("¿Esta seguro que desea elimar este usuario? Los datos no se podran recuperar",
                    "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (dr == DialogResult.Yes)
                {
                    eliminarCliente();
                    cargaClientes();
                }
                else if (dr == DialogResult.No)
                {
                    
                    saveButtonCliente.Hide();
                }
            }

        }


        private void button10_Click(object sender, EventArgs e)
        {
            saveButtonCliente.Show();
            habilitarDatosCliente();
            
        }

        private void saveButtonCliente_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("¿Esta seguro que desea sobreescribir los datos?", "OPERACION CRITICA", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                cambiarDatosCliente();
                saveButtonCliente.Hide();
                deshabilitarDatosCliente();
                cargaClientes();
            }
            else if (dr == DialogResult.No)
            {
                cargarCliente();
                deshabilitarDatosCliente();
                saveButtonCliente.Hide();
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            if (this.StatusUpdated != null)
                this.StatusUpdated(new object(), new EventArgs());
            
            //resetearBotones(sender);
            //Fondo.mascotas1.Enabled = true;
            //mascotas1.BringToFront();
            //mascotas1.Visible = true;
        }
    }

   
}
