using AzureDevOps.Model;

namespace AzureDevOps.Report
{
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
