// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDefinition.cs" company="Freek Giele">
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
    /// DTO for release definition.
    /// </summary>
    public class AzureDevOpsReleaseDefinition
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the environments.
        /// </summary>
        public IEnumerable<AzureDevOpsReleaseDefinitionEnvironment> Environments { get; set; }

        /// <summary>
        /// Gets or sets the release triggers.
        /// </summary>
        public IEnumerable<AzureDevOpsTrigger> Triggers { get; set; }

        /// <summary>
        /// Gets or sets the url of the release definition.
        /// </summary>
        public Uri Url { get; set; }
    }
}
