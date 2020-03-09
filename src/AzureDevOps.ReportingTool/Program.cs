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
    using CommandLine;

    public static class Program
    {
        private static readonly HashSet<IReport> Reports = new HashSet<IReport>();
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args);
            await Run().ConfigureAwait(false);
        }

        private static async Task Run()
        {
            var azDOUrl = new Uri(ConfigurationManager.AppSettings.Get("AzureDevOps.URL"));
            var pat = ConfigurationManager.AppSettings.Get("AzureDevOps.PAT");

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(
                    System.Text.Encoding.ASCII.GetBytes($":{pat}")));

            var scanClient = new Client(HttpClient);

            var dataOptions = Reports.Select(rep => rep.DataOptions).Aggregate((x, y) => x | y);

            var azureDevOpsInstance = await scanClient.ScanAsync(dataOptions, ConfigurationManager.AppSettings.Get("AzureDevOps.Collections").Split(';'), azDOUrl).ConfigureAwait(false);
            await Generator.CreateReportsAsync(Reports, azureDevOpsInstance, "C:\\Temp\\ComplianceReports").ConfigureAwait(false);
        }

        private class Options
        {
            [Option('a', "all", Required = false, HelpText = "Debug: Run all scans.")]
            public bool ScanAll
            {
                set
                {
                    Console.WriteLine($"Option: {value}");
                    Reports.Add(new ScanAllReport());
                }
            }

            [Option('b', "build", Required = false, HelpText = "Generate the Build report.")]
            public bool ScanBuild
            {
                set
                {
                    Console.WriteLine($"Option: {value}");
                    Reports.Add(new BuildReport());
                }
            }

            [Option('g', "git", Required = false, HelpText = "Generate the Git Repository report.")]
            public bool GitRepositories
            {
                set
                {
                    Console.WriteLine($"Option: {value}");
                    Reports.Add(new GitRepositoryReport());
                }
            }

            [Option('r', "release", Required = false, HelpText = "Generate the Release report.")]
            public bool Release
            {
                set
                {
                    Console.WriteLine($"Option: {value}");
                    Reports.Add(new ReleaseReport());
                }
            }
        }
    }
}
