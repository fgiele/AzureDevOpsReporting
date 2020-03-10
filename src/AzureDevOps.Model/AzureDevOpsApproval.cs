// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsApproval.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    /// <summary>
    /// DTO for deployment approval.
    /// </summary>
    public class AzureDevOpsApproval
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets rank.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is automated approval.
        /// </summary>
        public bool IsAutomated { get; set; }

        /// <summary>
        /// Gets or sets identity of approver.
        /// </summary>
        public AzureDevOpsIdentity Approver { get; set; }
    }
}