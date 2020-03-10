// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeployApproval.cs" company="Freek Giele">
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

    /// <summary>
    /// DTO for deployment approval.
    /// </summary>
    public class AzureDevOpsDeployApproval
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets revision.
        /// </summary>
        public int Revision { get; set; }

        /// <summary>
        /// Gets or sets approval type.
        /// </summary>
        public string ApprovalType { get; set; }

        /// <summary>
        /// Gets or sets created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets comments.
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets number of the attempt.
        /// </summary>
        public int Attempt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is an automatic approval.
        /// </summary>
        public bool IsAutomated { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public System.Uri Url { get; set; }

        /// <summary>
        /// Gets or sets identity of request approver.
        /// </summary>
        public AzureDevOpsIdentity Approver { get; set; }

        /// <summary>
        /// Gets or sets identity of actual approver.
        /// </summary>
        public AzureDevOpsIdentity ApprovedBy { get; set; }
    }
}
