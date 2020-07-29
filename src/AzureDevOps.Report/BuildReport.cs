// -----------------------------------------------------------------------
// <copyright file="BuildReport.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using System;
    using AzureDevOps.Model;

    /// <summary>
    /// Build report definition.
    /// </summary>
    public class BuildReport : CsvReportDefinition, IReport
    {
        /// <summary>
        /// Gets data-options in use with the build report.
        /// </summary>
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts;

        /// <summary>
        /// Gets title of the build report.
        /// </summary>
        public string Title => $"BuildReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        /// <summary>
        /// Parses the collected data and generates a CSV report.
        /// </summary>
        /// <param name="instance">Instance object containing the data collected from Azure DevOps.</param>
        /// <returns>CSV string.</returns>
        public string Generate(AzureDevOpsInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.CreateHeaders(
                "Collection",
                "Project",
                "Repistory",
                "Branch",
                "Build nr",
                "Build status",
                "Build result",
                "Artifact name",
                "Artifact type",
                "Artifact download");

            foreach (var collection in instance.Collections)
            {
                foreach (var project in collection.Projects)
                {
                    foreach (var build in project.Builds)
                    {
                        foreach (var artifact in build.Artifacts)
                        {
                            this.AddLine(
                                    collection.Name,
                                    project.Name,
                                    build.Repository.Name,
                                    build.SourceBranch,
                                    build.BuildNumber,
                                    build.Status,
                                    build.Result,
                                    artifact.Name,
                                    artifact.Resource.Type,
                                    artifact.Resource.DownloadUrl);
                        }
                    }
                }
            }

            return this.GetReport();
        }
    }
}
