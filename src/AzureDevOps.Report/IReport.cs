using AzureDevOps.Model;

namespace AzureDevOps.Report
{
    public interface IReport
    {
        public DataOptions DataOptions { get; }

        public string Title { get; }

        public string Generate(AzureDevOpsInstance instance);
    }
}
