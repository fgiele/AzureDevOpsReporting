// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeployApprovalsSnapshot.cs" company="Freek Giele">
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
    /// DTO for deployment approval snapshot.
    /// </summary>
    public class AzureDevOpsDeployApprovalsSnapshot
    {
        /// <summary>
        /// Gets or sets approvals.
        /// </summary>
        public IEnumerable<AzureDevOpsApproval> Approvals { get; set; }
    }
}
