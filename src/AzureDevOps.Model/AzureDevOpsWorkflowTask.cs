// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsWorkflowTask.cs" company="Freek Giele">
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

    /// <summary>
    /// DTO for Workflow Task.
    /// </summary>
    public class AzureDevOpsWorkflowTask
    {
        /// <summary>
        /// Gets or sets the task ID.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the step is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the definition type.
        /// </summary>
        public string DefinitionType { get; set; }
    }
}
