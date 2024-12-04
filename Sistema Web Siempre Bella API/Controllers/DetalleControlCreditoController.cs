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
    public class DetalleControlCreditoController : ControllerBase
    {
        //Conexion con la BD
        private readonly string CadenaSQl;

        public DetalleControlCreditoController(IConfiguration config)
        {
            CadenaSQl = config.GetConnectionString("CadenaSQL");
        }

        //------------------------------APIs-----------------------------------------
        //Listar
        [HttpGet] //mostrar datos
        [Route("Lista")] //ruta para llamarlo
        public IActionResult Lista() //Metodo (IActionResult es para...)
        {
            List<DetalleControlCreditoModel> lista = new List<DetalleControlCreditoModel>(); //Crear lista

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_DetalleControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetalleControlCreditoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDDetalleCredito = Convert.ToInt32(reader["IDDetalleCredito"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                MontoCuota = (decimal)reader["MontoCuota"],
                                NumeroCuota = Convert.ToInt32(reader["NumeroCuota"]),
                                IDTipoFactura = Convert.ToInt32(reader["IDTipoFactura"]),
                                NoFactura = Convert.ToInt32(reader["NoFactura"])

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
        [Route("Obtener/{idDetalleCredito:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
        public IActionResult Obtener(int idDetalleCredito) //Metodo (IActionResult es para...) // en los () va el parametro necesario
        {
            List<DetalleControlCreditoModel> lista = new List<DetalleControlCreditoModel>(); //Crear lista
            DetalleControlCreditoModel detalleControlCredito = new DetalleControlCreditoModel(); //Instancia la clase Modelo del Producto

            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Lista_DetalleControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos
                    cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                    using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                    {
                        while (reader.Read())
                        {
                            lista.Add(new DetalleControlCreditoModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                            {
                                //Atributos de la clase producto
                                //se convierte al tipo de dato definido en la BD
                                IDDetalleCredito = Convert.ToInt32(reader["IDDetalleCredito"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                                MontoCuota = (decimal)reader["MontoCuota"],
                                NumeroCuota = Convert.ToInt32(reader["NumeroCuota"]),
                                IDTipoFactura = Convert.ToInt32(reader["IDTipoFactura"]),
                                NoFactura = Convert.ToInt32(reader["NoFactura"])


                            });
                        }
                    }
                }

                detalleControlCredito = lista.Where(item => item.IDDetalleCredito == idDetalleCredito).FirstOrDefault(); //filtar de una lista 

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = detalleControlCredito }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = detalleControlCredito }); //Muestra error si lo hay
            }
        }

        //Insertar
        [HttpPost] // Guardar datos
        [Route("Guardar")] //ruta para llamarlo
        public IActionResult Guardar([FromBody] DetalleControlCreditoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Insertar_DetalleControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    cmd.Parameters.AddWithValue("MontoCuota", Math.Round((decimal)objeto.MontoCuota, 2)); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("NumeroCuota", objeto.NumeroCuota);
                    cmd.Parameters.AddWithValue("IDTipoFactura", objeto.IDTipoFactura);
                    cmd.Parameters.AddWithValue("NoFactura", objeto.NoFactura);

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
        public IActionResult Editar([FromBody] DetalleControlCreditoModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Editar_DetalleControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                    cmd.Parameters.AddWithValue("IDDetalleCredito", objeto.IDDetalleCredito == 0 ? DBNull.Value : objeto.IDDetalleCredito); // numeros con == 0 
                    cmd.Parameters.AddWithValue("MontoCuota", objeto.MontoCuota == 0 ? DBNull.Value : Math.Round((decimal)objeto.MontoCuota, 2));
                    cmd.Parameters.AddWithValue("NumeroCuota", objeto.NumeroCuota == 0 ? DBNull.Value : objeto.NumeroCuota); // string con is null
                    cmd.Parameters.AddWithValue("IDTipoFactura", objeto.IDTipoFactura == 0 ? DBNull.Value : objeto.IDTipoFactura);
                    cmd.Parameters.AddWithValue("NoFactura", objeto.NoFactura == 0 ? DBNull.Value : objeto.NoFactura);

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
        [Route("Eliminar/{idDetalleCredito:int}")] //ruta para llamarlo //con parametro a capturar
        public IActionResult Eliminar(int idDetalleCredito) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
        {
            try //captura errores
            {
                using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
                {
                    conexion.Open();

                    var cmd = new SqlCommand("SP_Eliminar_DetalleControlCredito", conexion); //ejecutar Procedimiento almacenado de la base de datos

                    //Definir parametros de entrada
                    // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                    cmd.Parameters.AddWithValue("IDDetalleControlCredito", idDetalleCredito);

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
        ***/
    }
}
