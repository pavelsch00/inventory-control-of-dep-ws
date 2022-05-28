using System.Text;

using inventory_control_of_dep_dal.Domain;

namespace inventory_control_of_dep_api.Infrastructure.Services.PDFService
{
    public static class TemplateGenerator
    {
        public static string GetHTMLString(List<MaterialValue> materialValues)
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>InventoryNumber</th>
                                        <th>Description</th>
                                        <th>NomenclatureNumber</th>
                                        <th>PassportNumber</th>
                                    </tr>");
            foreach (var emp in materialValues)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", emp.InventoryNumber, emp.Description, emp.NomenclatureNumber, emp.PassportNumber);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
