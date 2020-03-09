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
    public class AzureDevOpsApproval
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public bool IsAutomated { get; set; }

        public AzureDevOpsIdentity Approver { get; set; }
    }
}