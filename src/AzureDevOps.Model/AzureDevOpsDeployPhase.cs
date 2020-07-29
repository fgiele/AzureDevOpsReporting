// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeployPhase.cs" company="Freek Giele">
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
    public class AzureDevOpsDeployPhase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rank of the approval step.
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Gets or sets the workflow tasks.
        /// </summary>
        public IEnumerable<AzureDevOpsWorkflowTask> WorkflowTasks { get; set; }
    }
}
