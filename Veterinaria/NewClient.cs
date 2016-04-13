﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Veterinaria
{
    public partial class NewClient : Form
    {
        //parametros de la conexion
        private string connStr;
        //variable que maneja la conexion
        private MySqlConnection conn;
        //variable que sirve para crear la conexion
        private static MySqlCommand comando;
        private DataTable datos = new DataTable();


        private string dni;
        private string nombre;
        private string apellido;
        private string email;
        private string telefono;
        private string direccion;
        private string fecha;

        public NewClient()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addClient();
        }

        private void addClient()
        {
            nombre = textBox1.Text;
            apellido = textBox2.Text;
            dni = textBox3.Text;
            email = textBox4.Text;
            telefono = textBox5.Text;
            direccion = textBox6.Text;
            fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();

            comando = new MySqlCommand("INSERT INTO `cliente` VALUES ('" + dni + "','" + nombre + "','" + apellido + "','" + email + "','" + telefono + "','" + direccion + "','" + fecha + "')", conn);
            comando.ExecuteNonQuery();
            conn.Close();
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            MySqlDataAdapter sda = new MySqlDataAdapter("Select * from cliente", conn);
            //Se puede realizar de esta manera con el adapter o coon un DataReader, me quedo con esta 
            //MySqlDataAdapter sda = new MySqlDataAdapter("INSERT INTO `cliente` VALUES ('"+dni+"','"+nombre+ "','" + apellido + "','" + email + "','" + telefono + "','" + direccion + "','" + fecha + "')", conn);


            this.Close();
        }

        private void button1_Click(object sender, EventArgs e )
        {

            this.Close();
        }
    }
}
