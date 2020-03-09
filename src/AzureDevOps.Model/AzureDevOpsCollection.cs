// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsCollection.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System;
    using System.Collections.Generic;

    public class AzureDevOpsCollection
    {
        public AzureDevOpsCollection()
        {
            this.Projects = new List<AzureDevOpsProject>();
        }

        public string Name { get; set; }

        public Guid Id { get; set; }

        public List<AzureDevOpsProject> Projects { get; private set; }
    }
}
