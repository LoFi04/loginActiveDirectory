using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.DirectoryServices;
using System.Data.SqlClient;

namespace loginActiveDirectory
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        string usuario = string.Empty;
        string roles_auth = string.Empty;
        int area = 0;
        int codigoEmpleado = 0;
        int idRol = 0;
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            modelo mdl = new modelo();
            string departamento = string.Empty;
            string[] valores = new string[2];
            DataTable dtUsuarios = new DataTable("usuarios");

            string mensaje = string.Empty;

            DirectoryEntry de = new DirectoryEntry("LDAP://192.168.10.90");

            ldapAuthentication autenticacion = new ldapAuthentication("LDAP://192.168.10.90");
            if (autenticacion.isAuthenitcated("LICEOJAVIER", txtUsuario.Text, txtContraseña.Text, out mensaje))
            {
                usuario = txtUsuario.Text;
                valores = autenticacion.getGroups("LICEOJAVIER", txtUsuario.Text, txtContraseña.Text, out mensaje);
                roles_auth = valores[0];
                area = Convert.ToInt16(valores[1]);

                if (roles_auth == null )
                {
                    MessageBox.Show("No esta autorizado para utilizar este modulo");
                }
                else
                {
                    departamento = autenticacion.ObtencionPropiedad("department", txtUsuario.Text, txtContraseña.Text)[0].ToString();
                    if (departamento != "")
                    {
                        try
                        {
                            codigoEmpleado = Convert.ToInt32(departamento);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    else
                    {
                        MessageBox.Show("El codigo no esta asociado al directorio");
                    }

                    DataTable dtRoles = new DataTable("roles");

                    string cadena = "select ps.id_rol,nombre, emp.nombre1 + ' ' + emp.apellido1 as nombre_empleado from permiso_siaco ps "+
                                    "INNER JOIN rol_siaco rs ON ps.id_rol=rs.id_rol and idtipomodulo=8 "+
                                    "INNER JOIN emplegen emp ON emp.empleado=ps.id_empleado "+
                                    "WHERE id_empleado ="+codigoEmpleado;
                    if (mdl.llenaTabla(cadena, dtRoles,out mensaje) > 0)
                    {
                        idRol = (int)dtRoles.Rows[0]["id_rol"];
                        roles_auth = dtRoles.Rows[0]["nombre"].ToString();
                        usuario = dtRoles.Rows[0]["nombre_empleado"].ToString();
                    }
                    else
                    {
                        idRol=0;
                    }
                    this.Hide();
                    Form f = new frmBienvenida();
                    f.ShowDialog();
                }       
            }
            else
            {
                MessageBox.Show("No es posible ingresar");
            }
        }               
    }
}
