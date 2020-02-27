using System;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsPolicySettings
    {
        public int MinimumApproverCount { get; set; }

        public bool CreatorVoteCounts { get; set; }

        public bool AllowDownvotes { get; set; }

        public bool ResetOnSourcePush { get; set; }

        public IEnumerable<Guid> RequiredReviewerIds { get; set; }

        public bool AllowNoFastForward { get; set; }

        public bool AllowSquash { get; set; }

        public bool AllowRebase { get; set; }

        public bool AllowRebaseMerge { get; set; }

        public int BuildDefinitionId { get; set; }

        public bool QueueOnSourceUpdateOnly { get; set; }

        public bool ManualQueueOnly { get; set; }

        public string DisplayName { get; set; }

        public decimal ValidDuration { get; set; }

        public IEnumerable<AzureDevOpsPolicyScope> Scope { get; set; }
    }
}