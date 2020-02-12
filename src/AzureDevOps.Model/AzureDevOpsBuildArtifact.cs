using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsBuildArtifact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AzureDevOpsArtifactResource Resource { get; set; }
    }

    public class AzureDevOpsBuildArtifacts
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsBuildArtifact> Artifacts { get; set; }
    }
}
