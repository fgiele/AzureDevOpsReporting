// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTriggerType.cs" company="Freek Giele">
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
    /// Valid options for the trigger type of a release.
    /// </summary>
    public enum AzureDevOpsTriggerType
    {
        /// <summary>
        /// Artifact based release trigger.
        /// </summary>
        ArtifactSource,

        /// <summary>
        /// Container image based release trigger.
        /// </summary>
        ContainerImage,

        /// <summary>
        /// Package based release trigger.
        /// </summary>
        Package,

        /// <summary>
        /// Pull request based release trigger.
        /// </summary>
        PullRequest,

        /// <summary>
        /// Schedule based release trigger.
        /// </summary>,
        Schedule,

        /// <summary>
        /// Source repository based release trigger.
        /// </summary>
        SourceRepo,

        /// <summary>
        /// Release trigger type not set.
        /// </summary>
        Undefined,
    }
}
