using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsDeployApprovalsSnapshot
    {
        public IEnumerable<AzureDevOpsApproval> Approvals { get; set; }
    }
}
