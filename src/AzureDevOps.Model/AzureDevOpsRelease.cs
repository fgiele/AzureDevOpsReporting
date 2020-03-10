// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsRelease.cs" company="Freek Giele">
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
    /// DTO for release.
    /// </summary>
    public class AzureDevOpsRelease
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
        /// Gets or sets created on.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public System.Uri Url { get; set; }

        /// <summary>
        /// Gets or sets created by identity.
        /// </summary>
        public AzureDevOpsIdentity CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets environments.
        /// </summary>
        public IEnumerable<AzureDevOpsEnvironment> Environments { get; set; }

        /// <summary>
        /// Gets or sets artifacts.
        /// </summary>
        public IEnumerable<AzureDevOpsReleaseArtifact> Artifacts { get; set; }
    }
}
