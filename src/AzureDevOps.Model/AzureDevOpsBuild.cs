// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuild.cs" company="Freek Giele">
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
    /// DTO for build.
    /// </summary>
    public class AzureDevOpsBuild
    {
        /// <summary>
        /// Gets or sets links from this build.
        /// </summary>
        [JsonProperty("_links")]
        public AzureDevOpsBuildLinks Links { get; set; }

        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets build number.
        /// </summary>
        public string BuildNumber { get; set; }

        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets result.
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Gets or sets url.
        /// </summary>
        public System.Uri Url { get; set; }

        /// <summary>
        /// Gets or sets repository.
        /// </summary>
        public AzureDevOpsSourceRepository Repository { get; set; }

        /// <summary>
        /// Gets or sets source branch.
        /// </summary>
        public string SourceBranch { get; set; }

        /// <summary>
        /// Gets or sets time build was started.
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// Gets or sets time build was finished.
        /// </summary>
        public string FinishTime { get; set; }

        /// <summary>
        /// Gets or sets time build was queued.
        /// </summary>
        public string QueueTime { get; set; }

        /// <summary>
        /// Gets or sets reason this build was queued.
        /// </summary>
        public AzureDevOpsBuildReason Reason { get; set; }

        /// <summary>
        /// Gets or sets artifacts.
        /// </summary>
        public IEnumerable<AzureDevOpsBuildArtifact> Artifacts { get; set; }

        /// <summary>
        /// Gets or sets the timeline.
        /// </summary>
        public AzureDevOpsBuildTimeline Timeline { get; set; }
    }
}
