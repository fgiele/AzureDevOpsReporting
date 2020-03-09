﻿// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleases.cs" company="Freek Giele">
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

    public class AzureDevOpsReleases
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsRelease> Releases { get; set; }
    }
}
