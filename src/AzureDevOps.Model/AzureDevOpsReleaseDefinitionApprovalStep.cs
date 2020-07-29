// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDefinitionApprovalStep.cs" company="Freek Giele">
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
    /// DTO for release definition approval step.
    /// </summary>
    public class AzureDevOpsReleaseDefinitionApprovalStep
    {
        /// <summary>
        /// Gets or sets the approver.
        /// </summary>
        public AzureDevOpsIdentity Approver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the approval is automatic.
        /// </summary>
        public bool IsAutomated { get; set; }

        /// <summary>
        /// Gets or sets the rank of the apprval step.
        /// </summary>
        public int Rank { get; set; }
    }
}
