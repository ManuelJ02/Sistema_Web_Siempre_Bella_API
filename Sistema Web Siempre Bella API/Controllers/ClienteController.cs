using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sistema_Web_Siempre_Bella_API.Models;

using System.Data;
using System.Data.SqlClient;

using Microsoft.Data.SqlClient;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;
using SqlCommand = Microsoft.Data.SqlClient.SqlCommand;

namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [EnableCors("ReglasCors")]


    [Route("api/[controller]")]

    [ApiController]
    public class ClienteController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public ClienteController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<ClienteModel> lista = new List<ClienteModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Clientes", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ClienteModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDCliente = Convert.ToInt32(reader["IDCliente"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                CedulaCliente = reader["CedulaCliente"].ToString(),
                                NombreCliente = reader["NombreCliente"].ToString(),
                                // Verificamos si "Descuento" es nulo antes de convertir
                                Descuento = reader.IsDBNull(reader.GetOrdinal("Descuento"))
                                    ? (float?)null  // Si es nulo, lo dejamos como nulo
                                    : Convert.ToSingle(reader["Descuento"]),  // Convertimos a float
                                TelefonoCliente = reader["TelefonoCliente"].ToString()
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
        [Route("Obtener/{idCliente:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idCliente) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<ClienteModel> lista = new List<ClienteModel>(); //Crear lista
            ClienteModel cliente = new ClienteModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Clientes", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ClienteModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDCliente = Convert.ToInt32(reader["IDCliente"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                CedulaCliente = reader["CedulaCliente"].ToString(),
                                NombreCliente = reader["NombreCliente"].ToString(),
                                Descuento = reader.IsDBNull(reader.GetOrdinal("Descuento"))
                                    ? (float?)null  // Si es nulo, lo dejamos como nulo
                                    : Convert.ToSingle(reader["Descuento"]),  // Convertimos a float si no es nulo
                                TelefonoCliente = reader["TelefonoCliente"].ToString()

                            });
                        }
                    }
                }

                cliente = lista.Where(item => item.IDCliente == idCliente).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = cliente }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = cliente }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] ClienteModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_Cliente", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("@CedulaCliente", objeto.CedulaCliente);
                    cmd.Parameters.AddWithValue("@NombreCliente", objeto.NombreCliente);

                    // Redondear el descuento a 2 decimales
                    cmd.Parameters.AddWithValue("@Descuento", objeto.Descuento == null ? DBNull.Value : Math.Round((float)objeto.Descuento, 2));

                    cmd.Parameters.AddWithValue("@TelefonoCliente", objeto.TelefonoCliente);

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
        public IActionResult Editar([FromBody] ClienteModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_Cliente", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("@IDCliente", objeto.IDCliente == 0 ? DBNull.Value : objeto.IDCliente);
                    cmd.Parameters.AddWithValue("@CedulaCliente", string.IsNullOrWhiteSpace(objeto.CedulaCliente) ? DBNull.Value : objeto.CedulaCliente);
                    cmd.Parameters.AddWithValue("@NombreCliente", string.IsNullOrWhiteSpace(objeto.NombreCliente) ? DBNull.Value : objeto.NombreCliente);
                    cmd.Parameters.AddWithValue("@Descuento", objeto.Descuento == null ? DBNull.Value : Math.Round((float)objeto.Descuento, 2));
                    cmd.Parameters.AddWithValue("@TelefonoCliente", string.IsNullOrWhiteSpace(objeto.TelefonoCliente) ? DBNull.Value : objeto.TelefonoCliente);

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
        [Route("Eliminar/{idCliente:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idCliente) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_Cliente", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDCliente", idCliente);

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
    }
}
