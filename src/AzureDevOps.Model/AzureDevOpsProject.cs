// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsProject.cs" company="Freek Giele">
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

    public class AzureDevOpsProject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public System.Uri Url { get; set; }

        public IEnumerable<AzureDevOpsBuild> Builds { get; set; }

        public IEnumerable<AzureDevOpsRelease> Releases { get; set; }

        public IEnumerable<AzureDevOpsRepository> Repositories { get; set; }
    }
}
