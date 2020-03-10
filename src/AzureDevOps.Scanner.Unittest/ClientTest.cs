// -----------------------------------------------------------------------
// <copyright file="ClientTest.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Scanner.Unittest
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using AzureDevOps.Model;
    using FluentAssertions;
    using Moq;
    using Xunit;

    /// <summary>
    /// Contains the actual tests being run against the client.
    /// </summary>
    public partial class ClientTest : IDisposable
    {
        private readonly Mock<MockHttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;

        public ClientTest()
        {
            this.mockHttpMessageHandler = new Mock<MockHttpMessageHandler> { CallBase = true };
            this.httpClient = new HttpClient(this.mockHttpMessageHandler.Object);
        }

        [Fact]
        public async Task ScanAsync_WhenNoProjects_ShouldReturnProperInstance()
        {
            // Arrange
            this.mockHttpMessageHandler.Setup(
                mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/_apis/projects")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenMultipleCollectionsOnPrem_ShouldReturnProperInstance()
        {
            // Arrange
            var expectedCollection1 = "testcol1";
            var expectedCollection2 = "testcol2";
            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{ExpectedUrl}/{expectedCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{ExpectedUrl}/{expectedCollection2}/_apis/projects")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(2);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenAzureDevOpsUrl_OkOnSingleCollection()
        {
            // Arrange
            var azureUrl = "https://dev.azure.com";
            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{azureUrl}/{ExpectedCollection}/_apis/projects")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(azureUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public void ScanAsync_WhenAzureDevOpsUrl_FailsOnMultipleCollection()
        {
            // Arrange
            var azureUrl = "https://dev.azure.com";
            var expectedCollection1 = "testcol1";
            var expectedCollection2 = "testcol2";
            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{azureUrl}/{expectedCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{azureUrl}/{expectedCollection2}/_apis/projects")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, new Uri(azureUrl)).ConfigureAwait(false));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task ScanAsync_WhenNoBuilds_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.expectedProject.Builds = Array.Empty<AzureDevOpsBuild>();

            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/build/builds")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsNoArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneBuild();

            this.expectedProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsWithArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneBuild();
            this.HttpMockOneArtifact();

            this.expectedBuild.Artifacts = new HashSet<AzureDevOpsBuildArtifact> { this.expectedArtifact };
            this.expectedProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build | DataOptions.BuildArtifacts, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenNoReleases_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();

            this.expectedProject.Releases = Array.Empty<AzureDevOpsRelease>();

            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/release/releases")))
                .Returns(EmptyResponse);
            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRelease_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneRelease();

            this.expectedProject.Releases = new HashSet<AzureDevOpsRelease> { this.expectedRelease };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenReleaseDetails_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneRelease();
            this.HttpMockOneReleaseDetails();

            this.expectedProject.Releases = new HashSet<AzureDevOpsRelease> { this.expectedDetailRelease };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release | DataOptions.ReleaseDetails, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenNoRepositories_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();

            this.expectedProject.Repositories = Array.Empty<AzureDevOpsRepository>();

            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/git/repositories?api-version=5.0")))
                .Returns(EmptyResponse);

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRepositories_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneRepository();

            this.expectedProject.Repositories = new HashSet<AzureDevOpsRepository> { this.expectedRepository };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRepositoriesWithMinReviewPolicy_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneRepository();
            this.HttpMockOneRolicy();

            this.expectedRepository.Policies = new HashSet<AzureDevOpsPolicy> { this.expectedPolicy };
            this.expectedProject.Repositories = new HashSet<AzureDevOpsRepository> { this.expectedRepository };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git | DataOptions.GitPolicies, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)).ConfigureAwait(false);

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.httpClient != null)
                {
                    this.httpClient.Dispose();
                    this.httpClient = null;
                }
            }
        }
    }
}
