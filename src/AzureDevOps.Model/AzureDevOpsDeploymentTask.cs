// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeploymentTask.cs" company="Freek Giele">
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
    /// DTO for task executed during deployment.
    /// </summary>
    public class AzureDevOpsDeploymentTask
    {
        /// <summary>
        /// Gets or sets displayname.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets task.
        /// </summary>
        public AzureDevOpsTask Task { get; set; }
    }
}
