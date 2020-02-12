using System;

namespace AzureDevOps.Model
{
    public class AzureDevOpsPolicyScope
    {
        public Guid repositoryId { get; set; }

        public string RefName { get; set; }

        public string MatchKind { get; set; }
    }
}
