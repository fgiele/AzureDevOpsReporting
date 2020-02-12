namespace AzureDevOps.Model
{
    public class AzureDevOpsReleaseArtifact
    {
        public string SourceId { get; set; }

        public string Type { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsRetained { get; set; }

        public AzureDevOpsDefinitionReference DefinitionReference { get; set; }
    }
}
