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

                    using (SqlDataAdapter data = new SqlDataAdapter(cmd))
                    {
                        data.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ObtenerFacturaId(int NoFactura)
        {
            DataTable dt = new DataTable();

            string query = @"
            SELECT
                *
            FROM
                vFacturas
            WHERE
                NoFactura = @NoFactura";

            string conexion = _config.GetConnectionString("CadenaSQL");

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("NoFactura", NoFactura);

                    conn.Open();

                    using (SqlDataAdapter data = new SqlDataAdapter(cmd))
                    {
                        data.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable ObtenerVentas()
        {
            DataTable dt = new DataTable();

            string query = "select NoFactura,FechaFactura,TotalFactura from Facturas";

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
