// -----------------------------------------------------------------------
// <copyright file="DataOptions.cs" company="Freek Giele">
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
    /// Bit flag containing scanning options required for specific reports.
    /// </summary>
    [Flags]
    public enum DataOptions
    {
        /// <summary>
        /// Scan git repositories.
        /// </summary>
        Git = 1,

        /// <summary>
        /// Scan git policies.
        /// </summary>
        GitPolicies = 2,

        /// <summary>
        /// Scan builds.
        /// </summary>
        Build = 4,

        /// <summary>
        /// Scan build artifacts.
        /// </summary>
        BuildArtifacts = 8,

        /// <summary>
        /// Scan releases.
        /// </summary>
        Release = 16,

        /// <summary>
        /// Scan release details
        /// </summary>
        ReleaseDetails = 32,
    }
}
