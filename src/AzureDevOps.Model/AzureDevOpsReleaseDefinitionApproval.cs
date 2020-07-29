// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDefinitionApproval.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// DTO for release definition approval.
    /// </summary>
    public class AzureDevOpsReleaseDefinitionApproval
    {
        /// <summary>
        /// Gets or sets the approvals.
        /// </summary>
        public IEnumerable<AzureDevOpsReleaseDefinitionApprovalStep> Approvals { get; set; }
    }
}
