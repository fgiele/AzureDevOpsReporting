using System;

namespace AzureDevOps.Model
{
    public class AzureDevOpsDeployApproval
    {
        public int Id { get; set; }

        public int Revision { get; set; }

        public string ApprovalType { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public int Attempt { get; set; }

        public bool IsAutomated { get; set; }

        public string Url { get; set; }

        public AzureDevOpsIdentity Approver { get; set; }

        public AzureDevOpsIdentity ApprovedBy { get; set; }
    }
}
