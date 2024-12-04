using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sistema_Web_Siempre_Bella_API.Models;

using System.Data;
using System.Data.SqlClient;

namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [EnableCors("ReglasCors")]

    [Route("api/[controller]")]

    [ApiController]
    public class ProveedorController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public ProveedorController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<ProveedorModel> lista = new List<ProveedorModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Proveedores", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ProveedorModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDProveedores = Convert.ToInt32(reader["IDProveedores"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                NombreEmpresa = reader["NombreEmpresa"].ToString(),
                                NombreGerenteVentas = reader["NombreGerenteVentas"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString(),
                                Direccion = reader["Direccion"].ToString()

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
        [Route("Obtener/{idProveedores:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idProveedores) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<ProveedorModel> lista = new List<ProveedorModel>(); //Crear lista
            ProveedorModel proveedor = new ProveedorModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Proveedores", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ProveedorModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDProveedores = Convert.ToInt32(reader["IDProveedores"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                NombreEmpresa = reader["NombreEmpresa"].ToString(),
                                NombreGerenteVentas = reader["NombreGerenteVentas"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Email = reader["Email"].ToString(),
                                Direccion = reader["Direccion"].ToString()

                            });
                        }
                    }
                }

                proveedor = lista.Where(item => item.IDProveedores == idProveedores).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = proveedor }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = proveedor }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] ProveedorModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_Proveedores", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("NombreEmpresa", objeto.NombreEmpresa); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("NombreGerenteVentas", objeto.NombreGerenteVentas);
                    cmd.Parameters.AddWithValue("Telefono", objeto.Telefono);
                    cmd.Parameters.AddWithValue("Email", objeto.Email);
                    cmd.Parameters.AddWithValue("Direccion", objeto.Direccion);

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
        public IActionResult Editar([FromBody] ProveedorModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_Proveedores", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("IDProveedores", objeto.IDProveedores == 0 ? DBNull.Value : objeto.IDProveedores); // numeros con == 0 
                    cmd.Parameters.AddWithValue("NombreEmpresa", string.IsNullOrWhiteSpace(objeto.NombreEmpresa) ? DBNull.Value : objeto.NombreEmpresa);
                    cmd.Parameters.AddWithValue("NombreGerenteVentas", string.IsNullOrWhiteSpace(objeto.NombreGerenteVentas) ? DBNull.Value : objeto.NombreGerenteVentas); // string con is null
                    cmd.Parameters.AddWithValue("Telefono", string.IsNullOrWhiteSpace(objeto.Telefono) ? DBNull.Value : objeto.Telefono);
                    cmd.Parameters.AddWithValue("Email", string.IsNullOrWhiteSpace(objeto.Email) ? DBNull.Value : objeto.Email);
                    cmd.Parameters.AddWithValue("Direccion", string.IsNullOrWhiteSpace(objeto.Direccion) ? DBNull.Value : objeto.Direccion);

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
        [Route("Eliminar/{idProveedores:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idProveedores) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_Proveedores", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDProveedores", idProveedores);

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
