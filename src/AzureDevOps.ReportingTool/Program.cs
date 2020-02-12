using AzureDevOps.Report;
using AzureDevOps.Scanner;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureDevOps.ReportingTool
{
    class Program
    {
        private static HashSet<IReport> Reports = new HashSet<IReport>();
        private static HttpClient httpClient = new HttpClient();

        private class Options
        {
            [Option('r', "release", Required = false, HelpText = "Generate the Release report.")]
            public bool Release { set { Reports.Add(new ReleaseReport()); } }

            [Option('g', "git", Required = false, HelpText = "Generate the Git Repository report.")]
            public bool GitRepositories { set { Reports.Add(new GitRepositoryReport()); } }

            [Option('a', "all", Required = false, HelpText = "Debug: Run all scans.")]
            public bool ScanAll { set { Reports.Add(new ScanAllReport()); } }
        }

        static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args);
            await Run();

            Console.WriteLine("done");
        }

        private static async Task Run()
        {
            var azDOUrl = ConfigurationManager.AppSettings.Get("AzureDevOps.URL");
            var pat = ConfigurationManager.AppSettings.Get("AzureDevOps.PAT");

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", "", pat))));

            var scanClient = new Client(httpClient);
            var generator = new Generator();

            var dataOptions = Reports.Select(rep => rep.DataOptions).Aggregate((x, y) => x | y);

            var azureDevOpsInstance = await scanClient.ScanAsync(dataOptions, ConfigurationManager.AppSettings.Get("AzureDevOps.Collections").Split(';'), azDOUrl);
            await generator.CreateReportsAsync(Reports, azureDevOpsInstance, "C:\\Temp\\ComplianceReports");
        }
    }
}
