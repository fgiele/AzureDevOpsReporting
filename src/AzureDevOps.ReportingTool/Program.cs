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
    using System.Net.Http;
    using AzureDevOps.Report;
    using AzureDevOps.Scanner;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Main entry point for application.
    /// </summary>
    public static class Program
    {
        private const string ConfigKey = "config";

        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Main entry point for application.
        /// </summary>
        /// <param name="args">Program arguments.</param>
        public static void Main(string[] args)
        {
            if (args == null)
            {
                Console.WriteLine($"Please provide configuration filepath with --{ConfigKey} <ConfigFile>.");
            }
            else
            {
                var configBuilder = new ConfigurationBuilder();
                configBuilder.AddCommandLine(args);

                var config = configBuilder.Build();
                var configFile = config[ConfigKey];

                if (string.IsNullOrWhiteSpace(configFile))
                {
                    Console.WriteLine($"Please provide configuration filepath with --{ConfigKey} <ConfigFile>.");
                }
                else
                {
                    if (System.IO.File.Exists(configFile))
                    {
                        var reportTool = new ReportTool(new Client(HttpClient), new Generator());
                        reportTool.Main(configFile);
                    }
                    else
                    {
                        Console.WriteLine($"{configFile} not found.");
                    }
                }
            }
        }
    }
}
