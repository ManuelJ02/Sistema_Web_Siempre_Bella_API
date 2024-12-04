using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sistema_Web_Siempre_Bella_API.Models;

using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]

    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public EmpleadoController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<EmpleadoModel> lista = new List<EmpleadoModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Empleados", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new EmpleadoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDEmpleado = Convert.ToInt32(reader["IDEmpleado"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                NombreEmpleado = reader["NombreEmpleado"].ToString(),
                                CargoEmpleado = reader["CargoEmpleado"].ToString(),
                                Contraseña = reader["Contraseña"].ToString(),
                                Usuario = reader["Usuario"].ToString()
                            });
                        }
                    }

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista }); //Se muestra la lista sin problemas

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista }); //Muestra error si lo hay
            }
        }

        //Obtener
        [HttpGet] //mostrar datos
        [Route("Obtener/{idEmpleados:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idEmpleados) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<EmpleadoModel> lista = new List<EmpleadoModel>(); //Crear lista
            EmpleadoModel empleado = new EmpleadoModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Empleados", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new EmpleadoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDEmpleado = Convert.ToInt32(reader["IDEmpleado"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                NombreEmpleado = reader["NombreEmpleado"].ToString(),
                                CargoEmpleado = reader["CargoEmpleado"].ToString(),
                                Contraseña = reader["Contraseña"].ToString(),
                                Usuario = reader["Usuario"].ToString()

                            });
                        }
                    }
                }

                empleado = lista.Where(item => item.IDEmpleado == idEmpleados).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = empleado }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = empleado }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] EmpleadoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_Empleados", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("NombreEmpleado", objeto.NombreEmpleado); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("CargoEmpleado", objeto.CargoEmpleado);
                    cmd.Parameters.AddWithValue("Contraseña", objeto.Contraseña);
                    cmd.Parameters.AddWithValue("Usuario", objeto.Usuario);

                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP" 

                    cmd.ExecuteNonQuery(); // Ejecutar el creado
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" }); //Se muestra el mensaje que esta correcto

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message }); //Muestra error si lo hay
            }
        }

        //Editar
        [HttpPut] // Editar datos
        [Route("Editar")] //ruta para llamarlo
        public IActionResult Editar([FromBody] EmpleadoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_Empleados", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("IDEmpleados", objeto.IDEmpleado == 0 ? DBNull.Value : objeto.IDEmpleado); // numeros con == 0 
                    cmd.Parameters.AddWithValue("NombreEmpleado", string.IsNullOrWhiteSpace(objeto.NombreEmpleado) ? DBNull.Value : objeto.NombreEmpleado);
                    cmd.Parameters.AddWithValue("CargoEmpleado", string.IsNullOrWhiteSpace(objeto.CargoEmpleado) ? DBNull.Value : objeto.CargoEmpleado); // string con is null
                    cmd.Parameters.AddWithValue("Contraseña", string.IsNullOrWhiteSpace(objeto.Contraseña) ? DBNull.Value : objeto.Contraseña);
                    cmd.Parameters.AddWithValue("Usuario", string.IsNullOrWhiteSpace(objeto.Usuario) ? DBNull.Value : objeto.Usuario);

                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP" 

                    cmd.ExecuteNonQuery(); // Ejecutar el comando creado
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Editado" }); //Se muestra el mensaje que esta correcto

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message }); //Muestra error si lo hay
            }
        }

        //Eliminar
        [HttpDelete] // Eliminar datos
        [Route("Eliminar/{idEmpleado:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idEmpleado) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_Empleados", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDEmpleados", idEmpleado);

                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP" 

                    cmd.ExecuteNonQuery(); // Ejecutar el comando creado
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Eliminado" }); //Se muestra el mensaje que esta correcto

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message }); //Muestra error si lo hay
            }
        }

        private string HashPassword(string Contraseña)
        {
            if (string.IsNullOrEmpty(Contraseña))
            {
                throw new ArgumentNullException(nameof(Contraseña), "La contraseña no puede ser nula o vacia");
            }

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Contraseña));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public class LoginRequest
        {
            public string Usuario { get; set; }
            public string Contraseña { get; set; }
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            if(loginRequest == null || string.IsNullOrEmpty(loginRequest.Usuario) || string.IsNullOrEmpty(loginRequest.Contraseña)) 
            {
                return BadRequest("Usuario y contraseña son requeridos");
            }

            using (SqlConnection conn = new SqlConnection(CadenaSQl))
            {
                SqlCommand cmd = new SqlCommand("SP_ValidarUsuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                string hashedPassword = HashPassword(loginRequest.Contraseña);

                cmd.Parameters.AddWithValue("@Usuario", loginRequest.Usuario);
                cmd.Parameters.AddWithValue("@Contraseña", hashedPassword); // Usar contraseña hasheada

                conn.Open();
                int Autenticado = (int)cmd.ExecuteScalar();
                conn.Close();

                if (Autenticado == 1)
                {
                    return Ok(new { mensaje = "Autenticacion exitosa"});
                }
                else 
                {
                    return Unauthorized(new { mensaje = "Credenciales incorrectas" });
                }
            }
        }

    }
}
