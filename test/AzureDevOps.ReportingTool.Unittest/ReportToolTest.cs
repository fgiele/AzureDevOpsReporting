// -----------------------------------------------------------------------
// <copyright file="ReportToolTest.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.ReportingTool.Unittest
{
    using System;
    using System.Collections.Generic;
    using AzureDevOps.Model;
    using AzureDevOps.Report;
    using AzureDevOps.Scanner;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class ReportToolTest
    {
        private const string NoReportsFile = "..\\..\\..\\NoReportsSettings.json";
        private const string AllReportsFile = "..\\..\\..\\AllReportsSettings.json";

        [Fact]
        public void Main_WhenArgumentNull_ThrowsArgumentNullException()
        {
            // Arrange
            var clientMock = new Mock<IClient>();
            var generatorMock = new Mock<IGenerator>();
            var systemUnderTest = new ReportTool(clientMock.Object, generatorMock.Object);

            // Act
            var actualException = Record.Exception(() => systemUnderTest.Main(null));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Main_WhenArgumentEmpty_ThrowsArgumentNullException()
        {
            // Arrange
            var clientMock = new Mock<IClient>();
            var generatorMock = new Mock<IGenerator>();
            var systemUnderTest = new ReportTool(clientMock.Object, generatorMock.Object);

            // Act
            var actualException = Record.Exception(() => systemUnderTest.Main(string.Empty));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public void Main_WhenFileNotFound_ThrowsArgumentException()
        {
            // Arrange
            var testFile = "bogus";
            var clientMock = new Mock<IClient>();
            var generatorMock = new Mock<IGenerator>();
            var systemUnderTest = new ReportTool(clientMock.Object, generatorMock.Object);

            // Act
            var actualException = Record.Exception(() => systemUnderTest.Main(testFile));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<ArgumentException>();
        }

        [Fact]
        public void Main_WhenNoReports_ShouldNotStartProcessing()
        {
            // Arrange
            var clientMock = new Mock<IClient>();
            var generatorMock = new Mock<IGenerator>();
            var systemUnderTest = new ReportTool(clientMock.Object, generatorMock.Object);

            // Act
            systemUnderTest.Main(NoReportsFile);

            // Assert
            clientMock.Verify(cli => cli.ScanAsync(It.IsAny<DataOptions>(), It.IsAny<IEnumerable<string>>(), It.IsAny<Uri>()), Times.Never);
            generatorMock.Verify(gen => gen.CreateReportsAsync(It.IsAny<IEnumerable<IReport>>(), It.IsAny<AzureDevOpsInstance>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Main_WhenReports_ShouldStartProcessing()
        {
            // Arrange
            var testClient = new System.Net.Http.HttpClient();
            var clientMock = new Mock<IClient>();
            var generatorMock = new Mock<IGenerator>();
            clientMock.Setup(cli => cli.RestClient).Returns(testClient);
            var systemUnderTest = new ReportTool(clientMock.Object, generatorMock.Object);

            // Act
            systemUnderTest.Main(AllReportsFile);

            // Assert
            clientMock.Verify(cli => cli.ScanAsync(It.IsAny<DataOptions>(), It.IsAny<IEnumerable<string>>(), It.IsAny<Uri>()), Times.Once);
            generatorMock.Verify(gen => gen.CreateReportsAsync(It.IsAny<IEnumerable<IReport>>(), It.IsAny<AzureDevOpsInstance>(), It.IsAny<string>()), Times.Once);

            testClient.Dispose();
        }
    }
}
