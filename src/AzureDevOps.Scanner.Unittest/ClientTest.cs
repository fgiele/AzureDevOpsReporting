using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using AzureDevOps.Model;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AzureDevOps.Scanner.Unittest
{
    public class ClientTest
    {
        private Mock<MockHttpMessageHandler> mockHttpMessageHandler;
        private HttpClient httpClient;
        private const string testUrl = "https://example.com";
        private const string testCollection = "testcol";
        private readonly AzureDevOpsProject testProject = new AzureDevOpsProject
        {
            Description = "testDescription",
            Id = Guid.NewGuid(),
            Name = "testproj",
            Url = $"{testUrl}/12345678",
            Builds = Array.Empty<AzureDevOpsBuild>(),
            Releases = Array.Empty<AzureDevOpsRelease>(),
            Repositories = Array.Empty<AzureDevOpsRepository>()
        };

        private readonly AzureDevOpsBuild testBuild = new AzureDevOpsBuild
        {
            BuildNumber = "testBuildNumber",
            Id = 12345,
            Result = "testResult",
            Status = "testStatus",
            Url = $"{testUrl}/testUrl"
        };

        private readonly AzureDevOpsBuildArtifact testArtifact = new AzureDevOpsBuildArtifact
        {
            Id = 54321,
            Name = "testArtifact",
            Resource = new AzureDevOpsArtifactResource
            {
                DownloadUrl = "testArtifactUrl",
                Type = "testType"
            }
        };

        private readonly AzureDevOpsRelease testRelease = new AzureDevOpsRelease
        {

        };

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
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(testProject);
            actual.Collections[0].Projects[0].Builds.Should().BeEmpty();
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsNoArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneBuild();
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection }, testUrl);

            // Assert
            var filledProject = testProject;
            filledProject.Builds = new HashSet<AzureDevOpsBuild> { testBuild };
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(filledProject);
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

            var filledProject = testProject;
            var filledBuild = testBuild;
            filledBuild.Artifacts = new HashSet<AzureDevOpsBuildArtifact> { testArtifact };
            filledProject.Builds = new HashSet<AzureDevOpsBuild> { filledBuild }; ;

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build | DataOptions.BuildArtifacts, new string[] { testCollection }, testUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(filledProject);
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
    }
}
