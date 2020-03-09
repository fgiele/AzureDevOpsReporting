// -----------------------------------------------------------------------
// <copyright file="Generator.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using AzureDevOps.Model;

    /// <summary>
    /// Generates the reports based on collected information and report definitions.
    /// </summary>
    public static class Generator
    {
        /// <summary>
        /// Parses the data from Azure DevOps and based on the requested reports produces files.
        /// </summary>
        /// <param name="reports">Reports to be created.</param>
        /// <param name="azureDevOpsInstance">Instance object holding the Azure DevOps scanned data.</param>
        /// <param name="reportFolder">Location to write the reports to.</param>
        /// <returns>Task object/ async function.</returns>
        public static Task CreateReportsAsync(IEnumerable<IReport> reports, AzureDevOpsInstance azureDevOpsInstance, string reportFolder)
        {
            if (reports == null)
            {
                throw new ArgumentNullException(nameof(reports));
            }

            if (!Directory.Exists(reportFolder))
            {
                throw new DirectoryNotFoundException(reportFolder);
            }

            return ProcessReports(reports, azureDevOpsInstance, reportFolder);
        }

        private static async Task WriteReportAsync(string reportFilePath, string report)
        {
            await File.WriteAllTextAsync(reportFilePath, report).ConfigureAwait(false);
            Console.WriteLine($"Report generated: {reportFilePath}");
        }

        private static async Task ProcessReports(IEnumerable<IReport> reports, AzureDevOpsInstance azureDevOpsInstance, string reportFolder)
        {
            var reportTasks = new HashSet<Task>();
            foreach (var report in reports)
            {
                reportTasks.Add(WriteReportAsync(Path.Combine(reportFolder, report.Title), report.Generate(azureDevOpsInstance)));
            }

            await Task.WhenAll(reportTasks).ConfigureAwait(false);
        }
    }
}
