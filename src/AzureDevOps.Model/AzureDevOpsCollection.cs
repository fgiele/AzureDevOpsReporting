using System;
using System.Collections.Generic;

namespace AzureDevOps.Model
{
    public class AzureDevOpsCollection
    {
        public AzureDevOpsCollection()
        {
            Projects = new List<AzureDevOpsProject>();
        }

        public string Name { get; set; }

        public Guid Id { get; set; }

        public List<AzureDevOpsProject> Projects { get; private set; }
    }
}
