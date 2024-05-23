using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loginActiveDirectory
{
    public class Conexion
    {
        public static string cadena = ConfigurationManager.ConnectionStrings["conexion"].ToString();
    }
}
