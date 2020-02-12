using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureDevOps.Model
{
    public class AzureDevOpsProject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public IEnumerable<AzureDevOpsBuild> Builds { get; set; }

        public IEnumerable<AzureDevOpsRelease> Releases { get; set; }

        public IEnumerable<AzureDevOpsRepository> Repositories { get; set; }
    }

    public class AzureDevOpsProjects
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsProject> Projects { get; set; }
    }
}
