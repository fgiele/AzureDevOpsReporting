// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.ReportingTool
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using AzureDevOps.Report;
    using AzureDevOps.Scanner;

    /// <summary>
    /// Main entry point for application.
    /// </summary>
    public static class Program
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Main entry point for application.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            if (args == null)
            {
                WriteHelptext();
            }
            else
            {
                var selectedReports = ParseArguments(args);

                if (selectedReports.Count == 0)
                {
                    WriteHelptext();
                }
                else
                {
                    Run(selectedReports).Wait();
                }
            }
        }

        private static async Task Run(HashSet<IReport> reports)
        {
            var azDOUrl = new Uri(ConfigurationManager.AppSettings.Get("AzureDevOps.URL"));
            var pat = ConfigurationManager.AppSettings.Get("AzureDevOps.PAT");

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($":{pat}")));

            var scanClient = new Client(HttpClient);

            var dataOptions = reports.Select(rep => rep.DataOptions).Aggregate((x, y) => x | y);

            var azureDevOpsInstance = await scanClient.ScanAsync(dataOptions, ConfigurationManager.AppSettings.Get("AzureDevOps.Collections").Split(';'), azDOUrl).ConfigureAwait(false);
            await Generator.CreateReportsAsync(reports, azureDevOpsInstance, "C:\\Temp\\ComplianceReports").ConfigureAwait(false);
        }

        private static void WriteHelptext()
        {
            var helptext = $"The following reports are available:" + Environment.NewLine +
                            $"-a / -all: {nameof(ScanAllReport)}" + Environment.NewLine +
                            $"-b / -build: {nameof(BuildReport)}" + Environment.NewLine +
                            $"-g / -git: {nameof(GitRepositoryReport)}" + Environment.NewLine +
                            $"-r / -release: {nameof(ReleaseReport)}" + Environment.NewLine;

            Console.WriteLine(helptext);
        }

        private static HashSet<IReport> ParseArguments(string[] args)
        {
            var selectedReports = new HashSet<IReport>();

            foreach (var argument in args)
            {
                switch (argument.TrimStart('-').ToUpperInvariant())
                {
                    case "A":
                    case "ALL":
                        selectedReports.Add(new ScanAllReport());
                        break;
                    case "B":
                    case "BUILD":
                        selectedReports.Add(new BuildReport());
                        break;
                    case "G":
                    case "GIT":
                        selectedReports.Add(new GitRepositoryReport());
                        break;
                    case "R":
                    case "RELEASE":
                        selectedReports.Add(new ReleaseReport());
                        break;
                    default:
                        Console.WriteLine($"Unknown option: {argument}");
                        break;
                }
            }

            return selectedReports;
        }
    }
}
