// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsPolicySettings.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System;
    using System.Collections.Generic;

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