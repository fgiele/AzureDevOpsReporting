// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseDefinitions.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// DTO for release definitions.
    /// </summary>
    public class AzureDevOpsReleaseDefinitions
    {
        /// <summary>
        /// Gets or sets number of found release definitions.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets list of release definitions.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsReleaseDefinition> ReleaseDefinitions { get; set; }
    }
}