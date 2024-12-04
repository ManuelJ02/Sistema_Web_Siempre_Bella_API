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
    public class FacturaController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public FacturaController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<FacturaModel> lista = new List<FacturaModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Facturas", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new FacturaModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                NoFactura = Convert.ToInt32(reader["NoFactura"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                TotalFactura = (decimal)(reader["TotalFactura"]),
                                FechaFactura = Convert.ToDateTime(reader["FechaFactura"]),
                                IDEmpleado = Convert.ToInt32(reader["IDEmpleado"]),
                                IDCliente = Convert.ToInt32(reader["IDCliente"])
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
        [Route("Obtener/{noFactura:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int noFactura) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<FacturaModel> lista = new List<FacturaModel>(); //Crear lista
            FacturaModel factura = new FacturaModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_Facturas", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new FacturaModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                NoFactura = Convert.ToInt32(reader["NoFactura"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                TotalFactura = (decimal)(reader["TotalFactura"]),
                                FechaFactura = Convert.ToDateTime(reader["FechaFactura"]),
                                IDEmpleado = Convert.ToInt32(reader["IDEmpleado"]),
                                IDCliente = Convert.ToInt32(reader["IDCliente"])
                            });
                        }
                    }
                }

                factura = lista.Where(item => item.NoFactura == noFactura).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = factura }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = factura }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] FacturaModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_Facturas", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("TotalFactura", Math.Round((objeto.TotalFactura),2)); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("FechaFactura", objeto.FechaFactura);
                    cmd.Parameters.AddWithValue("IDEmpleado", objeto.IDEmpleado);
                    cmd.Parameters.AddWithValue("IDCliente", objeto.IDCliente);

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


        /****
        //Editar
        [HttpPut] // Editar datos
        [Route("Editar")] //ruta para llamarlo
        public IActionResult Editar([FromBody] FacturaModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_Facturas", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("NoFactura", objeto.NoFactura == 0 ? DBNull.Value : objeto.NoFactura); // numeros con == 0 
                    cmd.Parameters.AddWithValue("TotalFactura", objeto.TotalFactura == 0 ? DBNull.Value : objeto.TotalFactura);
                    cmd.Parameters.AddWithValue("FechaFactura", objeto.FechaFactura == null ? DBNull.Value : objeto.FechaFactura); // string con is null
                    cmd.Parameters.AddWithValue("IDEmpleado", objeto.IDEmpleado == 0 ? DBNull.Value : objeto.IDEmpleado);

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
        ****/

        /****
        //Eliminar
        [HttpDelete] // Eliminar datos
        [Route("Eliminar/{noFactura:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int noFactura) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_Facturas", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("NoFactura", noFactura);

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
        ****/
    }
}
