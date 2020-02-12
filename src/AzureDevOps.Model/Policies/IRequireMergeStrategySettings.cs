namespace AzureDevOps.Model.Policies
{
    public interface IRequireMergeStrategySettings : IAzureDevOpsPolicySettings
    {
        public bool AllowNoFastForward { get; set; }

        public bool AllowSquash { get; set; }

        public bool AllowRebase { get; set; }

        public bool AllowRebaseMerge { get; set; }
    }
}