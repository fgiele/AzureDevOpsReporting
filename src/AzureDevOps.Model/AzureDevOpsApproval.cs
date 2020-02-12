namespace AzureDevOps.Model
{
    public class AzureDevOpsApproval
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public bool IsAutomated { get; set; }

        public AzureDevOpsIdentity Approver { get; set; }
    }
}