using AzureDevOps.Model;
using System;
using System.Linq;
using System.Text;

namespace AzureDevOps.Report
{
    public class ReleaseReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => DataOptions.Release | DataOptions.ReleaseDetails;

        public string Title => $"ReleaseReport-{DateTime.Now:yyyyMMdd-HHmmss}.csv";

        public string Generate(AzureDevOpsInstance instance)
        {
            CreateHeaders("Collection", "Project", "Release name", "R. Status", "Environment", "E. Status", "Attempt", "Auto approve", "Approvers");

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
                                        release.Status,
                                        environment.Name,
                                        environment.Status,
                                        preDeployApproval.Attempt,
                                        preDeployApproval.IsAutomated,
                                        string.Join("&", environment.PreApprovalsSnapshot.Approvals.Select(app => app.Approver?.DisplayName))
                                        );

                            }
                        }
                    }
                }
            }
            return GetReport();
        }
    }
}
