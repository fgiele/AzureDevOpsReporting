// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsRepository.cs" company="Freek Giele">
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
    /// DTO for Repository.
    /// </summary>
    public class AzureDevOpsRepository
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
        /// Gets or sets url.
        /// </summary>
        public System.Uri Url { get; set; }

        /// <summary>
        /// Gets or sets defaultBranch.
        /// </summary>
        public string DefaultBranch { get; set; }

        /// <summary>
        /// Gets or sets size.
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets policies.
        /// </summary>
        public IEnumerable<AzureDevOpsPolicy> Policies { get; set; }
    }
}