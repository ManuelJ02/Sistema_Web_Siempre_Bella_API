using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Web_Siempre_Bella_API.Services;

namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsClientesController : ControllerBase
    {
        private readonly DBServices _dBServices;

        public ReportsClientesController(DBServices dBServices)
        {
            _dBServices = dBServices;
        }

        [HttpGet("Clientes")]
        public IActionResult ObtenerDatosClientes()
        {
            var dt = _dBServices.ObtenerDatosClientes();

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Reportes", "Clientes.rdlc");

            LocalReport report = new LocalReport(path);
            report.AddDataSource("dsClientes", dt);

            var result = report.Execute(RenderType.Pdf);

            return File(result.MainStream, "application/pdf");
        }
    }
}
