namespace AzureDevOps.Model
{
    public class AzureDevOpsDeploymentTask
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AzureDevOpsTask Task { get; set; }
    }
}
