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

    /// <summary>
    /// DTO for policy settings.
    /// </summary>
    public class AzureDevOpsPolicySettings
    {
        /// <summary>
        /// Gets or sets minimum approver count.
        /// </summary>
        public int MinimumApproverCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether creator vote counts.
        /// </summary>
        public bool CreatorVoteCounts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether allow down votes.
        /// </summary>
        public bool AllowDownvotes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether resets votes on source changes.
        /// </summary>
        public bool ResetOnSourcePush { get; set; }

        /// <summary>
        /// Gets or sets list of required reviewers.
        /// </summary>
        public IEnumerable<Guid> RequiredReviewerIds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether no fast-forward allowed.
        /// </summary>
        public bool AllowNoFastForward { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether squash merge allowed.
        /// </summary>
        public bool AllowSquash { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rebase allowed.
        /// </summary>
        public bool AllowRebase { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rebase + merge allowed.
        /// </summary>
        public bool AllowRebaseMerge { get; set; }

        /// <summary>
        /// Gets or sets build definition for build verification.
        /// </summary>
        public int BuildDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only queue build when sources changed.
        /// </summary>
        public bool QueueOnSourceUpdateOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether only queue manually.
        /// </summary>
        public bool ManualQueueOnly { get; set; }

        /// <summary>
        /// Gets or sets displayname.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets duration of validity of the build.
        /// </summary>
        public decimal ValidDuration { get; set; }

        /// <summary>
        /// Gets or sets scope of the policy.
        /// </summary>
        public IEnumerable<AzureDevOpsPolicyScope> Scope { get; set; }
    }
}