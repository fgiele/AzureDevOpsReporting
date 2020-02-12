using System;
using System.Collections.Generic;
using System.Text;

namespace AzureDevOps.Model
{
    public class AzureDevOpsInstance
    {
        public AzureDevOpsInstance()
        {
            this.Collections = new List<AzureDevOpsCollection>();
        }

        public List<AzureDevOpsCollection> Collections { get; private set; }
    }
}
