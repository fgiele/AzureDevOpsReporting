// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsProject.cs" company="Freek Giele">
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

    /// <summary>
    /// DTO for Azure DevOps project.
    /// </summary>
    public class AzureDevOpsProject
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public System.Uri Url { get; set; }

        /// <summary>
        /// Gets or sets builds.
        /// </summary>
        public IEnumerable<AzureDevOpsBuild> Builds { get; set; }

        /// <summary>
        /// Gets or sets releases.
        /// </summary>
        public IEnumerable<AzureDevOpsRelease> Releases { get; set; }

        /// <summary>
        /// Gets or sets repositories.
        /// </summary>
        public IEnumerable<AzureDevOpsRepository> Repositories { get; set; }
    }
}
