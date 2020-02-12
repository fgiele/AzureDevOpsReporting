using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace AzureDevOps.Model
{
    public class AzureDevOpsRelease
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url { get; set; }

        public AzureDevOpsIdentity CreatedBy { get; set; }

        public IEnumerable<AzureDevOpsEnvironment> Environments { get; set; }

        public IEnumerable<AzureDevOpsReleaseArtifact> Artifacts { get; set; }
    }

    public class AzureDevOpsReleases
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsRelease> Releases { get; set; }
    }
}
