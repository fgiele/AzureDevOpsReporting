namespace AzureDevOps.Model.Policies
{
    public interface IMinimumNumberOfReviewersSettings : IAzureDevOpsPolicySettings
    {
        public int MinimumApproverCount { get; set; }

        public bool CreatorVoteCounts { get; set; }
        
        public bool AllowDownvotes { get; set; }
        
        public bool ResetOnSourcePush { get; set; }
    }
}
