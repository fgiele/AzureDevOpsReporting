using AzureDevOps.Model;
using System;
using System.Linq;

namespace AzureDevOps.Report
{
    public class ReleaseReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Release | DataOptions.ReleaseDetails;

        public string Title => $"ReleaseReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        public string Generate(AzureDevOpsInstance instance)
        {
            CreateHeaders("Collection",
                            "Project",
                            "Release name",
                            "Release date",
                            "R. Status",
                            "Environment",
                            "E. Status",
                            "Attempt",
                            "Attempt date",
                            "Auto approve",
                            "Required approval",
                            "Approval given by",
                            "Nr. of Artifacts",
                            "Artifact versions",
                            "Source branches");

            foreach (var collection in instance.Collections)
            {
                foreach (var project in collection.Projects)
                {
                    foreach (var release in project.Releases)
                    {
                        foreach (var environment in release.Environments)
                        {
                            foreach (var preDeployApproval in environment.PreDeployApprovals)
                            {
                                AddLine(collection.Name,
                                        project.Name,
                                        release.Name,
                                        release.CreatedOn,
                                        release.Status,
                                        environment.Name,
                                        environment.Status,
                                        preDeployApproval.Attempt,
                                        preDeployApproval.CreatedOn,
                                        preDeployApproval.IsAutomated,
                                        preDeployApproval.IsAutomated ? string.Empty : $"{preDeployApproval.Approver?.DisplayName}",
                                        preDeployApproval.IsAutomated ? string.Empty : $"{preDeployApproval.ApprovedBy?.DisplayName}",
                                        release.Artifacts?.Count() ?? 0,
                                        string.Join(" & ", release.Artifacts?.Select(art => art.DefinitionReference)?.Select(def => $"{def.Definition.Name} - {def.Version.Name}")),
                                        string.Join(" & ", release.Artifacts?.Select(art => art.DefinitionReference)?.Select(def => $"{def.Branch.Name}")));
                            }
                        }
                    }
                }
            }
            return GetReport();
        }
    }
}
