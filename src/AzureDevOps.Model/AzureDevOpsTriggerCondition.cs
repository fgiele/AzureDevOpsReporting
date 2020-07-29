// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTriggerCondition.cs" company="Freek Giele">
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
    /// DTO for release definition trigger.
    /// </summary>
    public class AzureDevOpsTriggerCondition
    {
        /// <summary>
        /// Gets or sets the source branch.
        /// </summary>
        public string SourceBranch { get; set; }
    }
}