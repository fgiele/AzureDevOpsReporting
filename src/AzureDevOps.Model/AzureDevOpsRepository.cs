// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsRepository.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System;
    using System.Collections.Generic;

    public class AzureDevOpsRepository
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public System.Uri Url { get; set; }

        public string DefaultBranch { get; set; }

        public long Size { get; set; }

        public IEnumerable<AzureDevOpsPolicy> Policies { get; set; }
    }
}