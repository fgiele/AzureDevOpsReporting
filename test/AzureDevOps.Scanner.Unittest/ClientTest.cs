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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(azureUrl));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenAzureDevOpsUrl_FailsOnMultipleCollection()
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
            var actualException = await Record.ExceptionAsync(async () => await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, new Uri(azureUrl)));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Should().BeOfType<InvalidOperationException>();
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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            this.HttpMockMultipleProject();
            this.HttpMockOneBuild();

            this.expectedProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(2);
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsWithArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockOneBuild();
            this.HttpMockOneArtifact();
            this.HttpMockOneTimeline();

            this.expectedBuild.Timeline = this.expectedTimeline;
            this.expectedBuild.Artifacts = new HashSet<AzureDevOpsBuildArtifact> { this.expectedArtifact };
            this.expectedProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build | DataOptions.BuildArtifacts, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release | DataOptions.ReleaseDetails, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenReleaseDefinition_ShouldReturnProperInstance()
        {
            // Arrange
            this.HttpMockOneProject();
            this.HttpMockListReleaseDefinitions();
            this.HttpMockOneReleaseDefinition();

            this.expectedProject.ReleaseDefinitions = new HashSet<AzureDevOpsReleaseDefinition> { this.expectedReleaseDefinition };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.ReleaseDefinitions, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

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
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git | DataOptions.GitPolicies, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRestAPINoContent_ShouldShowEmpty()
        {
            // Arrange
            this.HttpMockNoProject();

            var systemUnderTest = new Client(this.httpClient);

            // Act
            // Expect ArgumentNullException, since NoContent returns a typed null object which is then parsed through
            var actual = await Assert.ThrowsAsync<ArgumentNullException>(async () => await systemUnderTest.ScanAsync(DataOptions.Git | DataOptions.GitPolicies, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public async Task ScanAsync_WhenRestAPIError_ShouldThrowAfterRetries()
        {
            // Arrange
            this.HttpMockFailProject();

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await Assert.ThrowsAsync<HttpRequestException>(async () => await systemUnderTest.ScanAsync(DataOptions.Git | DataOptions.GitPolicies, new string[] { ExpectedCollection }, new Uri(ExpectedUrl)));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<HttpRequestException>();
        }

        [Fact]
        public async Task ScanAsync_WhenContinuationToken_GetsFullList()
        {
            // Arrange
            this.HttpMockMultipleProject();
            this.HttpMockOneBuild();
            this.expectedProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };
            this.secondProject.Builds = new HashSet<AzureDevOpsBuild> { this.expectedBuild };

            var systemUnderTest = new Client(this.httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { ExpectedCollection }, new Uri(ExpectedUrl));

            // Assert
            this.mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(2);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.expectedProject);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(this.secondProject);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.httpClient != null)
            {
                this.httpClient.Dispose();
                this.httpClient = null;
            }
        }
    }
}
