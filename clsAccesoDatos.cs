using System;
using System.Data.OleDb;
using System.Windows.Forms;
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

        public void ConectarBD()
        {
            try
            {
                conexionBD = new OleDbConnection();
                conexionBD.ConnectionString = cadenaConexion;
                conexionBD.Open();
                estadoConexion = "Conectado";
            }
            catch (Exception ex)
            {

                estadoConexion = "Error: " + ex.Message;
            }

        }

        public void TraerDatos(DataGridView grilla)
        {
            comandoBD = new OleDbCommand();

            comandoBD.Connection = conexionBD;
            comandoBD.CommandType = System.Data.CommandType.TableDirect;  //q tipo de operacion quierp hacer y que me traiga TOD la tabla con el tabledirect
            comandoBD.CommandText = "SOCIOS"; //Que tabla traigo

            lectorBD = comandoBD.ExecuteReader(); //abre la tabla y muestra por renglon
            grilla.Columns.Add("Nombre", "Nombre");
            grilla.Columns.Add("Apellido", "Apellido");
            grilla.Columns.Add("Pais", "Pais");         


            if (lectorBD.HasRows) //SI TIENE FILAS
            {
                while (lectorBD.Read()) //mientras pueda leer, mostrar (leer)
                {
                    datosTabla += "-" + lectorBD[0]; //dato d la comlumna 0
                    grilla.Rows.Add(lectorBD[1], lectorBD[2], lectorBD[3]);
                }

            }

        }

        public void BuscarPorID(int codigo){

            comandoBD = new OleDbCommand();

            comandoBD.Connection = conexionBD;
            comandoBD.CommandType = System.Data.CommandType.TableDirect;  //q tipo de operacion quierp hacer y que me traiga TOD la tabla con el tabledirect
            comandoBD.CommandText = "SOCIOS"; //Que tabla traigo

            lectorBD = comandoBD.ExecuteReader(); //abre la tabla y muestra por renglon
            


            if (lectorBD.HasRows) //SI TIENE FILAS
            {
                bool Find = false;
                while (lectorBD.Read()) //mientras pueda leer, mostrar (leer)
                {
                    if (int.Parse(lectorBD[0].ToString()) == codigo){
                        
                        MessageBox.Show("Cliente Existente: \n" + lectorBD[1].ToString() +" "+ lectorBD[2].ToString() , 
                            "lll", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Find = true;
                        break;
                    }
                   
                }
                if(Find == false){

                    MessageBox.Show("NO Existente" , "Consulta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                }
            }
        }

    }
}
