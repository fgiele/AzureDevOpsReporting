using System;

namespace AzureDevOps.Model
{
    public class AzureDevOpsSourceRepository
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
    }
}