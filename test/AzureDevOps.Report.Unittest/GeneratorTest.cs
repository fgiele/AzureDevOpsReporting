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
        public void CreateReportsAsync_WhenReportsNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(null, new AzureDevOpsInstance(), "testVal").ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void CreateReportsAsync_WhenAzDOInstanceNullInstance_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), null, "testVal").ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void CreateReportsAsync_WhenFolderNotExist_ThrowsException()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), new AzureDevOpsInstance(), "testVal").ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<System.IO.DirectoryNotFoundException>();
        }

        [Fact]
        public void CreateReportsAsync_WhenCorrectEmpty_ExitsCorrect()
        {
            // Arrange
            var systemUnderTest = new Generator();

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport>(), new AzureDevOpsInstance(), ".").ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeNull();
        }

        [Fact]
        public void CreateReportsAsync_WhenCorrect_GeneratesFile()
        {
            // Arrange
            var systemUnderTest = new Generator();
            var testReport = new ScanAllReport();

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.CreateReportsAsync(new HashSet<IReport> { testReport }, new AzureDevOpsInstance(), ".").ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeNull();
            System.IO.File.Exists(testReport.Title).Should().BeTrue();

            System.IO.File.Delete(testReport.Title);
        }
    }
}
