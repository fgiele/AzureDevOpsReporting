// -----------------------------------------------------------------------
// <copyright file="ReportConfiguration.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.ReportingTool
{
    using System.Collections.Generic;

    /// <summary>
    /// Contains the configuration for the ReportingTool.
    /// </summary>
    public class ReportConfiguration
    {
        /// <summary>
        /// Gets or sets settings for Azure DevOps.
        /// </summary>
        public AzureDevOpsSettings AzureDevOps { get; set; }

        /// <summary>
        /// Gets or sets reports to generate.
        /// </summary>
        public IEnumerable<string> Reports { get; set; }
    }
}
