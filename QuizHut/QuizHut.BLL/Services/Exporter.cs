namespace QuizHut.BLL.Services
{
    using Microsoft.EntityFrameworkCore;
    
    using OfficeOpenXml;
    
    using QuizHut.BLL.Services.Contracts;
    using QuizHut.DLL.Entities;
    using QuizHut.DLL.Repositories.Contracts;

    public class Exporter : IExporter
    {
        private readonly IRepository<Result> repository;

        public Exporter(IRepository<Result> repository)
        {
            this.repository = repository;
        }

        public void GenerateExcelReport()
        {
            // Fetch all the results with related event data
            var resultsWithEvents = repository.All()
                .Include(r => r.Event)
                .Include(r => r.Student)
                .ToList();
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create a new Excel package
            using (var package = new ExcelPackage())
            {
                // Create the worksheet
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Set the column headers
                worksheet.Cells[1, 1].Value = "Event Name";
                worksheet.Cells[1, 2].Value = "Quiz Name";
                worksheet.Cells[1, 3].Value = "Student Name";
                worksheet.Cells[1, 4].Value = "Points";
                worksheet.Cells[1, 5].Value = "Max Points";
                worksheet.Cells[1, 6].Value = "Activation Date";

                // Populate the data rows
                for (var i = 0; i < resultsWithEvents.Count; i++)
                {
                    var result = resultsWithEvents[i];

                    worksheet.Cells[i + 2, 1].Value = result.Event.Name;
                    worksheet.Cells[i + 2, 2].Value = result.QuizName;
                    worksheet.Cells[i + 2, 3].Value = result.Student.UserName;
                    worksheet.Cells[i + 2, 4].Value = result.Points;
                    worksheet.Cells[i + 2, 5].Value = result.MaxPoints;
                    worksheet.Cells[i + 2, 6].Value = result.Event.ActivationDateAndTime;
                }

                // Auto-fit columns for better readability
                worksheet.Cells.AutoFitColumns();

                // Save the Excel package to a file
                var filePath = @"D:\Report.xlsx";
                package.SaveAs(new FileInfo(filePath));
            }
        }
    }
}
