// -----------------------------------------------------------------------
// <copyright file="IGenerator.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AzureDevOps.Model;

    /// <summary>
    /// Interface for Generator.
    /// </summary>
    public interface IGenerator
    {
        /// <summary>
        /// Parses the data from Azure DevOps and based on the requested reports produces files.
        /// </summary>
        /// <param name="reports">Reports to be created.</param>
        /// <param name="azureDevOpsInstance">Instance object holding the Azure DevOps scanned data.</param>
        /// <param name="reportFolder">Location to write the reports to.</param>
        /// <returns>Task object/ async function.</returns>
        Task CreateReportsAsync(IEnumerable<IReport> reports, AzureDevOpsInstance azureDevOpsInstance, string reportFolder);
    }
}