
using inventory_control_of_dep_dal.Domain;
using inventory_control_of_dep_dal.Repository;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace inventory_control_of_dep_api.Infrastructure.Services.PDFService
{
    public class PdfCreatorService : IPdfCreatorService
    {
        private readonly IConverter _converter;
        private readonly IRepository<MaterialValue> _materialValueRepository;

        public PdfCreatorService(IConverter converter, IRepository<MaterialValue> materialValueRepository)
        {
            _converter = converter ?? throw new ArgumentNullException(nameof(converter));
            _materialValueRepository = materialValueRepository ?? throw new ArgumentNullException(nameof(materialValueRepository));
        }

        public byte[] CreatePdf()
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
                HtmlContent = TemplateGenerator.GetHTMLString(_materialValueRepository.GetAll().ToList()),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
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
