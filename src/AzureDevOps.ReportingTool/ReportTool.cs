// -----------------------------------------------------------------------
// <copyright file="ReportTool.cs" company="Freek Giele">
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
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using AzureDevOps.Report;
    using AzureDevOps.Scanner;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Main entry point for application.
    /// </summary>
    public class ReportTool
    {
        private readonly IClient client;
        private readonly IGenerator generator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportTool"/> class.
        /// </summary>
        /// <param name="client">Instance of Client to use for scanning.</param>
        /// <param name="generator">Instance of Generator to use for creating reports.</param>
        public ReportTool(IClient client, IGenerator generator)
        {
            this.client = client;
            this.generator = generator;
        }

        /// <summary>
        /// Main entry point for application.
        /// </summary>
        /// <param name="configFile">Filepath to configuration file.</param>
        public void Main(string configFile)
        {
            if (configFile == null || string.IsNullOrWhiteSpace(configFile))
            {
                throw new ArgumentNullException(nameof(configFile));
            }

            var configPath = Path.Combine(Directory.GetCurrentDirectory(), configFile);
            if (!File.Exists(configPath))
            {
                throw new ArgumentException($"{configFile} not found", nameof(configFile));
            }

            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(configPath, false);

            var config = configBuilder.Build();
            ReportConfiguration appConfiguration = ConfigurationBinder.Get<ReportConfiguration>(config);
            var selectedReports = ParseReports(appConfiguration.Reports ?? Array.Empty<string>());

            if (selectedReports.Any())
            {
                this.Run(selectedReports, appConfiguration.AzureDevOps).Wait();
            }
            else
            {
                WriteHelptext();
            }
        }

        private static void WriteHelptext()
        {
            var helptext = $"The following reports are available:" + Environment.NewLine +
                            $"a / all: {nameof(ScanAllReport)}" + Environment.NewLine +
                            $"b / build: {nameof(BuildReport)}" + Environment.NewLine +
                            $"c / compliance: {nameof(CombinedComplianceReport)}" + Environment.NewLine +
                            $"g / git: {nameof(GitRepositoryReport)}" + Environment.NewLine +
                            $"r / release: {nameof(ReleaseReport)}" + Environment.NewLine +
                            $"t / time: {nameof(BuildDurationReport)}" + Environment.NewLine;

            Console.WriteLine(helptext);
        }

        private static HashSet<IReport> ParseReports(IEnumerable<string> args)
        {
            var selectedReports = new HashSet<IReport>();

            foreach (var argument in args)
            {
                switch (argument.ToUpperInvariant())
                {
                    case "A":
                    case "ALL":
                        selectedReports.Add(new ScanAllReport());
                        break;
                    case "B":
                    case "BUILD":
                        selectedReports.Add(new BuildReport());
                        break;
                    case "C":
                    case "COMPLIANCE":
                        selectedReports.Add(new CombinedComplianceReport());
                        break;
                    case "G":
                    case "GIT":
                        selectedReports.Add(new GitRepositoryReport());
                        break;
                    case "R":
                    case "RELEASE":
                        selectedReports.Add(new ReleaseReport());
                        break;
                    case "T":
                    case "TIME":
                        selectedReports.Add(new BuildDurationReport());
                        break;
                    default:
                        Console.WriteLine($"Unknown option: {argument}");
                        break;
                }
            }

            return selectedReports;
        }

        private async Task Run(HashSet<IReport> reports, AzureDevOpsSettings configuration)
        {
            this.client.RestClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.client.RestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($":{configuration.PAT}")));

            var dataOptions = reports.Select(rep => rep.DataOptions).Aggregate((x, y) => x | y);

            var azureDevOpsInstance = await this.client.ScanAsync(dataOptions, configuration.Collections, configuration.Url).ConfigureAwait(false);
            await this.generator.CreateReportsAsync(reports, azureDevOpsInstance, "C:\\Temp\\ComplianceReports").ConfigureAwait(false);
        }
    }
}
