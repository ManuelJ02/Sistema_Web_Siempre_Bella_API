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
    public class DevolucionController : ControllerBase
    {
        /****
     //Conexion con la BD
     private readonly string CadenaSQl;

     public DevolucionController(IConfiguration config)
     {
         CadenaSQl = config.GetConnectionString("CadenaSQL");
     }

     //------------------------------APIs-----------------------------------------
     //Listar
     [HttpGet] //mostrar datos
     [Route("Lista")] //ruta para llamarlo
     public IActionResult Lista() //Metodo (IActionResult es para...)
     {
         List<DevolucionModel> lista = new List<DevolucionModel>(); //Crear lista

         try //captura errores
         {
             using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
             {
                 conexion.Open();

                 var cmd = new SqlCommand("SP_Lista_Devoluciones", conexion); //ejecutar Procedimiento almacenado de la base de datos
                 cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                 using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                 {
                     while (reader.Read())
                     {
                         lista.Add(new DevolucionModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                         {
                             //Atributos de la clase producto
                             //se convierte al tipo de dato definido en la BD
                             IDDevoluciones = Convert.ToInt32(reader["IDDevoluciones"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                             CantDevolver = Convert.ToInt32(reader["CantDevolver"]),
                             MotivosDevolucion = reader["MotivosDevolucion"].ToString(),
                             IDDetalleFactura = Convert.ToInt32(reader["IDDetalleFactura"]),
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
     [Route("Obtener/{idDevoluciones:int}")] //ruta para llamarlo // Especifica como recibe el parametro en la ruta con "/{}"
     public IActionResult Obtener(int idDevoluciones) //Metodo (IActionResult es para...) // en los () va el parametro necesario
     {
         List<DevolucionModel> lista = new List<DevolucionModel>(); //Crear lista
         DevolucionModel devolucion = new DevolucionModel(); //Instancia la clase Modelo del Producto

         try //captura errores
         {
             using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
             {
                 conexion.Open();

                 var cmd = new SqlCommand("SP_Lista_Devoluciones", conexion); //ejecutar Procedimiento almacenado de la base de datos
                 cmd.CommandType = CommandType.StoredProcedure; //Definir tipo de comando en este caso "SP"

                 using (var reader = cmd.ExecuteReader()) // reader para leer los resultados del comando anterior
                 {
                     while (reader.Read())
                     {
                         lista.Add(new DevolucionModel() //almacenar los resultados en la lista creada, creando un nuevo producto 
                         {
                             //Atributos de la clase producto
                             //se convierte al tipo de dato definido en la BD
                             IDDevoluciones = Convert.ToInt32(reader["IDDevoluciones"]), //el nombre dentro del [""] del ser el mismo de las columnas de las relaciones de sql
                             CantDevolver = Convert.ToInt32(reader["CantDevolver"]),
                             MotivosDevolucion = reader["MotivosDevolucion"].ToString(),
                             IDDetalleFactura = Convert.ToInt32(reader["IDDetalleFactura"]),

                         });
                     }
                 }
             }

             devolucion = lista.Where(item => item.IDDevoluciones == idDevoluciones).FirstOrDefault(); //filtar de una lista 

             return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = devolucion }); //Se muestra el producto seleccionado/filtrado con mensaje de ok

         }
         catch (Exception error)
         {
             return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = devolucion }); //Muestra error si lo hay
         }
     }

     //Insertar
     [HttpPost] // Guardar datos
     [Route("Guardar")] //ruta para llamarlo
     public IActionResult Guardar([FromBody] DevolucionModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
     {
         try //captura errores
         {
             using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
             {
                 conexion.Open();

                 var cmd = new SqlCommand("SP_Insertar_Devoluciones", conexion); //ejecutar Procedimiento almacenado de la base de datos

                 //Definir parametros de entrada
                 cmd.Parameters.AddWithValue("CantDevolver", objeto.CantDevolver); // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                 cmd.Parameters.AddWithValue("MotivosDevoluciones", objeto.MotivosDevolucion);
                 cmd.Parameters.AddWithValue("IDDetalleFactura", objeto.IDDetalleFactura);

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
     public IActionResult Editar([FromBody] DevolucionModel objeto) //Metodo (IActionResult es para...) //le pasa una struc de la clase producto
     {
         try //captura errores
         {
             using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
             {
                 conexion.Open();

                 var cmd = new SqlCommand("SP_Editar_Devoluciones", conexion); //ejecutar Procedimiento almacenado de la base de datos

                 //Definir parametros de entrada
                 // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                 // lo siguente despues de lo anterior es validar que si es nulo deje el valor anterior
                 cmd.Parameters.AddWithValue("IDDevoluciones", objeto.IDDevoluciones == 0 ? DBNull.Value : objeto.IDDevoluciones); // numeros con == 0 
                 cmd.Parameters.AddWithValue("CantDevolver", objeto.CantDevolver == 0 ? DBNull.Value : objeto.CantDevolver);
                 cmd.Parameters.AddWithValue("MotivosDevoluciones", objeto.MotivosDevolucion is null ? DBNull.Value : objeto.MotivosDevolucion);
                 cmd.Parameters.AddWithValue("IDDetalleFactura", objeto.IDDetalleFactura == 0 ? DBNull.Value : objeto.IDDetalleFactura);

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
     [Route("Eliminar/{idDevoluciones:int}")] //ruta para llamarlo //con parametro a capturar
     public IActionResult Eliminar(int idDevoluciones) //Metodo (IActionResult es para...) //le pasa un parametro para identificar el producto a eliminar
     {
         try //captura errores
         {
             using (var conexion = new SqlConnection(CadenaSQl)) //Conexion con sql
             {
                 conexion.Open();

                 var cmd = new SqlCommand("SP_Eliminar_Devoluciones", conexion); //ejecutar Procedimiento almacenado de la base de datos

                 //Definir parametros de entrada
                 // en las "" va el nombre del parametro del sql que es elque lleva el @, seguido del objeto estructura creado y . para lapropiedad que almacen a el dato (la columna de la tabla)
                 cmd.Parameters.AddWithValue("IDDevoluciones", idDevoluciones);

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
