// -----------------------------------------------------------------------
// <copyright file="ReleaseReportTest.cs" company="Freek Giele">
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
    public class ReleaseReportTest
    {
        [Fact]
        public void DataOptions_WhenCalled_ShouldRequestBuildAndArtifacts()
        {
            // Arrange
            var expected = DataOptions.Release | DataOptions.ReleaseDetails;
            var systemUnderTest = new ReleaseReport();

            // Act
            var actual = systemUnderTest.DataOptions;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Title_WhenCalled_ShouldReturnUniqueString()
        {
            // Arrange
            var systemUnderTest = new ReleaseReport();

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
            var systemUnderTest = new ReleaseReport();

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
            var expected = $"SEP=;{Environment.NewLine}Collection;Project;Release name;Release date;R. Status;Environment;E. Status;Attempt;Attempt date;Auto approve;Required approval;Approval given by;ReplacedToken?;Nr. of Artifacts;Artifact - version [branch];{Environment.NewLine}testValue;testValue;testValue;{testDate};testValue;testValue;testValue;1;{testDate};False;testIdentity;testIdentity;True;0;;{Environment.NewLine}testValue;testValue;testValue;{testDate};testValue;testValue;testValue;1;{testDate};True;;;False;1;'testValue - testValue [testValue]';{Environment.NewLine}";
            var testSearchedTaskId = new Guid("a8515ec8-7254-4ffd-912c-86772e2b5962");
            var testString = "testValue";
            var testGuid = Guid.NewGuid();
            var testInt = 1;
            var testIdentity = new AzureDevOpsIdentity { DisplayName = "testIdentity" };
            var testProject = new AzureDevOpsProject
            {
                Name = testString,
                Releases = new HashSet<AzureDevOpsRelease>
                {
                    new AzureDevOpsRelease
                    {
                        Name = testString,
                        CreatedOn = testDate,
                        Status = testString,
                        Environments = new HashSet<AzureDevOpsEnvironment>
                        {
                            new AzureDevOpsEnvironment
                            {
                                Name = testString,
                                Status = testString,
                                DeploySteps = new HashSet<AzureDevOpsDeployStep>
                                {
                                    new AzureDevOpsDeployStep
                                    {
                                        Attempt = 1,
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
                                                                 Task = new AzureDevOpsTask
                                                                 {
                                                                     Id = testSearchedTaskId,
                                                                 },
                                                            },
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                                PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                                {
                                    new AzureDevOpsDeployApproval
                                    {
                                        Attempt = testInt,
                                        CreatedOn = testDate,
                                        IsAutomated = false,
                                        Approver = testIdentity,
                                        ApprovedBy = testIdentity,
                                    },
                                },
                            },
                        },
                        Artifacts = new HashSet<AzureDevOpsReleaseArtifact>(),
                    },
                    new AzureDevOpsRelease
                    {
                        Name = testString,
                        CreatedOn = testDate,
                        Status = testString,
                        Environments = new HashSet<AzureDevOpsEnvironment>
                        {
                            new AzureDevOpsEnvironment
                            {
                                Name = testString,
                                Status = testString,
                                DeploySteps = new HashSet<AzureDevOpsDeployStep>
                                {
                                    new AzureDevOpsDeployStep
                                    {
                                        Attempt = 1,
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
                                                                 Task = new AzureDevOpsTask
                                                                 {
                                                                     Id = testGuid,
                                                                 },
                                                            },
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                                PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                                {
                                    new AzureDevOpsDeployApproval
                                    {
                                        Attempt = testInt,
                                        CreatedOn = testDate,
                                        IsAutomated = true,
                                    },
                                },
                            },
                        },
                        Artifacts = new HashSet<AzureDevOpsReleaseArtifact>
                        {
                            new AzureDevOpsReleaseArtifact
                            {
                                DefinitionReference = new AzureDevOpsDefinitionReference
                                {
                                    Definition = new AzureDevOpsReferenceField
                                    {
                                        Name = testString,
                                    },
                                    Version = new AzureDevOpsReferenceField
                                    {
                                        Name = testString,
                                    },
                                    Branch = new AzureDevOpsReferenceField
                                    {
                                        Name = testString,
                                    },
                                },
                            },
                        },
                    },
                },
            };
            var testCollection = new AzureDevOpsCollection { Name = testString };
            testCollection.Projects.Add(testProject);
            var testAzureDevOpsInstance = new AzureDevOpsInstance();
            testAzureDevOpsInstance.Collections.Add(testCollection);

            var systemUnderTest = new ReleaseReport();

            // Act
            var actual = systemUnderTest.Generate(testAzureDevOpsInstance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
