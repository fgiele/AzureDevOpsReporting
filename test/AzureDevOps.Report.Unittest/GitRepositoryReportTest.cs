// -----------------------------------------------------------------------
// <copyright file="GitRepositoryReportTest.cs" company="Freek Giele">
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
    /// Contains the actual tests being run against the git repository report.
    /// </summary>
    public class GitRepositoryReportTest
    {
        [Fact]
        public void DataOptions_WhenCalled_ShouldRequestBuildAndArtifacts()
        {
            // Arrange
            var expected = DataOptions.Git | DataOptions.GitPolicies;
            var systemUnderTest = new GitRepositoryReport();

            // Act
            var actual = systemUnderTest.DataOptions;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Title_WhenCalled_ShouldReturnUniqueString()
        {
            // Arrange
            var systemUnderTest = new GitRepositoryReport();

            // Act
            var actual = systemUnderTest.Title;
            await Task.Delay(2000); // Wait 2 seconds
            var secondTitle = systemUnderTest.Title;

            // Assert
            actual.Should().NotBeNullOrEmpty();
            actual.Should().NotBe(secondTitle);
        }

        [Fact]
        public void Generate_WhenNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new GitRepositoryReport();

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
            var expected = $"SEP=;{Environment.NewLine}Collection;Project;Repository;Branch;Policy;Enabled;Enforced;Minimum Approvers;CreatorCounts;Reset on push;{Environment.NewLine}testValue;testValue;testValue;testValue;testValue;True;True;1;True;True;{Environment.NewLine}testValue;testValue;testValue;testValue;testValue;True;True;;;;{Environment.NewLine}";
            var testString = "testValue";
            var testInt = 1;
            var testBool = true;
            var testProject = new AzureDevOpsProject
            {
                Name = testString,
                Repositories = new HashSet<AzureDevOpsRepository>
                {
                    new AzureDevOpsRepository
                    {
                        Name = testString,
                        Policies = new HashSet<AzureDevOpsPolicy>
                        {
                            new AzureDevOpsPolicy
                            {
                                IsBlocking = testBool,
                                IsEnabled = testBool,
                                PolicyType = new AzureDevOpsPolicyType
                                {
                                    Id = new Guid(PolicyType.MinimumNumberOfReviewers),
                                    DisplayName = testString,
                                },
                                Settings = new AzureDevOpsPolicySettings
                                {
                                    MinimumApproverCount = testInt,
                                    CreatorVoteCounts = testBool,
                                    ResetOnSourcePush = testBool,
                                    Scope = new HashSet<AzureDevOpsPolicyScope>
                                    {
                                        new AzureDevOpsPolicyScope
                                        {
                                            RefName = testString,
                                        },
                                    },
                                },
                            },
                            new AzureDevOpsPolicy
                            {
                                IsBlocking = testBool,
                                IsEnabled = testBool,
                                PolicyType = new AzureDevOpsPolicyType
                                {
                                    Id = new Guid(PolicyType.ActiveComments),
                                    DisplayName = testString,
                                },
                                Settings = new AzureDevOpsPolicySettings
                                {
                                    Scope = new HashSet<AzureDevOpsPolicyScope>
                                    {
                                        new AzureDevOpsPolicyScope
                                        {
                                            RefName = testString,
                                        },
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

            var systemUnderTest = new GitRepositoryReport();

            // Act
            var actual = systemUnderTest.Generate(testAzureDevOpsInstance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
