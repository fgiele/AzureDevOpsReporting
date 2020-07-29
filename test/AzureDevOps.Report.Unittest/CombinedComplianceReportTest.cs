// -----------------------------------------------------------------------
// <copyright file="CombinedComplianceReportTest.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report.Unittest
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using AzureDevOps.Model;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel;
    using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
    using Xunit;

    /// <summary>
    /// Contains the actual tests being run against the combined compliance report.
    /// </summary>
    public class CombinedComplianceReportTest
    {
        [Fact]
        public void DataOptions_WhenCalled_ShouldRequestBuildAndArtifacts()
        {
            // Arrange
            var expected = DataOptions.Build | DataOptions.BuildArtifacts | DataOptions.Git | DataOptions.GitPolicies | DataOptions.Release | DataOptions.ReleaseDetails | DataOptions.ReleaseDefinitions;
            var systemUnderTest = new CombinedComplianceReport();

            // Act
            var actual = systemUnderTest.DataOptions;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Title_WhenCalled_ShouldReturnUniqueString()
        {
            // Arrange
            var systemUnderTest = new CombinedComplianceReport();

            // Act
            var actual = systemUnderTest.Title;
            await Task.Delay(2000).ConfigureAwait(false); // Wait 2 seconds
            var secondTitle = systemUnderTest.Title;

            // Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().NotBe(secondTitle);
        }

        [Fact]
        public void Generate_WhenNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new CombinedComplianceReport();

            // Act
            var actualException = Record.Exception(() => systemUnderTest.Generate(null));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Generate_WithInstance_GeneratesReport()
        {
            // Arrange
            var expected = this.GetExpectedReport();
            var testUri = new Uri("https://www.example.com/");
            var testString = "testValue";
            var testInt = 1;
            var testGuid = Guid.NewGuid();
            var testResult = AzureDevOpsTaskResult.Succeeded;
            var testState = AzureDevOpsTimelineRecordState.Completed;
            var testProject = new AzureDevOpsProject
            {
                Name = testString,
                Builds = new HashSet<AzureDevOpsBuild>
                {
                    new AzureDevOpsBuild
                    {
                        Reason = AzureDevOpsBuildReason.Manual,
                        BuildNumber = testString,
                        Links = new AzureDevOpsBuildLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                    },
                    new AzureDevOpsBuild
                    {
                        Reason = AzureDevOpsBuildReason.Schedule,
                        BuildNumber = testString,
                        Timeline = new AzureDevOpsBuildTimeline
                        {
                            Records = new HashSet<AzureDevOpsTimelineRecord>
                            {
                                new AzureDevOpsTimelineRecord
                                {
                                    Name = "nope",
                                    Order = testInt,
                                    Result = testResult,
                                    State = testState,
                                    Task = new AzureDevOpsTask
                                    {
                                        Id = testGuid,
                                        Name = testString,
                                        Version = testString,
                                    },
                                    Type = testString,
                                },
                            },
                        },
                        Links = new AzureDevOpsBuildLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                    },
                    new AzureDevOpsBuild
                    {
                        Reason = AzureDevOpsBuildReason.BatchedCI,
                        BuildNumber = testString,
                        Timeline = new AzureDevOpsBuildTimeline
                        {
                            Records = new HashSet<AzureDevOpsTimelineRecord>
                            {
                                new AzureDevOpsTimelineRecord
                                {
                                    Name = "Sonarstep",
                                    Order = 1,
                                    Result = AzureDevOpsTaskResult.Succeeded,
                                    State = AzureDevOpsTimelineRecordState.Completed,
                                    Task = new AzureDevOpsTask
                                    {
                                        Id = testGuid,
                                        Name = "SonarQubePublish",
                                        Version = testString,
                                    },
                                    Type = testString,
                                },
                                new AzureDevOpsTimelineRecord
                                {
                                    Name = "execTest",
                                    Order = 2,
                                    Result = AzureDevOpsTaskResult.Succeeded,
                                    State = AzureDevOpsTimelineRecordState.Completed,
                                    Task = new AzureDevOpsTask
                                    {
                                        Id = testGuid,
                                        Name = testString,
                                        Version = testString,
                                    },
                                    Type = testString,
                                },
                            },
                        },
                        Links = new AzureDevOpsBuildLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                    },
                },
                Repositories = new HashSet<AzureDevOpsRepository>
                {
                    new AzureDevOpsRepository
                    {
                        Name = testString,
                        WebUrl = testUri,
                        Policies = new HashSet<AzureDevOpsPolicy>(),
                    },
                    new AzureDevOpsRepository
                    {
                        Name = testString,
                        WebUrl = testUri,
                        Policies = new HashSet<AzureDevOpsPolicy>
                        {
                            new AzureDevOpsPolicy
                            {
                                IsBlocking = true,
                                IsEnabled = true,
                                PolicyType = new AzureDevOpsPolicyType
                                {
                                    Id = new Guid(PolicyType.MinimumNumberOfReviewers),
                                },
                                Settings = new AzureDevOpsPolicySettings
                                {
                                    MinimumApproverCount = 2,
                                    ResetOnSourcePush = true,
                                    Scope = new HashSet<AzureDevOpsPolicyScope>
                                    {
                                        new AzureDevOpsPolicyScope
                                        {
                                            RefName = "refs/heads/master",
                                        },
                                    },
                                },
                            },
                            new AzureDevOpsPolicy
                            {
                                IsBlocking = true,
                                IsEnabled = true,
                                PolicyType = new AzureDevOpsPolicyType
                                {
                                    Id = new Guid(PolicyType.ActiveComments),
                                },
                                Settings = new AzureDevOpsPolicySettings
                                {
                                    Scope = new HashSet<AzureDevOpsPolicyScope>
                                    {
                                        new AzureDevOpsPolicyScope
                                        {
                                            RefName = "refs/heads/master",
                                        },
                                    },
                                },
                            },
                            new AzureDevOpsPolicy
                            {
                                IsBlocking = true,
                                IsEnabled = true,
                                PolicyType = new AzureDevOpsPolicyType
                                {
                                    Id = new Guid(PolicyType.SuccessfulBuild),
                                },
                                Settings = new AzureDevOpsPolicySettings
                                {
                                    Scope = new HashSet<AzureDevOpsPolicyScope>
                                    {
                                        new AzureDevOpsPolicyScope
                                        {
                                            RefName = "refs/heads/master",
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                ReleaseDefinitions = new HashSet<AzureDevOpsReleaseDefinition>
                {
                    new AzureDevOpsReleaseDefinition
                    {
                        Name = testString,
                        Url = testUri,
                        Links = new AzureDevOpsReleaseLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                        Triggers = new HashSet<AzureDevOpsTrigger>
                        {
                            new AzureDevOpsTrigger
                            {
                                TriggerType = testString,
                                TriggerConditions = new HashSet<AzureDevOpsTriggerCondition>
                                {
                                    new AzureDevOpsTriggerCondition
                                    {
                                        SourceBranch = testString,
                                    },
                                },
                            },
                        },
                        Environments = new HashSet<AzureDevOpsReleaseDefinitionEnvironment>
                        {
                            new AzureDevOpsReleaseDefinitionEnvironment
                            {
                                Name = testString,
                                PreDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = true,
                                        },
                                    },
                                },
                                PostDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = true,
                                        },
                                    },
                                },
                                Conditions = new HashSet<AzureDevOpsCondition>
                                {
                                    new AzureDevOpsCondition
                                    {
                                        ConditionType = AzureDevOpsConditionType.Event,
                                        Name = testString,
                                        Value = testString,
                                    },
                                },
                            },
                            new AzureDevOpsReleaseDefinitionEnvironment
                            {
                                Name = testString,
                                PreDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = new AzureDevOpsIdentity
                                            {
                                                DisplayName = testString,
                                            },
                                        },
                                    },
                                },
                                PostDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = new AzureDevOpsIdentity
                                            {
                                                DisplayName = testString,
                                            },
                                        },
                                    },
                                },
                                Conditions = new HashSet<AzureDevOpsCondition>
                                {
                                    new AzureDevOpsCondition
                                    {
                                        ConditionType = AzureDevOpsConditionType.EnvironmentState,
                                        Name = testString,
                                        Value = testString,
                                    },
                                },
                                DeployPhases = new HashSet<AzureDevOpsDeployPhase>
                                {
                                    new AzureDevOpsDeployPhase
                                    {
                                        WorkflowTasks = new HashSet<AzureDevOpsWorkflowTask>
                                        {
                                            new AzureDevOpsWorkflowTask
                                            {
                                                Name = testString,
                                            },
                                            new AzureDevOpsWorkflowTask
                                            {
                                                Name = testString,
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                Releases = new HashSet<AzureDevOpsRelease>
                {
                    new AzureDevOpsRelease
                    {
                        Name = testString,
                        Environments = new HashSet<AzureDevOpsEnvironment>
                        {
                            new AzureDevOpsEnvironment
                            {
                                Name = testString,
                                TriggerReason = "Manual",
                                PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                                {
                                    new AzureDevOpsDeployApproval
                                    {
                                        Attempt = testInt,
                                        IsAutomated = true,
                                    },
                                },
                                DeploySteps = new HashSet<AzureDevOpsDeployStep>
                                {
                                    new AzureDevOpsDeployStep
                                    {
                                        Attempt = testInt,
                                    },
                                },
                            },
                        },
                        Links = new AzureDevOpsReleaseLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                    },
                    new AzureDevOpsRelease
                    {
                        Name = testString,
                        Environments = new HashSet<AzureDevOpsEnvironment>
                        {
                            new AzureDevOpsEnvironment
                            {
                                Name = testString,
                                TriggerReason = testString,
                                PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                                {
                                    new AzureDevOpsDeployApproval
                                    {
                                        Attempt = testInt,
                                        IsAutomated = false,
                                        ApprovedBy = new AzureDevOpsIdentity(),
                                    },
                                },
                                DeploySteps = new HashSet<AzureDevOpsDeployStep>
                                {
                                    new AzureDevOpsDeployStep
                                    {
                                        Attempt = testInt,
                                        ReleaseDeployPhases = new HashSet<AzureDevOpsReleaseDeployPhase>
                                        {
                                            new AzureDevOpsReleaseDeployPhase
                                            {
                                                DeploymentJobs = new HashSet<AzureDevOpsDeploymentJob>
                                                {
                                                    new AzureDevOpsDeploymentJob
                                                    {
                                                        Tasks = new HashSet<AzureDevOpsDeploymentTask>
                                                        {
                                                            new AzureDevOpsDeploymentTask
                                                            {
                                                                Name = "execTest",
                                                            },
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                        Links = new AzureDevOpsReleaseLinks
                        {
                            Web = new AzureDevOpsLink
                            {
                                Href = testUri,
                            },
                        },
                    },
                },
            };
            var testCollection = new AzureDevOpsCollection { Name = testString };
            testCollection.Projects.Add(testProject);
            var testAzureDevOpsInstance = new AzureDevOpsInstance();
            testAzureDevOpsInstance.Collections.Add(testCollection);

            var systemUnderTest = new CombinedComplianceReport();

            // Act
            var actual = systemUnderTest.Generate(testAzureDevOpsInstance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }

        private string GetExpectedReport()
        {
            var expectedBuilder = new StringBuilder();

            expectedBuilder.AppendLine("# testValue/testValue");
            expectedBuilder.AppendLine("## Git repositories");
            expectedBuilder.AppendLine("|Name|Protected master|>1 approvers|Reset on push|Comment resolution|Build check|Remarks|Link|");
            expectedBuilder.AppendLine("|---|---|---|---|---|---|---|---|");
            expectedBuilder.AppendLine("|testValue|(x)|(x)|(x)|(x)|(x)||https://www.example.com/|");
            expectedBuilder.AppendLine("|testValue|(/)|(/)|(/)|(/)|(/)||https://www.example.com/|");
            expectedBuilder.AppendLine("## Executed builds");
            expectedBuilder.AppendLine("|Name|CI|SonarQube|Test|Remarks|Link|");
            expectedBuilder.AppendLine("|---|---|---|---|---|---|");
            expectedBuilder.AppendLine("|testValue|(x)|(x)|(x)|No timeline found!|https://www.example.com/|");
            expectedBuilder.AppendLine("|testValue|(x)|(x)|(x)||https://www.example.com/|");
            expectedBuilder.AppendLine("|testValue|(/)|(/)|(/)|Tests executed: execTest|https://www.example.com/|");
            expectedBuilder.AppendLine("## Executed releases");
            expectedBuilder.AppendLine("|Name|CD|Environment|Attempt|Approval|Tests|Remarks|Link|");
            expectedBuilder.AppendLine("|---|---|---|---|---|---|---|---|");
            expectedBuilder.AppendLine("|testValue|(x)|testValue|1|(x)|(x)||https://www.example.com/|");
            expectedBuilder.AppendLine("|testValue|(/)|testValue|1|(/)|(/)|Tests executed: execTest|https://www.example.com/|");
            expectedBuilder.AppendLine("## Release definitions");
            expectedBuilder.AppendLine("|Name|Environment|CD|PreviousEnvironment|PreApproval|PreApprover(s)|PostApproval|PostApprover(s)|Test* tasks|Link|");
            expectedBuilder.AppendLine("|---|---|---|---|---|---|---|---|---|---|");
            expectedBuilder.AppendLine("|testValue|testValue|(x)||(x)||(x)|||https://www.example.com/|");
            expectedBuilder.AppendLine("|testValue|testValue|(/)|testValue|(/)|testValue|(/)|testValue|testValue, testValue|https://www.example.com/|");

            return expectedBuilder.ToString();
        }
    }
}
