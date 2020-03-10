// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDeployPhase.cs" company="Freek Giele">
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
    /// DTO for Release deployment phase.
    /// </summary>
    public class AzureDevOpsReleaseDeployPhase
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
        /// Gets or sets status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets deployment jobs.
        /// </summary>
        public IEnumerable<AzureDevOpsDeploymentJob> DeploymentJobs { get; set; }
    }
}
