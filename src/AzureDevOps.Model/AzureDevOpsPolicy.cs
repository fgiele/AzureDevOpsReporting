﻿// -----------------------------------------------------------------------
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

    public class AzureDevOpsPolicy
    {
        public bool IsEnabled { get; set; }

        public bool IsBlocking { get; set; }

        [JsonProperty("type")]
        public AzureDevOpsPolicyType PolicyType { get; set; }

        public AzureDevOpsPolicySettings Settings { get; set; }
    }
}