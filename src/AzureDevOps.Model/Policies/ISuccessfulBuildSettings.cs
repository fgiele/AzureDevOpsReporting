namespace AzureDevOps.Model.Policies
{
    public interface ISuccessfulBuildSettings : IAzureDevOpsPolicySettings
    {
        public int BuildDefinitionId { get; set; }

        public bool QueueOnSourceUpdateOnly { get; set; }
        
        public bool ManualQueueOnly { get; set; }
        
        public string DisplayName { get; set; }

        public decimal ValidDuration { get; set; }
    }
}
