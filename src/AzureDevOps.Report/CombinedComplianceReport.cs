﻿// -----------------------------------------------------------------------
// <copyright file="CombinedComplianceReport.cs" company="Freek Giele">
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
    /// This report contains the combined compliance information about the use
    /// of repositories, build and release pipelines.
    /// </summary>
    public class CombinedComplianceReport : MDReportDefinition, IReport
    {
        /// <summary>
        /// Gets data-options in use with the report.
        /// </summary>
        public DataOptions DataOptions => DataOptions.Build | DataOptions.BuildArtifacts | DataOptions.Git | DataOptions.GitPolicies | DataOptions.Release | DataOptions.ReleaseDetails | DataOptions.ReleaseDefinitions;

        /// <summary>
        /// Gets title of the report.
        /// </summary>
        public string Title => $"Compliance-{DateTime.Now:yyyyMMdd-HHmmss}.md";

        private static AzureDevOpsBuildReason[] CIReason => new[] { AzureDevOpsBuildReason.BatchedCI, AzureDevOpsBuildReason.IndividualCI, AzureDevOpsBuildReason.PullRequest, AzureDevOpsBuildReason.Triggered };

        /// <summary>
        /// Parses the collected data and generates a report.
        /// </summary>
        /// <param name="instance">Instance object containing the data collected from Azure DevOps.</param>
        /// <returns>CSV string.</returns>
        public string Generate(AzureDevOpsInstance instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            // Parse per project
            foreach (var collection in instance.Collections)
            {
                foreach (var project in collection.Projects)
                {
                    this.AddText($"# {collection.Name}/{project.Name}");
                    this.AddText("## Git repositories");
                    this.MakeGitReport(project);
                    this.AddText("## Executed builds");
                    this.MakeBuildReport(project);
                    this.AddText("## Executed releases");
                    this.MakeReleaseReport(project);
                    this.AddText("## Release definitions");
                    this.MakeReleaseDefinitionReport(project);
                }
            }

            return this.GetReport();
        }

        private void MakeGitReport(AzureDevOpsProject project)
        {
            this.CreateHeaders(
                "Name",
                "Protected master",
                ">1 approvers",
                "Reset on push",
                "Comment resolution",
                "Build check",
                "Remarks",
                "Link");

            foreach (var repository in project.Repositories)
            {
                var hasPolicy = false;
                var minApprovers = false;
                var pushReset = false;
                var commentResolve = false;
                var buildRequired = false;
                foreach (var policy in repository.Policies.Where(pol => pol.IsEnabled && pol.IsBlocking))
                {
                    hasPolicy = true;
                    foreach (var scope in policy.Settings.Scope.Where(scope => scope.RefName != null && scope.RefName.Contains("master", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (policy.PolicyType.Id.ToString() == PolicyType.MinimumNumberOfReviewers)
                        {
                            minApprovers = policy.Settings.CreatorVoteCounts ? policy.Settings.MinimumApproverCount > 1 : policy.Settings.MinimumApproverCount > 0;
                            pushReset = policy.Settings.ResetOnSourcePush;
                        }

                        if (policy.PolicyType.Id.ToString() == PolicyType.ActiveComments)
                        {
                            commentResolve = true;
                        }

                        if (policy.PolicyType.Id.ToString() == PolicyType.SuccessfulBuild)
                        {
                            buildRequired = true;
                        }
                    }
                }

                this.AddRow(
                    repository.Name,
                    hasPolicy ? "(/)" : "(x)",
                    minApprovers ? "(/)" : "(x)",
                    pushReset ? "(/)" : "(x)",
                    commentResolve ? "(/)" : "(x)",
                    buildRequired ? "(/)" : "(x)",
                    string.Empty,
                    repository.WebUrl);
            }
        }

        private void MakeBuildReport(AzureDevOpsProject project)
        {
            this.CreateHeaders(
                "Name",
                "CI",
                "SonarQube",
                "Test",
                "Remarks",
                "Link");

            foreach (var build in project.Builds)
            {
                var hasSonarQube = false;
                var hasTests = false;
                var remark = string.Empty;
                var ciBuild = CIReason.Contains(build.Reason);
                if (build.Timeline == null)
                {
                    remark = "No timeline found!";
                }
                else
                {
                    hasSonarQube = build.Timeline.Records.Any(rec => rec.Task != null &&
                                                            rec.Task.Name == "SonarQubePublish" &&
                                                            (rec.Result == AzureDevOpsTaskResult.Succeeded ||
                                                            rec.Result == AzureDevOpsTaskResult.SucceededWithIssues));
                    var testTaskArray = build.Timeline.Records
                        .OrderBy(rec => rec.Order)
                        .Where(rec => rec.Task != null &&
                                    rec.Name.Contains("test", StringComparison.OrdinalIgnoreCase) &&
                                    (rec.Result == AzureDevOpsTaskResult.Succeeded ||
                                    rec.Result == AzureDevOpsTaskResult.SucceededWithIssues))?
                        .Select(rec => rec.Name).ToArray();

                    hasTests = testTaskArray != null && testTaskArray.Length > 0;
                    remark = hasTests ? $"Tests executed: {string.Join(';', testTaskArray)}" : string.Empty;
                }

                this.AddRow(
                    build.BuildNumber,
                    ciBuild ? "(/)" : "(x)",
                    hasSonarQube ? "(/)" : "(x)",
                    hasTests ? "(/)" : "(x)",
                    remark,
                    build.Links.Web.Href.AbsoluteUri);
            }
        }

        private void MakeReleaseReport(AzureDevOpsProject project)
        {
            this.CreateHeaders(
                "Name",
                "CD",
                "Environment",
                "Attempt",
                "Approval",
                "Tests",
                "Remarks",
                "Link");

            foreach (var release in project.Releases)
            {
                foreach (var environment in release.Environments)
                {
                    foreach (var attempt in environment.DeploySteps.Select(deploystep => deploystep.Attempt))
                    {
                        var cdRelease = false;
                        var hasApproval = false;
                        var hasTests = false;
                        var remark = string.Empty;

                        cdRelease = environment.TriggerReason != "Manual";
                        hasApproval = environment.PreDeployApprovals.FirstOrDefault(pda => pda.Attempt == attempt)?.ApprovedBy != null;
                        var testTaskArray = environment.DeploySteps
                            .FirstOrDefault(dep => dep.Attempt == attempt)?
                            .ReleaseDeployPhases?
                            .SelectMany(rdp => rdp.DeploymentJobs)?
                            .SelectMany(dpj => dpj.Tasks)?
                            .Where(task => task.Name.Contains("test", StringComparison.OrdinalIgnoreCase))?
                            .Select(task => task.Name).ToArray();
                        hasTests = testTaskArray != null && testTaskArray.Length > 0;
                        remark = hasTests ? $"Tests executed: {string.Join(';', testTaskArray)}" : string.Empty;

                        this.AddRow(
                            release.Name,
                            cdRelease ? "(/)" : "(x)",
                            environment.Name,
                            attempt,
                            hasApproval ? "(/)" : "(x)",
                            hasTests ? "(/)" : "(x)",
                            remark,
                            release.Links.Web.Href.AbsoluteUri);
                    }
                }
            }
        }

        private void MakeReleaseDefinitionReport(AzureDevOpsProject project)
        {
            this.CreateHeaders(
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
                    var tests = hasTests ? string.Join(", ", testTaskArray) : string.Empty;

                    this.AddRow(
                        releaseDefinition.Name,
                        environment.Name,
                        cdRelease ? "(/)" : "(x)",
                        environmentTriggers,
                        preApproval ? "(/)" : "(x)",
                        preApprovers,
                        postApproval ? "(/)" : "(x)",
                        postApprovers,
                        tests,
                        releaseDefinition.Links?.Web?.Href?.AbsoluteUri);
                }
            }
        }
    }
}
