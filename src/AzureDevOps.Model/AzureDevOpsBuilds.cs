// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuilds.cs" company="Freek Giele">
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
    /// DTO for builds
    /// </summary>
    public class AzureDevOpsBuilds
    {
        /// <summary>
        /// Gets or sets number of found builds.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets list of builds.
        /// </summary>
        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsBuild> Builds { get; set; }
    }
}
