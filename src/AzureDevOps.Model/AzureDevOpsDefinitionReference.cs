namespace AzureDevOps.Model
{
    public class AzureDevOpsDefinitionReference
    {
        public AzureDevOpsReferenceField ArtifactSourceDefinitionUrl { get; set; }
        public AzureDevOpsReferenceField BuildUri { get; set; }
        public AzureDevOpsReferenceField Definition { get; set; }
        public AzureDevOpsReferenceField PullRequestMergeCommitId { get; set; }
        public AzureDevOpsReferenceField Project { get; set; }
        public AzureDevOpsReferenceField Repository { get; set; }
        public AzureDevOpsReferenceField RequestedFor { get; set; }
        public AzureDevOpsReferenceField SourceVersion { get; set; }
        public AzureDevOpsReferenceField Version { get; set; }
        public AzureDevOpsReferenceField ArtifactSourceVersionUrl { get; set; }
        public AzureDevOpsReferenceField Branch { get; set; }
    }
}