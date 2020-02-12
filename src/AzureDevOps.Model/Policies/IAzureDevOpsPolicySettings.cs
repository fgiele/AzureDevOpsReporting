using System.Collections.Generic;

namespace AzureDevOps.Model.Policies
{
    public interface IAzureDevOpsPolicySettings
    {
        public IEnumerable<AzureDevOpsPolicyScope> Scope { get; set; }
    }
}
