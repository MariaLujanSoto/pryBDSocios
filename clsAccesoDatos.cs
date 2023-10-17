using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace pryBDSocios
{
    internal class clsAccesoDatos
    {
        OleDbConnection conexionBD;
        OleDbCommand comandoBD; //indica que quiero traer de las tablas
        OleDbDataReader lectorBD; //carga info para desp leer

        string cadenaConexion = @"Provider = Microsoft.ACE.OLEDB.12.0;" + " Data Source = ..\\..\\Resources\\EL_CLUB.accdb";
        
        public string estadoConexion = "";

        public string datosTabla = "";

        public void ConectarBD(){
            try
            {
                conexionBD = new OleDbConnection();
                conexionBD.ConnectionString = cadenaConexion;
                conexionBD.Open();
                estadoConexion = "Conectado";
            }
            catch (Exception ex)
            {

                estadoConexion ="Error: "+ ex.Message;
            }
            
        }

        public void TraerDatos()
        {
            comandoBD = new OleDbCommand();

            comandoBD.Connection = conexionBD;
            comandoBD.CommandType = System.Data.CommandType.TableDirect;  //q tipo de operacion quierp hacer y que me traiga TOD la tabla con el tabledirect
            comandoBD.CommandText = "SOCIOS"; //Que tabla traigo

           lectorBD = comandoBD.ExecuteReader(); //

            if (lectorBD.HasRows) //SI TIENE FILAS
            {
                while (lectorBD.Read())
                {
                    datosTabla += "-" +lectorBD[0]; //dato d la comlumna 0
                }

            }


        }
    }
}   
