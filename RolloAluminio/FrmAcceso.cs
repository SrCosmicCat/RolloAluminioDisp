using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace RolloAluminioDisparadores
{
    public partial class FrmAcceso : Form
    {
        Conexion Con = new Conexion();
        String SQL = "";
        MySqlCommand Query;
        MySqlDataReader Registros;

        bool existe = false;

        int id_usu1 = 0;
        String nom_usu1 = "", pass_usu1 = "";

        char[] abc = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        char[] julio = { 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'a', 'b', 'c' };

        public FrmAcceso()
        {
            InitializeComponent();

            mskPass.Enabled = false;
            btnAcceder.Enabled = false;
        }

        public string decifrado(string texto)
        {
            string NewText = "";
            for (int i = 0; i<texto.Length; i++)
            {
                for (int j = 0; j<julio.Length; j++)
                {
                    if ((texto[i] == julio[j]))
                    {
                        NewText += abc[j];
                    }
                }
            }
            return NewText;
        }

        public bool BuscaUsuario()
        {
            if (Con.abrirBD())
            {
                //MessageBox.Show("BD conectada uwu");
                try
                {
                    SQL = "SELECT * FROM Usuario WHERE nom_usu = '" + txtUsu.Text + "'";
                    Query = new MySqlCommand(SQL, Con.connection);
                    Registros = Query.ExecuteReader();
                    if (Registros.Read())
                    {
                        existe = true;

                        id_usu1 = Registros.GetInt32(0);
                        nom_usu1 = Registros.GetString(1);
                        pass_usu1 = Registros.GetString (2);
                        pass_usu1 = decifrado(pass_usu1);
                        Console.Out.WriteLine(pass_usu1);
                    }
                    else
                    {
                        existe = false;
                    }

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: " + ex);
                    existe = false;
                }
            }
            else
            {
                MessageBox.Show("BD no conectada unu");
            }
            Con.cerrarBD();
            return existe;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void btnAcceder_Click(object sender, EventArgs e)
        {
            if (BuscaUsuario())
            {
                if (mskPass.Text.Trim() != "")
                {
                    if (mskPass.Text.Equals(pass_usu1))
                    {
                        frmRollo fr = new frmRollo();
                        fr.Visible = true;
                        this.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta");
                        mskPass.Clear();
                        mskPass.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese una contraseña");
                    mskPass.Focus();
                }
            }
        }

        private void txtUsu_Enter(object sender, EventArgs e)
        {
            if (BuscaUsuario())
            {
                if (mskPass.Text.Trim() != "")
                {
                    if (mskPass.Text.Equals(pass_usu1))
                    {
                        frmRollo fr = new frmRollo();
                        fr.Visible = true;
                        this.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Contraseña incorrecta");
                        mskPass.Clear();
                        mskPass.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese una contraseña");
                    mskPass.Focus();
                }
            }
        }

        private void txtUsu_KeyPress(object sender, KeyPressEventArgs e)
        {
            int tecla = (int)e.KeyChar;
            if (tecla == 13)
            {
                if (BuscaUsuario())
                {
                    mskPass.Enabled = true;
                    mskPass.Focus();
                    btnAcceder.Enabled = true;
                    txtUsu.Enabled = false;
                }
                else
                {
                    MessageBox.Show("El usuario no existe");
                    txtUsu.Clear();
                    txtUsu.Focus();
                    btnAcceder.Enabled = false;
                }
            }
        }

        private void mskPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            int tecla = (int)e.KeyChar;
            if (!(tecla > 96 && tecla < 123 || tecla == 8 || tecla == 13))
            {
                MessageBox.Show(null, "Ingrese solo letras a-z", "Validación de datos");
                e.Handled = true; //Controla el evento de presionar una tecla
            }
        }
    }
}
