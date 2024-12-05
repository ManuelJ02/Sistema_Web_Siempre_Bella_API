using AspNetCore.Reporting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_Web_Siempre_Bella_API.Services;
using System.Data;


namespace Sistema_Web_Siempre_Bella_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsFacturas : ControllerBase
    {
        private readonly DBServices _dBServices;

        public ReportsFacturas(DBServices dbServices)
        {
            _dBServices = dbServices;
        }

        [HttpGet("reporte-pdf/{nofactura:int}")]
        public IActionResult GetReportePdf(int nofactura) 
        {
            DataTable datos = _dBServices.ObtenerFacturaId(nofactura);

            if (datos.Rows.Count == 0)
            {
                return NotFound("No se encontro la factura con el ID Proporcionado.");
            }

            string pathRDLC = Path.Combine(Directory.GetCurrentDirectory(), "Reportes","Facturas.rdlc");

            LocalReport report = new LocalReport(pathRDLC);
            report.AddDataSource("dsFacturas", datos);

            var result = report.Execute(RenderType.Pdf, 1);

            return File(result.MainStream, "application/pdf", $"Factura_{nofactura}.pdf");
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                DataTable invoices = _dBServices.ObtenerVentas();

                var data = invoices.AsEnumerable().Select(row => new
                {
                    NoFactura = row.Field<int>("NoFactura"),
                    Fechafactura = row.Field<DateTime>("FechaFactura").ToString("yyyy-MM-dd"),
                    TotalFactura = row.Field<decimal>("TotalFactura")
                }).Take(500).ToList();

                var result = new
                {
                    draw = 1,
                    recordsTotal = data.Count,
                    recordsFiltered = data.Count,
                    data = data
                };

                return Ok(result);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
