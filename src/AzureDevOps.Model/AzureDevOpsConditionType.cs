// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsConditionType.cs" company="Freek Giele">
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
    /// Enumeration for the Condition type.
    /// </summary>
    public enum AzureDevOpsConditionType
    {
        /// <summary>
        /// The condition type is artifact.
        /// </summary>
        Artifact,

        /// <summary>
        /// The condition type is environment state.
        /// </summary>
        EnvironmentState,

        /// <summary>
        /// The condition type is event.
        /// </summary>
        Event,

        /// <summary>
        /// The condition type is undefined.
        /// </summary>
        Undefined,
    }
}