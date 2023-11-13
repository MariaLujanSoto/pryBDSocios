using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;
using System.Security.Policy;

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
            string estado = "";
            string sexo = "";
            comandoBD = new OleDbCommand();

            comandoBD.Connection = conexionBD;
            comandoBD.CommandType = System.Data.CommandType.TableDirect;  //q tipo de operacion quierp hacer y que me traiga TOD la tabla con el tabledirect
            comandoBD.CommandText = "SOCIOS"; //Que tabla traigo

            lectorBD = comandoBD.ExecuteReader(); //abre la tabla y muestra por renglon
            grilla.Columns.Add("ID", "ID");
            grilla.Columns.Add("Nombre", "Nombre");
            grilla.Columns.Add("Apellido", "Apellido");
            grilla.Columns.Add("Pais", "Pais");
            grilla.Columns.Add("Edad", "Edad");
            grilla.Columns.Add("Sexo", "Sexo");
            grilla.Columns.Add("Ingreso", "Ingreso");
            grilla.Columns.Add("Puntaje", "Puntaje");
            grilla.Columns.Add("Estado", "Estado");

            if (lectorBD.HasRows) //SI TIENE FILAS
            {
                while (lectorBD.Read()) //mientras pueda leer, mostrar (leer)
                {
                    if (lectorBD.GetBoolean(5) == true)
                    {
                        sexo = "Masculino";
                    }
                    else
                    {
                        sexo = "Femenino";
                    }
                    if (lectorBD.GetBoolean(8) == true)
                    {
                        estado = "Activo";
                    }
                    else
                    {
                        estado = "Inactivo";
                    }
                    grilla.Rows.Add(lectorBD[0], lectorBD[1], lectorBD[2], lectorBD[3], lectorBD[4], sexo, lectorBD[6], lectorBD[7], estado);

                }

            }

        }

        public void BuscarPorID(int codigo)
        {
            OleDbDataAdapter objAdaptador = new OleDbDataAdapter(); // Agregar esta línea
            OleDbCommandBuilder commandBuilder = new OleDbCommandBuilder(objAdaptador);
            DataTable dt = new DataTable();

            objAdaptador.SelectCommand = new OleDbCommand("SELECT * FROM SOCIOS", conexionBD); // Ajustar la consulta según tu estructura

            objAdaptador.Fill(dt);

            bool encontrado = false;
            foreach (DataRow registro in dt.Rows)
            {
                if ((int)registro["CODIGO_SOCIO"] == codigo)
                {
                    encontrado = true;

                    DialogResult respuesta = MessageBox.Show($"El Cliente {registro["NOMBRE"]} existe en el sistema. ¿Desea cambiar su estado?", "Consulta", MessageBoxButtons.YesNo);

                    if (respuesta == DialogResult.Yes)
                    {
                        registro.BeginEdit();
                        registro["ESTADO"] = !(bool)registro["ESTADO"];
                        registro.EndEdit();

                        // Actualizar la base de datos
                        objAdaptador.Update(dt);

                        MessageBox.Show("Estado cambiado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    break; 
                }
            }

            if (!encontrado)
            {
                MessageBox.Show("Cliente no encontrado.", "Cliente No Registrado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
