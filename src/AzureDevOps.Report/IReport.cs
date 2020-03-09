// -----------------------------------------------------------------------
// <copyright file="IReport.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using AzureDevOps.Model;

    /// <summary>
    /// Interface definition of a report.
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Gets data-options in use with the report.
        /// </summary>
        public DataOptions DataOptions { get; }

        /// <summary>
        /// Gets title of the report.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Parses the collected data and generates a CSV report.
        /// </summary>
        /// <param name="instance">Instance object containing the data collected from Azure DevOps.</param>
        /// <returns>CSV string.</returns>
        public string Generate(AzureDevOpsInstance instance);
    }
}
