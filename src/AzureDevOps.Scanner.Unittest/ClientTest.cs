using AzureDevOps.Model;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AzureDevOps.Scanner.Unittest
{
    public partial class ClientTest
    {
        private Mock<MockHttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        
        public ClientTest()
        {
            mockHttpMessageHandler = new Mock<MockHttpMessageHandler> { CallBase = true };
            httpClient = new HttpClient(mockHttpMessageHandler.Object);
        }

        [Fact]
        public async Task ScanAsync_WhenNoProjects_ShouldReturnProperInstance()
        {
            // Arrange
            mockHttpMessageHandler.Setup(
                mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenMultipleCollectionsOnPrem_ShouldReturnProperInstance()
        {
            // Arrange
            var testCollection1 = "testcol1";
            var testCollection2 = "testcol2";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{testUrl}/{testCollection2}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection1, testCollection2 }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(2);
            actual.Collections[0].Projects.Should().BeEmpty();
            actual.Collections[1].Projects.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenAzureDevOpsUrl_OkOnSingleCollection()
        {
            // Arrange
            var azureUrl = "https://dev.azure.com";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{azureUrl}/{testCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection }, azureUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().BeEmpty();
        }

        [Fact]
        public void ScanAsync_WhenAzureDevOpsUrl_FailsOnMultipleCollection()
        {
            // Arrange
            var azureUrl = "https://dev.azure.com";
            var testCollection1 = "testcol1";
            var testCollection2 = "testcol2";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{azureUrl}/{testCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{azureUrl}/{testCollection2}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection1, testCollection2 }, azureUrl));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task ScanAsync_WhenNoBuilds_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            var expectedProject = testProject;
            expectedProject.Builds = Array.Empty<AzureDevOpsBuild>();

            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/{testProject.Id}/_apis/build/builds")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
            actual.Collections[0].Projects[0].Builds.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsNoArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneBuild();
            var systemUnderTest = new Client(httpClient);
            var expectedProject = testProject;
            expectedProject.Builds = new HashSet<AzureDevOpsBuild> { testBuild };

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
            actual.Collections[0].Projects[0].Builds.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Builds.First().Should().BeEquivalentTo(testBuild);
            actual.Collections[0].Projects[0].Builds.First().Artifacts.Should().BeNull();
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsWithArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneBuild();
            HttpMockOneArtifact();

            var expectedProject = testProject;
            var expectedBuild = testBuild;
            expectedBuild.Artifacts = new HashSet<AzureDevOpsBuildArtifact> { testArtifact };
            expectedProject.Builds = new HashSet<AzureDevOpsBuild> { expectedBuild }; ;

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build | DataOptions.BuildArtifacts, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
            actual.Collections[0].Projects[0].Builds.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Builds.First().Should().BeEquivalentTo(testBuild);
            actual.Collections[0].Projects[0].Builds.First().Artifacts.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Builds.First().Artifacts.First().Should().BeEquivalentTo(testArtifact);
        }

        [Fact]
        public async Task ScanAsync_WhenNoReleases_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();

            var expectedProject = testProject;
            expectedProject.Releases = Array.Empty<AzureDevOpsRelease>();

            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/{testProject.Id}/_apis/release/releases")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(testProject);
            actual.Collections[0].Projects[0].Releases.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenRelease_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneRelease();

            var expectedProject = testProject;
            expectedProject.Releases = new HashSet<AzureDevOpsRelease> { testRelease };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        private void HttpMockOneProject()
        {
            mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":\"{testProject.Id}\",\"name\":\"{testProject.Name}\",\"description\":\"{testProject.Description}\",\"url\":\"{testProject.Url}\"}}]}}")
                });
        }

        private void HttpMockOneBuild()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/{testProject.Id}/_apis/build/builds")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{testBuild.Id},\"buildNumber\":\"{testBuild.BuildNumber}\",\"status\":\"{testBuild.Status}\",\"result\":\"{testBuild.Result}\",\"url\":\"{testBuild.Url}\"}}]}}")
                            });
        }

        private void HttpMockOneArtifact()
        {
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testBuild.Url}/artifacts?api-version=5.1")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{testArtifact.Id},\"name\":\"{testArtifact.Name}\",\"resource\":{{\"type\":\"{testArtifact.Resource.Type}\",\"downloadUrl\":\"{ testArtifact.Resource.DownloadUrl }\"}}}}]}}")
                });
        }

        private void HttpMockOneRelease()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/{testProject.Id}/_apis/release/releases")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{testRelease.Id},\"name\":\"{testRelease.Name}\",\"status\":\"{testRelease.Status}\",\"createdOn\":\"{testRelease.CreatedOn}\",\"createdBy\":{{\"displayName\":\"{testRelease.CreatedBy.DisplayName}\",\"id\":\"{testRelease.CreatedBy.Id}\",\"uniqueName\":\"{testRelease.CreatedBy.UniqueName}\"}},\"url\":\"{testRelease.Url}\"}}]}}")
                            });
        }

        private void HttpMockOneReleaseDetails()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{testRelease.Url}")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{testRelease.Id},\"name\":\"{testRelease.Name}\",\"status\":\"{testRelease.Status}\",\"createdOn\":\"{testRelease.CreatedOn}\",\"createdBy\":{{\"displayName\":\"{testRelease.CreatedBy.DisplayName}\",\"id\":\"{testRelease.CreatedBy.Id}\",\"uniqueName\":\"{testRelease.CreatedBy.UniqueName}\"}},\"url\":\"{testRelease.Url}\"}}]}}")
                            });
        }
    }
}
