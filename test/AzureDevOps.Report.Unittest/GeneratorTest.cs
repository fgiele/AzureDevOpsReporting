// -----------------------------------------------------------------------
// <copyright file="GeneratorTest.cs" company="Freek Giele">
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
    /// Contains the actual tests being run against the report generator.
    /// </summary>
    public class GeneratorTest
    {
        [Fact]
        public async Task CreateReportsAsync_WhenReportsNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(null, new AzureDevOpsInstance(), "testVal"));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateReportsAsync_WhenAzDOInstanceNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), null, "testVal"));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateReportsAsync_WhenFolderNotExist_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), new AzureDevOpsInstance(), "testVal"));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<System.IO.DirectoryNotFoundException>();
        }

        [Fact]
        public async Task CreateReportsAsync_WhenCorrectEmpty_ExitsCorrect()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), new AzureDevOpsInstance(), "."));

            // Assert
            actualException.Should().BeNull();
        }

        [Fact]
        public async Task CreateReportsAsync_WhenCorrect_GeneratesFile()
        {
            // Arrange
            var systemUnderTest = new Generator();
            var testReport = new ScanAllReport();

            // Act
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport> { testReport }, new AzureDevOpsInstance(), "."));

            // Assert
            actualException.Should().BeNull();
            System.IO.File.Exists(testReport.Title).Should().BeTrue();

            System.IO.File.Delete(testReport.Title);
        }
    }
}
