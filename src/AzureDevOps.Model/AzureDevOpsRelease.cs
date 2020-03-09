// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsRelease.cs" company="Freek Giele">
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

    public class AzureDevOpsRelease
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public System.Uri Url { get; set; }

        public AzureDevOpsIdentity CreatedBy { get; set; }

        public IEnumerable<AzureDevOpsEnvironment> Environments { get; set; }

        public IEnumerable<AzureDevOpsReleaseArtifact> Artifacts { get; set; }
    }
}
