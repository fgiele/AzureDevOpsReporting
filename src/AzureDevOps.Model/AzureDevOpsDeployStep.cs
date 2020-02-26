using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsDeployStep
    {
        public int Attempt { get; set;}

        public IEnumerable<AzureDevOpsReleaseDeployPhase> ReleaseDeployPhases { get; set; }
    }
}
