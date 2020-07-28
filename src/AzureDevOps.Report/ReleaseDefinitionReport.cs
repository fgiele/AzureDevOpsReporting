// -----------------------------------------------------------------------
// <copyright file="ReleaseDefinitionReport.cs" company="Freek Giele">
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

    /// <summary>
    /// Release definition report definition.
    /// </summary>
    public class ReleaseDefinitionReport : CsvReportDefinition, IReport
    {
        /// <summary>
        /// Gets data-options in use with the release definition report.
        /// </summary>
        public DataOptions DataOptions => DataOptions.ReleaseDefinitions;

        /// <summary>
        /// Gets title of the release definition report.
        /// </summary>
        public string Title => $"ReleaseDefinitionReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

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
                "Name",
                "Environment",
                "CD",
                "PreviousEnvironment",
                "PreApproval",
                "PreApprover(s)",
                "PostApproval",
                "PostApprover(s)",
                "Test* tasks",
                "Link");

            foreach (var collection in instance.Collections)
            {
                foreach (var project in collection.Projects)
                {
                    foreach (var releaseDefinition in project.ReleaseDefinitions)
                    {
                        foreach (var environment in releaseDefinition.Environments)
                        {
                            var cdRelease = environment.Conditions.Any(cond => cond.ConditionType == AzureDevOpsConditionType.Artifact || cond.ConditionType == AzureDevOpsConditionType.EnvironmentState);
                            var environmentTriggers = string.Join(", ", environment.Conditions.Where(cond => cond.ConditionType == AzureDevOpsConditionType.EnvironmentState).Select(cond => cond.Name));
                            var preApproval = environment.PreDeployApprovals.Approvals.Any(app => !app.IsAutomated);
                            var preApprovers = string.Join(", ", environment.PreDeployApprovals.Approvals.Where(app => !app.IsAutomated).Select(app => app.Approver.DisplayName));
                            var postApproval = environment.PostDeployApprovals.Approvals.Any(app => !app.IsAutomated);
                            var postApprovers = string.Join(", ", environment.PostDeployApprovals.Approvals.Where(app => !app.IsAutomated).Select(app => app.Approver.DisplayName));

                            var testTaskArray = environment
                                .DeployPhases?
                                .SelectMany(depPhase => depPhase.WorkflowTasks)?
                                .Where(task => task.Name.Contains("test", StringComparison.OrdinalIgnoreCase))?
                                .Select(task => task.Name).ToArray();
                            var hasTests = testTaskArray != null && testTaskArray.Length > 0;
                            var tests = hasTests ? string.Join(';', testTaskArray) : string.Empty;

                            this.AddLine(
                                collection.Name,
                                project.Name,
                                releaseDefinition.Name,
                                environment.Name,
                                cdRelease,
                                environmentTriggers,
                                preApproval,
                                preApprovers,
                                postApproval,
                                postApprovers,
                                tests,
                                releaseDefinition.Url.AbsoluteUri);
                        }
                    }
                }
            }

            return this.GetReport();
        }
    }
}
