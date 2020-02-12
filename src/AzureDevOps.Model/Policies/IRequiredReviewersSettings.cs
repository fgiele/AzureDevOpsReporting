using System;
using System.Collections.Generic;

namespace AzureDevOps.Model.Policies
{
    public class IRequiredReviewersSettings : IAzureDevOpsPolicySettings
    {
        public IEnumerable<Guid> RequiredReviewerIds { get; set; }

        public IEnumerable<AzureDevOpsPolicyScope> Scope { get; set; }
    }
}
