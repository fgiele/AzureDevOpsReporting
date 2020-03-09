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

    public class AzureDevOpsDeployApproval
    {
        public int Id { get; set; }

        public int Revision { get; set; }

        public string ApprovalType { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }

        public int Attempt { get; set; }

        public bool IsAutomated { get; set; }

        public System.Uri Url { get; set; }

        public AzureDevOpsIdentity Approver { get; set; }

        public AzureDevOpsIdentity ApprovedBy { get; set; }
    }
}
