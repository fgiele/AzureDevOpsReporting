using AzureDevOps.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AzureDevOps.Report
{
    public class Generator
    {
        public Task CreateReportsAsync(IEnumerable<IReport> reports, AzureDevOpsInstance azureDevOpsInstance, string reportFolder)
        {
            if (!Directory.Exists(reportFolder))
            {
                throw new ArgumentException("Report folder does not exist", nameof(reportFolder));
            }

            return ProcessReports(reports, azureDevOpsInstance, reportFolder);
        }

        private async Task ProcessReports(IEnumerable<IReport> reports, AzureDevOpsInstance azureDevOpsInstance, string reportFolder)
        {
            var reportTasks = new HashSet<Task>();
            foreach (var report in reports)
            {
                reportTasks.Add(WriteReportAsync(Path.Combine(reportFolder, report.Title), report.Generate(azureDevOpsInstance)));
            }

            await Task.WhenAll(reportTasks);
        }

        private async Task WriteReportAsync(string reportFilePath, string report)
        {
            await File.WriteAllTextAsync(reportFilePath, report);
            Console.WriteLine($"Report generated: {reportFilePath}");
        }
    }
}
