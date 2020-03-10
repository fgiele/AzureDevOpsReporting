// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsPolicy.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using Newtonsoft.Json;

    /// <summary>
    /// DTO for Policy.
    /// </summary>
    public class AzureDevOpsPolicy
    {
        /// <summary>
        /// Gets or sets a value indicating whether is active.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether blocks completion of merge.
        /// </summary>
        public bool IsBlocking { get; set; }

        /// <summary>
        /// Gets or sets type of policy.
        /// </summary>
        [JsonProperty("type")]
        public AzureDevOpsPolicyType PolicyType { get; set; }

        /// <summary>
        /// Gets or sets settings of the policy.
        /// </summary>
        public AzureDevOpsPolicySettings Settings { get; set; }
    }
}