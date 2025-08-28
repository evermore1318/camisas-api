using DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Models.DTOs;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DSW_PROYECTO_PALACIO_CAMISAS_WebApp.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IConfiguration _config;

        public ReportesController(IConfiguration config)
        {
            _config = config;
        }

        private List<ReporteDiarioDto> obtenerReporteDiario(DateTime fecha)
        {
            var listado = new List<ReporteDiarioDto>();
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri(_config["Services:URL"]);
                var mensaje = http.GetAsync($"Reportes/diario?fecha={fecha:yyyy-MM-dd}").Result;
                var data = mensaje.Content.ReadAsStringAsync().Result;
                listado = JsonConvert.DeserializeObject<List<ReporteDiarioDto>>(data);
            }
            return listado;
        }

        public IActionResult Index(DateTime? fecha)
        {
            List<ReporteDiarioDto> listado = new List<ReporteDiarioDto>();

            if (fecha.HasValue)
            {
                listado = obtenerReporteDiario(fecha.Value);
                ViewBag.Fecha = fecha.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                ViewBag.Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }

            return View(listado);
        }

        public IActionResult ExportarPdf(DateTime fecha)
        {
            var listado = obtenerReporteDiario(fecha);
            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms);
                var pdf = new PdfDocument(writer);
                var doc = new Document(pdf);

                doc.Add(
                    new Paragraph(
                        new Text($"Reporte Diario - {fecha:dd/MM/yyyy}")
                    )
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                );

                var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 3, 2, 2 }))
                    .UseAllAvailableWidth();

                table.AddHeaderCell("N°");
                table.AddHeaderCell("Boleta");
                table.AddHeaderCell("Marca");
                table.AddHeaderCell("Cantidad");
                table.AddHeaderCell("Precio");

                foreach (var item in listado)
                {
                    table.AddCell(new Cell().Add(new Paragraph(item.Numero.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.Boleta)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Marca)));
                    table.AddCell(new Cell().Add(new Paragraph(item.Cantidad.ToString())));
                    table.AddCell(new Cell().Add(new Paragraph(item.Precio.ToString("C"))));
                }

                doc.Add(table);
                doc.Close();

                return File(ms.ToArray(), "application/pdf",
                    $"ReporteDiario_{fecha:yyyyMMdd}.pdf");
            }
        }
    }
}
