// -----------------------------------------------------------------------
// <copyright file="ScanAllReport.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using AzureDevOps.Model;

    /// <summary>
    /// This report is only meant to trigger a complete scan. No output.
    /// </summary>
    public class ScanAllReport : CsvReportDefinition, IReport
    {
        /// <summary>
        /// Gets the Data-Options for this report. All possible values.
        /// </summary>
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts | DataOptions.Git | DataOptions.GitPolicies | DataOptions.Release | DataOptions.ReleaseDetails | DataOptions.ReleaseDefinitions;

        /// <summary>
        /// Gets the name of the report. Placeholder, since no report is to be written.
        /// </summary>
        public string Title => "Nothing";

        /// <summary>
        /// Generates the report. Remains empty.
        /// </summary>
        /// <param name="instance">Instance object containing the data collected from Azure DevOps.</param>
        /// <returns>Nothing.</returns>
        public string Generate(AzureDevOpsInstance instance)
        {
            return string.Empty;
        }
    }
}
