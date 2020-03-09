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

    // This report is only meant to trigger a complete scan. No output.
    public class ScanAllReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts | DataOptions.Git | DataOptions.GitPolicies | DataOptions.Release | DataOptions.ReleaseDetails;

        public string Title => "Nothing";

        public string Generate(AzureDevOpsInstance instance)
        {
            return string.Empty;
        }
    }
}
