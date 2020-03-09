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

    public class AzureDevOpsEnvironment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public IEnumerable<AzureDevOpsDeployApproval> PreDeployApprovals { get; set; }

        public IEnumerable<AzureDevOpsDeployApproval> PostDeployApprovals { get; set; }

        public AzureDevOpsDeployApprovalsSnapshot PreApprovalsSnapshot { get; set; }

        public AzureDevOpsDeployApprovalsSnapshot PostApprovalsSnapshot { get; set; }

        public IEnumerable<AzureDevOpsDeployStep> DeploySteps { get; set; }

        public DateTime CreatedOn { get; set; }

        public string TriggerReason { get; set; }
    }
}
