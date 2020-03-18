// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTask.cs" company="Freek Giele">
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
    /// DTO for Pipeline Task.
    /// </summary>
    public class AzureDevOpsTask
    {
        /// <summary>
        /// Gets or sets task ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets task version.
        /// </summary>
        public string Version { get; set; }
    }
}
