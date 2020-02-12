using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevOps.Model
{
    [Flags]
    public enum DataOptions
    {
        Git = 1,
        GitPolicies = 2,
        Build = 4,
        BuildArtifacts = 8,
        Release = 16,
        ReleaseDetails = 32
    }
}
