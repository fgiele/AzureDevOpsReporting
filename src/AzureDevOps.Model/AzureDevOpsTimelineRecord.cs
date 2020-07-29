// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTimelineRecord.cs" company="Freek Giele">
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
    /// Timeline of the tasks run while executing a pipeline.
    /// </summary>
    public class AzureDevOpsTimelineRecord
    {
        /// <summary>
        /// Gets or sets the order of this step in the timeline.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets the displayname of the step in the timeline.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type of step in the timeline.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the task identity.
        /// </summary>
        public AzureDevOpsTask Task { get; set; }

        /// <summary>
        /// Gets or sets the state of the step in the timeline.
        /// </summary>
        public AzureDevOpsTimelineRecordState State { get; set; }

        /// <summary>
        /// Gets or sets the result of the step in the timeline.
        /// </summary>
        public AzureDevOpsTaskResult Result { get; set; }
    }
}
