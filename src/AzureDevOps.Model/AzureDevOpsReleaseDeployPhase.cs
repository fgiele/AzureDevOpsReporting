using System;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsReleaseDeployPhase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public IEnumerable<AzureDevOpsDeploymentJob> DeploymentJobs { get; set; }
    }
}
