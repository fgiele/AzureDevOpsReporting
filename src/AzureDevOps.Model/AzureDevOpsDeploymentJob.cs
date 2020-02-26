using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsDeploymentJob
    {
        public IEnumerable<AzureDevOpsDeploymentTask> Tasks { get; set; }
    }
}
