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

    public class BuildReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts;

        public string Title => $"BuildReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

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
