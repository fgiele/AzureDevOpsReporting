using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsBuild
    {
        public int Id { get; set; }

        public string BuildNumber { get; set; }

        public string Status { get; set; }

        public string Result { get; set; }

        public string Url { get; set; }

        public AzureDevOpsSourceRepository Repository { get; set; }

        public string SourceBranch { get; set; }

        public IEnumerable<AzureDevOpsBuildArtifact> Artifacts { get; set; }
    }

    public class AzureDevOpsBuilds
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsBuild> Builds { get; set; }
    }
}
