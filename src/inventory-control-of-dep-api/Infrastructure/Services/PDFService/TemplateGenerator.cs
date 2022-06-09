using System.Text;

using inventory_control_of_dep_api.Models.InventoryBook;

namespace inventory_control_of_dep_api.Infrastructure.Services.PDFService
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(List<InventoryBookResponse> request)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Инвентаризационная опись</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Название</th>
                                        <th>Категория</th>
                                        <th>Тип операции</th>
                                        <th>Дата</th>
                                        <th>Номер аудитории</th>
                                        <th>Инвентарный номер</th>
                                        <th>Номенклатурный номер</th>
                                    </tr>");
            foreach (var emp in request)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                    <td>{5}</td>
                                    <td>{6}</td>
                                  </tr>", emp.MaterialValueName, emp.CategoryName, emp.OperationTypeName, 
                                  emp.Date, emp.RoomNumber, emp.MaterialValuInventoryNumber, emp.NomenclatureNumber);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
