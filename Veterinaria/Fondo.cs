
// AUTOR : ALEJANDRO DIETTA MARTIN 1ºDAM 
// Este programa es una aplicacion para la gestion de una veterinaria, el destinatario son las personas que se estan encargando de las mascotas, veterinarios, o administradores
// del sistema. Hay un Super Usuario cuyo login es 'ADMIN' pass '1234' y con el cual podemos añadir o eliminar nuevos usuarios, en el caso de que despiados a nuevos profesionales
// y ya no necesitemos su cuenta o si queremos contratar mas y nevesitamos nuevos accesos. Los usuarios normales pueden crear nuevo clientes, modificarlos (algunos datos como el id
// que poseen dentro de la base de datos no se pueden modificar), añadir nuevas mascotas (su propietario debera estar guardaado previamenete en la base de datos, mediante este 
// programa se puede hacer, de lo contrario no podremos ingresar la mascota), modificar datos de las mascotas existentes, comprobar el registro de sus visitas al veterinario, 
// añadir nuevas visitas... al la hora de introdudir la foto de la mascota podremos hacerlo o bien poniendo una url de una foto de internet png de la mascota o seleccionando un archivo
// de imagen dentro de nuestro ordenador. A la hora de buscar clientes podemos usar el autocompletar y desde los clientes en su perfil podremos acceder a la mascota que deseemos
// Las contraseñas de los usuarios guardan cifradas en la base de datos por lo que lo ideal ese introducir desde el programa a todos los usuarios o bien pasar la contraseña ya cifrada
// en el script de la BDD puesto que si introducimos una no cifrada a la hora de hacer la comprobacion del login nos dara incorrecto ya que comprueba cifrando la que le pasamos 


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

    public partial class Fondo : Form
    {
        private  int tipoRecibido;
        
        public Fondo(int tipo)
        {
            
            InitializeComponent();
            tipoRecibido = tipo;
            comprobarPrivilegios();
            //this.ownerForm = ownerForm;
            //Creamos un eventHandler al que le pasamos una evento de click para poder ejecutar desde este form el evento de click
            // de un boton que se encuentra en el el UserControl "Clientes1"
            Clientes1.StatusUpdated += new EventHandler(cargarMascotaSeleccionada);
            //Otros event handler para poder administrar eventos de boton que se encuentra en los user control
            mascotas1.StatusUpdated += new EventHandler(cargarClienteSeleccionado);
            
            admineUsers1.StatusUpdated += new EventHandler(cargarNuevoUsuario);

            //tipo = this.ownerForm.tipo;          

            //picturebox2.sendtoback();




        }

        public void cargarNuevoUsuario(object sender, EventArgs e)
        {
            
            nuevoUsuario1.Enabled = true;
            nuevoUsuario1.BringToFront();
            nuevoUsuario1.Visible = true;
            }

        //Se comprueba que tipo de usuario es para en funcion de sus privilegios mostrar unos elementos u otros 
        private void comprobarPrivilegios()
        {

            if (tipoRecibido == 1)
            {
                button5.Enabled = true;
                button5.Visible = true;
            }
        }

        public void cargarClienteSeleccionado(object sender, EventArgs e)
        {
            resetearBotones(button1);

            Clientes1.Enabled = true;
            Clientes1.BringToFront();
            Clientes1.Visible = true;

            Clientes1.busquedaCliente = mascotas1.clienteActual();
            Clientes1.cargarCliente(); 


        }



        public void cargarMascotaSeleccionada(object sender, EventArgs e )
        {
            resetearBotones(button3);
            
            mascotas1.Enabled = true;
            mascotas1.BringToFront();
            mascotas1.Visible = true;

            mascotas1.id_Mascota = Clientes1.mascotaClienteAhorita();
            mascotas1.cargarMascota();
            

        }
      

        private void resetearBotones(object sender)
        {
            Button btn = (Button)sender;
            if (btn.ForeColor == Color.Black)
            {
                button1.ForeColor = Color.Black;
                button2.ForeColor = Color.Black;
                button3.ForeColor = Color.Black;
                button4.ForeColor = Color.Black;
                button5.ForeColor = Color.Black;
                //button6.ForeColor = Color.White;
                button7.ForeColor = Color.Black;
                button8.ForeColor = Color.Black;

                btn.ForeColor = Color.White;

            } 

            
        }
      
   
      

        //Sobrescrimibos el metodo de el cierre del form ya que queremos que cuando lo cerremos no solo cerremos este form sino toda la aplicacion
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //Button btn = (Button)sender;
            //if (btn.ForeColor == Color.Black)
            //{
            //    MainForm contenido = new MainForm();
            //    contenido.Show();

            //}
            //resetearBotones(sender);
           
            resetearBotones(sender);
            Clientes1.Enabled = true;
            Clientes1.BringToFront();
            Clientes1.Visible = true;
          


        }

        private void button7_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
           Application.Exit();
        
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
            nuevoCliente1.Enabled = true;
            nuevoCliente1.BringToFront();
            nuevoCliente1.Visible = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
            mascotas1.Enabled = true;
            mascotas1.BringToFront();
            mascotas1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
            NuevaMascota1.Enabled = true;
            NuevaMascota1.BringToFront();
            NuevaMascota1.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
            admineUsers1.Enabled = true;
            admineUsers1.BringToFront();
            admineUsers1.Visible = true;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
            Application.Restart();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            this.label1.Text = dateTime.ToString();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        
    }
}
