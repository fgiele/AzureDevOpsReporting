// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuild.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System.Collections.Generic;

    public class AzureDevOpsBuild
    {
        public int Id { get; set; }

        public string BuildNumber { get; set; }

        public string Status { get; set; }

        public string Result { get; set; }

        public System.Uri Url { get; set; }

        public AzureDevOpsSourceRepository Repository { get; set; }

        public string SourceBranch { get; set; }

        public IEnumerable<AzureDevOpsBuildArtifact> Artifacts { get; set; }
    }
}
