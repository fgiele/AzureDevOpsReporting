// -----------------------------------------------------------------------
// <copyright file="ReleaseDefinitionReportTest.cs" company="Freek Giele">
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
    using System.Threading.Tasks;
    using AzureDevOps.Model;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// Contains the actual tests being run against the release report.
    /// </summary>
    public class ReleaseDefinitionReportTest
    {
        [Fact]
        public void DataOptions_WhenCalled_ShouldRequestBuildAndArtifacts()
        {
            // Arrange
            var expected = DataOptions.ReleaseDefinitions;
            var systemUnderTest = new ReleaseDefinitionReport();

            // Act
            var actual = systemUnderTest.DataOptions;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Title_WhenCalled_ShouldReturnUniqueString()
        {
            // Arrange
            var systemUnderTest = new ReleaseDefinitionReport();

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
            var systemUnderTest = new ReleaseDefinitionReport();

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
            var testDate = DateTime.Now;
            var expected = $"SEP=;{Environment.NewLine}Collection;Project;Name;Environment;CD;PreviousEnvironment;PreApproval;PreApprover(s);PostApproval;PostApprover(s);Test* tasks;Link;{Environment.NewLine}testValue;testValue;testValue;testValue;False;;False;;False;;;https://www.example.com/;{Environment.NewLine}testValue;testValue;testValue;testValue;True;;True;testIdentity;True;testIdentity;testValue;https://www.example.com/;{Environment.NewLine}testValue;testValue;testValue;testValue;True;testValue;True;testIdentity;True;testIdentity;testValue;https://www.example.com/;{Environment.NewLine}";
            var testString = "testValue";
            var testInt = 1;
            var testIdentity = new AzureDevOpsIdentity { DisplayName = "testIdentity" };
            var testUri = new Uri("https://www.example.com/");
            var testProject = new AzureDevOpsProject
            {
                Name = testString,
                ReleaseDefinitions = new HashSet<AzureDevOpsReleaseDefinition>
                {
                    new AzureDevOpsReleaseDefinition
                    {
                        Name = testString,
                        Environments = new HashSet<AzureDevOpsReleaseDefinitionEnvironment>
                        {
                            new AzureDevOpsReleaseDefinitionEnvironment
                            {
                                Name = testString,
                                Conditions = new HashSet<AzureDevOpsCondition>(),
                                PreDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>(),
                                },
                                PostDeployApprovals = new AzureDevOpsReleaseDefinitionApproval()
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = true,
                                        },
                                    },
                                },
                            },
                        },
                        Url = testUri,
                    },
                    new AzureDevOpsReleaseDefinition
                    {
                        Name = testString,
                        Environments = new HashSet<AzureDevOpsReleaseDefinitionEnvironment>
                        {
                            new AzureDevOpsReleaseDefinitionEnvironment
                            {
                                Name = testString,
                                Conditions = new HashSet<AzureDevOpsCondition>
                                {
                                    new AzureDevOpsCondition
                                    {
                                        ConditionType = AzureDevOpsConditionType.Artifact,
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
                                        },
                                    },
                                },
                                PreDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = testIdentity,
                                        },
                                    },
                                },
                                PostDeployApprovals = new AzureDevOpsReleaseDefinitionApproval()
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = testIdentity,
                                        },
                                    },
                                },
                            },
                        },
                        Url = testUri,
                    },
                    new AzureDevOpsReleaseDefinition
                    {
                        Name = testString,
                        Environments = new HashSet<AzureDevOpsReleaseDefinitionEnvironment>
                        {
                            new AzureDevOpsReleaseDefinitionEnvironment
                            {
                                Name = testString,
                                Conditions = new HashSet<AzureDevOpsCondition>
                                {
                                    new AzureDevOpsCondition
                                    {
                                        ConditionType = AzureDevOpsConditionType.EnvironmentState,
                                        Name = testString,
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
                                        },
                                    },
                                },
                                PreDeployApprovals = new AzureDevOpsReleaseDefinitionApproval
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = testIdentity,
                                        },
                                    },
                                },
                                PostDeployApprovals = new AzureDevOpsReleaseDefinitionApproval()
                                {
                                    Approvals = new HashSet<AzureDevOpsReleaseDefinitionApprovalStep>
                                    {
                                        new AzureDevOpsReleaseDefinitionApprovalStep
                                        {
                                            IsAutomated = false,
                                            Approver = testIdentity,
                                        },
                                    },
                                },
                            },
                        },
                        Url = testUri,
                    },
                },
            };
            var testCollection = new AzureDevOpsCollection { Name = testString };
            testCollection.Projects.Add(testProject);
            var testAzureDevOpsInstance = new AzureDevOpsInstance();
            testAzureDevOpsInstance.Collections.Add(testCollection);

            var systemUnderTest = new ReleaseDefinitionReport();

            // Act
            var actual = systemUnderTest.Generate(testAzureDevOpsInstance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
