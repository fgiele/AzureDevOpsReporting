// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsArtifactResource.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    public class AzureDevOpsArtifactResource
    {
        public string Type { get; set; }

        public System.Uri DownloadUrl { get; set; }
    }
}
