// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDefinitionEnvironment.cs" company="Freek Giele">
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
    /// DTO for release environment.
    /// </summary>
    public class AzureDevOpsReleaseDefinitionEnvironment
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the deployment phases.
        /// </summary>
        public IEnumerable<AzureDevOpsDeployPhase> DeployPhases { get; set; }

        /// <summary>
        /// Gets or sets the pre deployment approvals.
        /// </summary>
        public AzureDevOpsReleaseDefinitionApproval PreDeployApprovals { get; set; }

        /// <summary>
        /// Gets or sets the pre deployment approvals.
        /// </summary>
        public AzureDevOpsReleaseDefinitionApproval PostDeployApprovals { get; set; }

        /// <summary>
        /// Gets or sets the conditions to deployent on this environment.
        /// </summary>
        public IEnumerable<AzureDevOpsCondition> Conditions { get; set; }
    }
}
