﻿// -----------------------------------------------------------------------
// <copyright file="BuildDurationReportTest.cs" company="Freek Giele">
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
    /// Contains the actual tests being run against the build duration report.
    /// </summary>
    public class BuildDurationReportTest
    {
        [Fact]
        public void DataOptions_WhenCalled_ShouldRequestBuild()
        {
            // Arrange
            var expected = DataOptions.Build;
            var systemUnderTest = new BuildDurationReport();

            // Act
            var actual = systemUnderTest.DataOptions;

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public async Task Title_WhenCalled_ShouldReturnUniqueString()
        {
            // Arrange
            var systemUnderTest = new BuildDurationReport();

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
            var systemUnderTest = new BuildDurationReport();

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
            var expected = $"SEP=;{Environment.NewLine}Collection;Project;Repistory;Branch;Build nr;Build status;Build result;Build queued;Build start;Build end;{Environment.NewLine}testValue;testValue;testValue;testValue;testValue;testValue;testValue;testValue;testValue;testValue;{Environment.NewLine}";
            var testString = "testValue";
            var testProject = new AzureDevOpsProject
            {
                Name = testString,
                Builds = new HashSet<AzureDevOpsBuild>
                {
                    new AzureDevOpsBuild
                    {
                        SourceBranch = testString,
                        BuildNumber = testString,
                        Status = testString,
                        Result = testString,
                        Repository = new AzureDevOpsSourceRepository
                        {
                            Name = testString,
                        },
                        QueueTime = testString,
                        StartTime = testString,
                        FinishTime = testString,
                    },
                },
            };
            var testCollection = new AzureDevOpsCollection { Name = testString };
            testCollection.Projects.Add(testProject);
            var testAzureDevOpsInstance = new AzureDevOpsInstance();
            testAzureDevOpsInstance.Collections.Add(testCollection);

            var systemUnderTest = new BuildDurationReport();

            // Act
            var actual = systemUnderTest.Generate(testAzureDevOpsInstance);

            // Assert
            actual.Should().NotBeNull();
            actual.Should().Be(expected);
        }
    }
}
