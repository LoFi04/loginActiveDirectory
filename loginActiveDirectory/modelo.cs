using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loginActiveDirectory
{
    public class modelo
    {
        public int llenaTabla(string cadena, DataTable tabla, out string mensaje)
        {
            bool respuesta = false;
            int numeroLineas = 0;
            string nombre = string.Empty;
            mensaje = string.Empty;

            try
            {
                if (tabla == null)
                {
                    tabla = new DataTable();
                }
                else
                {
                    nombre = tabla.TableName;
                }

                using (SqlConnection cn = new SqlConnection(Conexion.cadena))
                {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(cadena,cn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                    numeroLineas = tabla.Rows.Count;

                }
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
            }
            return numeroLineas;
        }
    }
}
