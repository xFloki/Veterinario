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
    public partial class NewPet : Form
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


        public NewPet()
        {
            InitializeComponent();
            cargaPropietarios();
        }

        private void addPet() {
            string nombre = newNamePet.Text;
            string sexo = newSexPet.Text;
            string id = newIdMascota.Text;
            string especie = newEspeciePet.Text;
            string chip = newChipPet.Text;
            string raza = newRazaPet.Text;
            string fecha = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string propietario = newPropietarioPet.Text;
            string pasport = newPasportPet.Text;

            connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
            conn = new MySqlConnection(connStr);
            //abre la conexion
            conn.Open();

            comando = new MySqlCommand("INSERT INTO `mascota` VALUES ('" + id + "','" + nombre + "','" + pasport + "','" + sexo + "','" + especie + "','" + chip + 
                "','" + propietario + "','" + raza + "','" + fecha + "')", conn);
            comando.ExecuteNonQuery();
            conn.Close();
          
        }

        private void cargaPropietarios() {
            try
            {
                connStr = "Server=localhost; Database= veterinario; Uid=root; Pwd=root ; Port=3306";
                conn = new MySqlConnection(connStr);
                //abre la conexion
                conn.Open();
                sentencia_SQL = "select dni from cliente";
                comando = new MySqlCommand(sentencia_SQL, conn);
                resultado = comando.ExecuteReader();
                newPropietarioPet.Items.Clear();
                while (resultado.Read())
                {
                    string sName = resultado.GetString("dni");
                    newPropietarioPet.Items.Add(sName);
                    newPropietarioPet.Text = sName;

                }


            }
            catch (Exception)
            {

                throw;
            }
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addPet();
                this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
