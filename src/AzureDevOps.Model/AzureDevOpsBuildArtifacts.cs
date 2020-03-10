// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuildArtifacts.cs" company="Freek Giele">
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
    /// DTO for build artifacts.
    /// </summary>
    public class AzureDevOpsBuildArtifacts
    {
        /// <summary>
        /// Gets or sets number of found build artifacts.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets list of build artifacts.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsBuildArtifact> Artifacts { get; set; }
    }
}
