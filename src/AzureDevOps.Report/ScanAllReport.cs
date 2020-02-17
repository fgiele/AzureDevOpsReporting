using AzureDevOps.Model;

namespace AzureDevOps.Report
{
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
