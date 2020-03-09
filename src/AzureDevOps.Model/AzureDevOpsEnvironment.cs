// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsEnvironment.cs" company="Freek Giele">
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
    /// DTO for release environment.
    /// </summary>
    public class AzureDevOpsEnvironment
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets pre-deployment approvals.
        /// </summary>
        public IEnumerable<AzureDevOpsDeployApproval> PreDeployApprovals { get; set; }

        /// <summary>
        /// Gets or sets post-deployment approvals.
        /// </summary>
        public IEnumerable<AzureDevOpsDeployApproval> PostDeployApprovals { get; set; }

        /// <summary>
        /// Gets or sets pre-approval snapshot.
        /// </summary>
        public AzureDevOpsDeployApprovalsSnapshot PreApprovalsSnapshot { get; set; }

        /// <summary>
        /// Gets or sets post-approval snapshot.
        /// </summary>
        public AzureDevOpsDeployApprovalsSnapshot PostApprovalsSnapshot { get; set; }

        /// <summary>
        /// Gets or sets deployment steps.
        /// </summary>
        public IEnumerable<AzureDevOpsDeployStep> DeploySteps { get; set; }

        /// <summary>
        /// Gets or sets created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets release triggered reason.
        /// </summary>
        public string TriggerReason { get; set; }
    }
}
