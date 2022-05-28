using Microsoft.AspNetCore.Mvc;

using inventory_control_of_dep_api.Infrastructure.Services.PDFService;

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

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileContentResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public IActionResult CreatePDF()
        {
            return File(_pdfCreatorService.CreatePdf(), "application/pdf");
        }
    }
}
