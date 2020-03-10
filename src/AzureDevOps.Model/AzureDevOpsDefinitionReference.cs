// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDefinitionReference.cs" company="Freek Giele">
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
    /// DTO for release artifact definition reference.
    /// </summary>
    public class AzureDevOpsDefinitionReference
    {
        /// <summary>
        /// Gets or sets artifact source definition url.
        /// </summary>
        public AzureDevOpsReferenceField ArtifactSourceDefinitionUrl { get; set; }

        /// <summary>
        /// Gets or sets build url.
        /// </summary>
        public AzureDevOpsReferenceField BuildUri { get; set; }

        /// <summary>
        /// Gets or sets definition.
        /// </summary>
        public AzureDevOpsReferenceField Definition { get; set; }

        /// <summary>
        /// Gets or sets pull request merge commit id.
        /// </summary>
        public AzureDevOpsReferenceField PullRequestMergeCommitId { get; set; }

        /// <summary>
        /// Gets or sets project.
        /// </summary>
        public AzureDevOpsReferenceField Project { get; set; }

        /// <summary>
        /// Gets or sets source repository.
        /// </summary>
        public AzureDevOpsReferenceField Repository { get; set; }

        /// <summary>
        /// Gets or sets identity of requestor.
        /// </summary>
        public AzureDevOpsReferenceField RequestedFor { get; set; }

        /// <summary>
        /// Gets or sets version in source.
        /// </summary>
        public AzureDevOpsReferenceField SourceVersion { get; set; }

        /// <summary>
        /// Gets or sets version.
        /// </summary>
        public AzureDevOpsReferenceField Version { get; set; }

        /// <summary>
        /// Gets or sets artifact source version url.
        /// </summary>
        public AzureDevOpsReferenceField ArtifactSourceVersionUrl { get; set; }

        /// <summary>
        /// Gets or sets branch.
        /// </summary>
        public AzureDevOpsReferenceField Branch { get; set; }
    }
}