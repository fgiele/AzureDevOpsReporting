using AzureDevOps.Model;
using System;

namespace AzureDevOps.Report
{
    public class BuildReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts;

        public string Title => $"BuildReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        public string Generate(AzureDevOpsInstance instance)
        {
            CreateHeaders("Collection",
                "Project",
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
                            AddLine(collection.Name,
                                    project.Name,
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
            return GetReport();
        }
    }
}
