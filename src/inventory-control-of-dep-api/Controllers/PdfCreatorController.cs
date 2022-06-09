using Microsoft.AspNetCore.Mvc;

using inventory_control_of_dep_api.Infrastructure.Services.PDFService;
using inventory_control_of_dep_api.Models.InventoryBook;

namespace inventory_control_of_dep_api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PdfCreatorController : Controller
    {
        private readonly IPdfCreatorService _pdfCreatorService;

        public PdfCreatorController(IPdfCreatorService pdfCreatorService)
        {
            _pdfCreatorService = pdfCreatorService ?? throw new ArgumentNullException(nameof(pdfCreatorService));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<FileContentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CreatePDF([FromBody] List<InventoryBookResponse> request)
        {
            return File(_pdfCreatorService.CreatePdf(request), "application/pdf");
        }
    }
}
