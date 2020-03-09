// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseArtifact.cs" company="Freek Giele">
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
    /// DTO for artifacts uses during a release.
    /// </summary>
    public class AzureDevOpsReleaseArtifact
    {
        /// <summary>
        /// Gets or sets source Id.
        /// </summary>
        public string SourceId { get; set; }

        /// <summary>
        /// Gets or sets type.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether primary artifact (trigger).
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether artifact is retained.
        /// </summary>
        public bool IsRetained { get; set; }

        /// <summary>
        /// Gets or sets definition of the artifact (build).
        /// </summary>
        public AzureDevOpsDefinitionReference DefinitionReference { get; set; }
    }
}
