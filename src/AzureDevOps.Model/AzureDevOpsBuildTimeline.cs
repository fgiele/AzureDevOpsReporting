// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuildTimeline.cs" company="Freek Giele">
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

    /// <summary>
    /// Timeline of jobs and tasks executed in the pipeline.
    /// </summary>
    public class AzureDevOpsBuildTimeline
    {
        /// <summary>
        /// Gets or sets the records in the timeline.
        /// </summary>
        public IEnumerable<AzureDevOpsTimelineRecord> Records { get; set; }
    }
}
