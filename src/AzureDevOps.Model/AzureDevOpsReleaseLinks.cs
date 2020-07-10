// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseLinks.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    /// <summary>
    /// DTO for buildlinks.
    /// </summary>
    public class AzureDevOpsReleaseLinks
    {
        /// <summary>
        /// Gets or sets the Uri to the Web UI of the build.
        /// </summary>
        public AzureDevOpsLink Web { get; set; }
    }
}
