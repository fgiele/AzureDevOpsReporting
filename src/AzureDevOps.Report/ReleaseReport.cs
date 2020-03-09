// -----------------------------------------------------------------------
// <copyright file="ReleaseReport.cs" company="Freek Giele">
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
    using System.Linq;
    using AzureDevOps.Model;

    public class ReleaseReport : ReportDefinition, IReport
    {
        private readonly Guid replaceTokenTaskId = new Guid("a8515ec8-7254-4ffd-912c-86772e2b5962");

        public DataOptions DataOptions => DataOptions.Release | DataOptions.ReleaseDetails;

        public string Title => $"ReleaseReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        public string Generate(AzureDevOpsInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.CreateHeaders(
                            "Collection",
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
                            "ReplacedToken?",
                            "Nr. of Artifacts",
                            "Artifact - version [branch]");

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
                                this.AddLine(
                                        collection.Name,
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
                                        this.DeploystepHasTask(environment.DeploySteps.Single(ds => ds.Attempt == preDeployApproval.Attempt), this.replaceTokenTaskId),
                                        release.Artifacts?.Count() ?? 0,
                                        string.Join(" & ", release.Artifacts?.Select(art => art.DefinitionReference)?.Select(def => $"'{def.Definition.Name} - {def.Version.Name} [{def.Branch.Name}]'")));
                            }
                        }
                    }
                }
            }

            return this.GetReport();
        }

        private bool DeploystepHasTask(AzureDevOpsDeployStep deployStep, Guid taskId)
        {
            return deployStep.
                ReleaseDeployPhases.Any(rdp =>
                    rdp.DeploymentJobs.Any(dj =>
                        dj.Tasks.Where(tsk => tsk.Task?.Id != null).Any(tsk =>
                            tsk.Task.Id.Equals(taskId))));
        }
    }
}
