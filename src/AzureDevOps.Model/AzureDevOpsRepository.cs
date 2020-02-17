using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsRepository
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string DefaultBranch { get; set; }

        public long Size { get; set; }

        public IEnumerable<AzureDevOpsPolicy> Policies { get; set; }
    }
    public class AzureDevOpsRepositories
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsRepository> Repositories { get; set; }
    }
}