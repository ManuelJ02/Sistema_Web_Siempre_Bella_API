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
    public class ControlCreditoController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public ControlCreditoController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<ControlCreditoModel> lista = new List<ControlCreditoModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_ControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ControlCreditoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDTipoFactura = Convert.ToInt32(reader["IDTipoFactura"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                FechaInicial = reader.IsDBNull(reader.GetOrdinal("FechaInicial"))
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["FechaInicial"]),
                                Interes = reader.IsDBNull(reader.GetOrdinal("Interes"))
                                    ? (float?)null  // Si es nulo, lo dejamos como nulo
                                    : Convert.ToSingle(reader["Interes"]),
                                Plazos = Convert.ToInt32(reader["Plazos"]),
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
        [Route("Obtener/{idTipoFactura:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idTipoFactura) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<ControlCreditoModel> lista = new List<ControlCreditoModel>(); //Crear lista
            ControlCreditoModel controlCredito = new ControlCreditoModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_ControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ControlCreditoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDTipoFactura = Convert.ToInt32(reader["IDTipoFactura"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                FechaInicial = reader.IsDBNull(reader.GetOrdinal("FechaInicial"))
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(reader["FechaInicial"]),
                                Interes = reader.IsDBNull(reader.GetOrdinal("Interes"))
                                    ? (float?)null  // Si es nulo, lo dejamos como nulo
                                    : Convert.ToSingle(reader["Interes"]),
                                Plazos = Convert.ToInt32(reader["Plazos"]),
                            });
                        }
                    }
                }

                controlCredito = lista.Where(item => item.IDTipoFactura == idTipoFactura).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = controlCredito }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = controlCredito }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] ControlCreditoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_ControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("FechaInicial", objeto.FechaInicial = Convert.ToDateTime(objeto.FechaInicial)); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("Interes", objeto.Interes == null ? DBNull.Value : Math.Round((float)objeto.Interes, 2));
                    cmd.Parameters.AddWithValue("Plazos", objeto.Plazos);

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
        public IActionResult Editar([FromBody] ControlCreditoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_ControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("IDTipoFactura", objeto.IDTipoFactura == 0 ? DBNull.Value : objeto.IDTipoFactura); // numeros con == 0 
                    cmd.Parameters.AddWithValue("FechaInicial", objeto.FechaInicial == null ? DBNull.Value : objeto.FechaInicial);
                    cmd.Parameters.AddWithValue("Interes", objeto.Interes == null ? DBNull.Value : Math.Round((double)objeto.Interes, 2)); // string con is null
                    cmd.Parameters.AddWithValue("Plazos", objeto.Plazos == 0 ? DBNull.Value : objeto.Plazos);

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
        ***/

        /*****
        //Eliminar
        [HttpDelete] // Eliminar datos
        [Route("Eliminar/{idTipoFactura:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idTipoFactura) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_ControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDTipoFactura", idTipoFactura);

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
