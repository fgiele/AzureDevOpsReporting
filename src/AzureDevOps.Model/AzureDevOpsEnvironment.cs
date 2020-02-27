using System;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsEnvironment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public IEnumerable<AzureDevOpsDeployApproval> PreDeployApprovals { get; set; }

        public IEnumerable<AzureDevOpsDeployApproval> PostDeployApprovals { get; set; }

        public AzureDevOpsDeployApprovalsSnapshot PreApprovalsSnapshot { get; set; }

        public AzureDevOpsDeployApprovalsSnapshot PostApprovalsSnapshot { get; set; }

        public IEnumerable<AzureDevOpsDeployStep> DeploySteps { get; set; }

        public DateTime CreatedOn { get; set; }

        public string TriggerReason { get; set; }
    }
}
