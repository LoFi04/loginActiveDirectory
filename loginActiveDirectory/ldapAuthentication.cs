using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loginActiveDirectory
{
    public class ldapAuthentication
    {
        private string _path = string.Empty;
        private string _path_orignal = "LDAP://192.168.10.90";
        private string _filterAttribure = string.Empty;

        public ldapAuthentication(string path)
        {
            _path = path;
            _path_orignal = path;
        }

        public bool isAuthenitcated(string dominio, string usuario, string contraseña,out string mensaje)
        {
            mensaje = string.Empty;
            string dominioUsuario = dominio +"\\" + usuario;
            _path = _path_orignal;

            DirectoryEntry de = new DirectoryEntry(_path, dominioUsuario, contraseña);

            try
            {
                object obj = de.NativeObject;
                DirectorySearcher ds = new DirectorySearcher(de);

                ds.Filter = "SAMAccountName=" + usuario ;
                ds.PropertiesToLoad.Add("cn");
                SearchResult sr = ds.FindOne();

                if(sr == null)
                {
                    return false;
                }

                _path = sr.Path;
                _filterAttribure = sr.Properties["cn"][0].ToString();
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
            return true;

        }

        public string[] getGroups(string dominio, string usuario, string contraseña, out string mensaje)
        {
            mensaje= string.Empty;
            string[] regresa = new string[2];
            int tempNum = 0;
            string dominioUsuario = dominio +  "\"" + usuario;
            DirectoryEntry de = new DirectoryEntry(_path, dominioUsuario, contraseña);
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "cn=" + _filterAttribure;
            ds.PropertiesToLoad.Add("memberOf");

            StringBuilder groupNames = new StringBuilder();
            string nombreGrupo = string.Empty;
            bool cDirectivo, cProfesor, cCoordNivel, cCoordinador, cTutor, cEnfermeria, cPsicologia;
            int carea =0;
            cProfesor = false;
            cDirectivo = false;
            cCoordinador = false;
            cTutor = false;
            cEnfermeria = false;
            cPsicologia = false;
            cCoordNivel = false;

            try
            {
                SearchResult sr = ds.FindOne();
                int propertyCount = sr.Properties.Count;

                string dn = string.Empty;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)sr.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf('=', 1);
                    commaIndex = dn.IndexOf(',', 1);
                    if (equalsIndex == -1)
                    {
                        return null;
                    }
                    string nombGrupo = dn.Substring(equalsIndex + 1, commaIndex - equalsIndex - 1);
                    if (nombGrupo == "presupuesto")
                    {
                        regresa[0] = "P";
                        regresa[1] = "0";
                        return regresa;
                    }

                    if (tempNum != -1)
                    {
                        cProfesor = true;
                        carea = tempNum;
                    }
                }


            }
            catch (Exception ex)
            {
                regresa[0] = "0";
                regresa[0] = "0";
            }
            return regresa;
        }

        public string[] ObtencionPropiedad(string propiedad, string DomainUserName, string pwd)
        {
            string[] retorno;
            SearchResult result;
            try
            {
                using (DirectoryEntry entry = new DirectoryEntry(_path, DomainUserName, pwd))
                {
                    using (DirectorySearcher search = new DirectorySearcher(entry))
                    {
                        search.PropertiesToLoad.Add(propiedad);
                        result = search.FindOne();
                        if (result.Properties.Contains(propiedad))
                        {
                            retorno = new string[result.Properties[propiedad].Count];
                            for (int i = 0; i < result.Properties[propiedad].Count; i++)
                            {
                                retorno[i] = (string)result.Properties[propiedad][i];
                            }
                        }
                        else
                        {
                            retorno = new string[1];
                            retorno[0] = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("NO SE PUDO OBTENER LA PROPIEDAD, VERIFIQUE: " + ex.Message);
                retorno = new string[1];
                retorno[0] = "";
            }
            return retorno;
        }
    }
}
