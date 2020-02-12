using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsPolicy
    {
        public bool IsEnabled { get; set; }

        public bool IsBlocking { get; set; }

        [JsonProperty("type")]
        public AzureDevOpsPolicyType PolicyType { get; set; }

        public AzureDevOpsPolicySettings Settings { get; set; }
    }

    public class AzureDevOpsPolicies
    {
        public int Count { get; set; }

        [JsonProperty("value")]
        public IEnumerable<AzureDevOpsPolicy> Policies { get; set; }
    }
}