// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeploymentJob.cs" company="Freek Giele">
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
    /// DTO for deployment job.
    /// </summary>
    public class AzureDevOpsDeploymentJob
    {
        /// <summary>
        /// Gets or sets tasks within the job.
        /// </summary>
        public IEnumerable<AzureDevOpsDeploymentTask> Tasks { get; set; }
    }
}
