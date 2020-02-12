using AzureDevOps.Model;
using System;
using System.Linq;

namespace AzureDevOps.Report
{
    public class GitRepositoryReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Git | DataOptions.GitPolicies;

        public string Title => $"GitReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        public string Generate(AzureDevOpsInstance instance)
        {
            CreateHeaders("Collection", "Project", "Repository", "Branch", "Policy", "Enabled", "Enforced", "Minimum Approvers", "CreatorCounts", "Reset on push");

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
                                AddLine(collection.Name,
                                        project.Name,
                                        repository.Name,
                                        scope.RefName,
                                        policy.PolicyType.DisplayName,
                                        policy.IsEnabled,
                                        policy.IsBlocking,
                                        SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.MinimumApproverCount),
                                        SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.CreatorVoteCounts),
                                        SettingsValue(policy, PolicyType.MinimumNumberOfReviewers, policy.Settings.ResetOnSourcePush)
                                        );
                            }
                        }
                    }
                }
            }
            return GetReport();
        }

        private string SettingsValue(AzureDevOpsPolicy policy, PolicyType desiredType, object value)
        {
            return policy.PolicyType.PolicyType == desiredType ? value.ToString() : string.Empty;
        }
    }
}
