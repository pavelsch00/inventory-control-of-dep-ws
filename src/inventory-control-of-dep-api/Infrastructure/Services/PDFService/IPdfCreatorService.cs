using inventory_control_of_dep_api.Models.InventoryBook;

namespace inventory_control_of_dep_api.Infrastructure.Services.PDFService
{
    public interface IPdfCreatorService
    {
        byte[] CreatePdf(List<InventoryBookResponse> request);
    }
}