// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTaskResult.cs" company="Freek Giele">
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
    /// Result of task within a timeline record.
    /// </summary>
    public enum AzureDevOpsTaskResult
    {
        /// <summary>
        /// Not executed, subsequently abandoned run.
        /// </summary>
        Abandoned,

        /// <summary>
        /// Not executed, cancelled.
        /// </summary>
        Canceled,

        /// <summary>
        /// Failed in execution.
        /// </summary>
        Failed,

        /// <summary>
        /// Skipped during execution.
        /// </summary>
        Skipped,

        /// <summary>
        /// Succesfully executed.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Succesfully executed with warnings.
        /// </summary>
        SucceededWithIssues,
    }
}
