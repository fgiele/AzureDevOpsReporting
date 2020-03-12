// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsSettings.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.ReportingTool
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains the AzureDevOps configuration for the ReportingTool.
    /// </summary>
    public class AzureDevOpsSettings
    {
        /// <summary>
        /// Gets or sets PAT for Azure DevOps.
        /// </summary>
        public string PAT { get; set; }

        /// <summary>
        /// Gets or sets URL of Azure DevOps.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets collections to scan in Azure DevOps.
        /// </summary>
        public IEnumerable<string> Collections { get; set; }
    }
}
