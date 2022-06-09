
using inventory_control_of_dep_api.Models.InventoryBook;

using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace inventory_control_of_dep_api.Infrastructure.Services.PDFService
{
    public class PdfCreatorService : IPdfCreatorService
    {
        private readonly IConverter _converter;

        public PdfCreatorService(IConverter converter)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        }

        public byte[] CreatePdf(List<InventoryBookResponse> request)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(request),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Страница [page] из [toPage]", Line = true }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }
    }
}
