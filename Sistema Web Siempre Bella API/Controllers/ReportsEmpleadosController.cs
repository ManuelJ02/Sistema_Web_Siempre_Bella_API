using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Sistema_Web_Siempre_Bella_API.Services;

namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsEmpleadosController : ControllerBase
    {
        private readonly DBServices _dBServices;

        public ReportsEmpleadosController(DBServices dBServices)
        {
            _dBServices = dBServices;
        }

        [HttpGet("Empleados")]
        public IActionResult ObtenerDatosUsuarios()
        {
            var dt = _dBServices.ObtenerDatosUsuarios();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Reportes", "Empleados.rdlc");

            LocalReport report = new LocalReport(path);
            report.AddDataSource("dsEmpleados", dt);

            var result = report.Execute(RenderType.Pdf);

            return File(result.MainStream, "application/pdf");
        }


    }
}
