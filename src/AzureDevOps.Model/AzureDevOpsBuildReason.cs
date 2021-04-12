// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuildReason.cs" company="Freek Giele">
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
    /// List of build reasons.
    /// </summary>
    public enum AzureDevOpsBuildReason
    {
        /// <summary>
        /// All reasons.
        /// </summary>
        All,

        /// <summary>
        /// The build was started for the trigger TriggerType.BatchedContinuousIntegration.
        /// </summary>
        BatchedCI,

        /// <summary>
        /// The build was started when another build completed.
        /// </summary>
        BuildCompletion,

        /// <summary>
        /// The build was started for the trigger ContinuousIntegrationType.Gated.
        /// </summary>
        CheckInShelveset,

        /// <summary>
        /// The build was started for the trigger TriggerType.ContinuousIntegration.
        /// </summary>
        IndividualCI,

        /// <summary>
        /// The build was started manually.
        /// </summary>
        Manual,

        /// <summary>
        /// No reason. This value should not be used.
        /// </summary>
        None,

        /// <summary>
        /// The build was started by a pull request. Added in resource version 3.
        /// </summary>
        PullRequest,

        /// <summary>
        /// The build was started when resources in pipeline triggered it
        /// </summary>
        ResourceTrigger,

        /// <summary>
        /// The build was started for the trigger TriggerType.Schedule.
        /// </summary>
        Schedule,

        /// <summary>
        /// The build was started for the trigger TriggerType.ScheduleForced.
        /// </summary>
        ScheduleForced,

        /// <summary>
        /// The build was triggered for retention policy purposes.
        /// </summary>
        Triggered,

        /// <summary>
        /// The build was created by a user.
        /// </summary>
        UserCreated,

        /// <summary>
        /// The build was started manually for private validation.
        /// </summary>
        ValidateShelveset,
    }
}
