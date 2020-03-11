// -----------------------------------------------------------------------
// <copyright file="IClient.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Scanner
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AzureDevOps.Model;

    /// <summary>
    /// Interface for client.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets httpClient used for rest calls.
        /// </summary>
        HttpClient RestClient { get; }

        /// <summary>
        /// Starts the scanning process of the Azure DevOps instance.
        /// </summary>
        /// <param name="dataOptions">Scanning options, determines the level and subjects of scanning.</param>
        /// <param name="collections">List of collections to be scanned.</param>
        /// <param name="azureDevOpsUrl">Uri of the Azure DevOps instance to be scanned.</param>
        /// <returns>Instance object holding all collected data.</returns>
        Task<AzureDevOpsInstance> ScanAsync(DataOptions dataOptions, IEnumerable<string> collections, Uri azureDevOpsUrl);
    }
}