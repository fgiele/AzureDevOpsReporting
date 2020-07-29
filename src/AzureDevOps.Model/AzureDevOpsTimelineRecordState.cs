// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTimelineRecordState.cs" company="Freek Giele">
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
    /// State of task within a timeline record.
    /// </summary>
    public enum AzureDevOpsTimelineRecordState
    {
        /// <summary>
        /// Completed state
        /// </summary>
        Completed,

        /// <summary>
        /// In progress state
        /// </summary>
        InProgress,

        /// <summary>
        /// Pending state
        /// </summary>
        Pending,
    }
}
