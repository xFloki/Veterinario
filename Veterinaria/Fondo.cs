﻿using System;
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
       

        public Fondo()
        {
            
            InitializeComponent();
            //Creamos un eventHandler al que le pasamos una evento de click para poder ejecutar desde este form el evento de click
            // de un boton que se encuentra en el el UserControl "Clientes1"
            Clientes1.StatusUpdated += new EventHandler(cargarMascotaSeleccionada);
            
            //picturebox2.sendtoback();


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
        }

        private void button8_Click(object sender, EventArgs e)
        {
            resetearBotones(sender);
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
