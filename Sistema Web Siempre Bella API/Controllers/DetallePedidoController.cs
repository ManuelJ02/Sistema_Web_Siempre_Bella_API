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
    public class DetallePedidoController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public DetallePedidoController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<DetallePedidoModel> lista = new List<DetallePedidoModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_DetallesPedido", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetallePedidoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDDetallePedido = Convert.ToInt32(reader["IDDetallePedido"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                CantidadProductos = Convert.ToInt32(reader["CantidadProductos"]),
                                IDProducto = Convert.ToInt32(reader["IDProducto"]),
                                SubtotalPedido = (decimal)(reader["SubtotalPedido"]),
                                NoPedido = Convert.ToInt32(reader["NoPedido"])

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
        [Route("Obtener/{idDetallePedido:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idDetallePedido) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<DetallePedidoModel> lista = new List<DetallePedidoModel>(); //Crear lista
            DetallePedidoModel detallePedido = new DetallePedidoModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_DetallesPedido", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetallePedidoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDDetallePedido = Convert.ToInt32(reader["IDDetallePedido"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                CantidadProductos = Convert.ToInt32(reader["CantidadProductos"]),
                                IDProducto = Convert.ToInt32(reader["IDProdcuto"]),
                                SubtotalPedido = (decimal)(reader["SubtotalPedido"]),
                                NoPedido = Convert.ToInt32(reader["NoPedido"])

                            });
                        }
                    }
                }

                detallePedido = lista.Where(item => item.IDDetallePedido == idDetallePedido).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = detallePedido }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = detallePedido }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] DetallePedidoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_DetallesPedido", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("CantidadProductos", objeto.CantidadProductos); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDProducto", objeto.IDProducto);
                    cmd.Parameters.AddWithValue("SubtotalPedido", Math.Round((decimal)objeto.SubtotalPedido, 2));
                    cmd.Parameters.AddWithValue("NoPedido", objeto.NoPedido);

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
        public IActionResult Editar([FromBody] DetallePedidoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_DetallesPedido", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("IDDetallePedido", objeto.IDDetallePedido == 0 ? DBNull.Value : objeto.IDDetallePedido); // numeros con == 0 
                    cmd.Parameters.AddWithValue("CantidadProductos", objeto.CantidadProductos == 0 ? DBNull.Value : objeto.CantidadProductos);
                    cmd.Parameters.AddWithValue("IDProducto", objeto.IDProducto == 0 ? DBNull.Value : objeto.IDProducto);
                    cmd.Parameters.AddWithValue("SubtotalPedido", objeto.SubtotalPedido == 0 ? DBNull.Value : Math.Round((decimal)objeto.SubtotalPedido, 2));
                    cmd.Parameters.AddWithValue("NoPedido", objeto.NoPedido == 0 ? DBNull.Value : objeto.NoPedido);

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


        /****
        //Eliminar
        [HttpDelete] // Eliminar datos
        [Route("Eliminar/{idDetallePedido:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idDetallePedido) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_DetallesPedido", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDDetallePedido", idDetallePedido);

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
