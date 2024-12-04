using System.Data;
using System.Data.SqlClient;

namespace Sistema_Web_Siempre_Bella_API.Services
{
    public class DBServices
    {
        private readonly IConfiguration _config;

        public DBServices(IConfiguration config)
        {
            _config = config;
        }

        public DataTable ObtenerDatosUsuarios()
        {
            DataTable dt = new DataTable();

            string query = "Select * FROM Empleados";

            string conexion = _config.GetConnectionString("CadenaSQL");

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using( SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ObtenerDatosClientes()
        {
            DataTable dt = new DataTable();

            string query = "Select * FROM Clientes";

            string conexion = _config.GetConnectionString("CadenaSQL");

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
