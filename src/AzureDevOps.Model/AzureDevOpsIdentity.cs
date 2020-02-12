using System;

namespace AzureDevOps.Model
{
    public class AzureDevOpsIdentity
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string UniqueName { get; set; }
    }
}
