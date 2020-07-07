// -----------------------------------------------------------------------
// <copyright file="GitRepositoryReport.cs" company="Freek Giele">
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
    /// Git compliance report definition.
    /// </summary>
    public class GitRepositoryReport : CsvReportDefinition, IReport
    {
        /// <summary>
        /// Gets data-options in use with the git report.
        /// </summary>
        public DataOptions DataOptions => DataOptions.Git | DataOptions.GitPolicies;

        /// <summary>
        /// Gets title of the git report.
        /// </summary>
        public string Title => $"GitReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

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

            this.CreateHeaders("Collection", "Project", "Repository", "Branch", "Policy", "Enabled", "Enforced", "Minimum Approvers", "CreatorCounts", "Reset on push");

            foreach (var collection in instance.Collections)
            {
                foreach (var project in collection.Projects)
                {
                    foreach (var repository in project.Repositories)
                    {
                        foreach (var policy in repository.Policies)
                        {
                            foreach (var scope in policy.Settings.Scope)
                            {
                                this.AddLine(
                                        collection.Name,
                                        project.Name,
                                        repository.Name,
                                        scope.RefName,
                                        policy.PolicyType.DisplayName,
                                        policy.IsEnabled,
                                        policy.IsBlocking,
                                        this.SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.MinimumApproverCount),
                                        this.SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.CreatorVoteCounts),
                                        this.SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.ResetOnSourcePush));
                            }
                        }
                    }
                }
            }

            return this.GetReport();
        }

        private string SettingsValue(AzureDevOpsPolicy policy, string desiredType, object value)
        {
            return policy.PolicyType.Id.ToString() == desiredType ? value.ToString() : string.Empty;
        }
    }
}
