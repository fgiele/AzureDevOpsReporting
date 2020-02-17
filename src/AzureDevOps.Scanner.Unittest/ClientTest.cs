using AzureDevOps.Model;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
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
                            req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection }, expectedUrl);

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
            var expectedCollection1 = "testcol1";
            var expectedCollection2 = "testcol2";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection2}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, expectedUrl);

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
                            req => req.RequestUri.ToString() == $"{azureUrl}/{expectedCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection }, azureUrl);

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
            var expectedCollection1 = "testcol1";
            var expectedCollection2 = "testcol2";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{azureUrl}/{expectedCollection1}/_apis/projects" ||
                                    req.RequestUri.ToString() == $"{azureUrl}/{expectedCollection2}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection1, expectedCollection2 }, azureUrl));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task ScanAsync_WhenNoBuilds_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            expectedProject.Builds = Array.Empty<AzureDevOpsBuild>();

            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/{expectedProject.Id}/_apis/build/builds")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsNoArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneBuild();

            expectedProject.Builds = new HashSet<AzureDevOpsBuild> { expectedBuild };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenBuildsWithArtifacts_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneBuild();
            HttpMockOneArtifact();

            expectedBuild.Artifacts = new HashSet<AzureDevOpsBuildArtifact> { expectedArtifact };
            expectedProject.Builds = new HashSet<AzureDevOpsBuild> { expectedBuild }; ;

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Build | DataOptions.BuildArtifacts, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenNoReleases_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();

            expectedProject.Releases = Array.Empty<AzureDevOpsRelease>();

            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/{expectedProject.Id}/_apis/release/releases")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRelease_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneRelease();

            expectedProject.Releases = new HashSet<AzureDevOpsRelease> { expectedRelease };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenReleaseDetails_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneRelease();
            HttpMockOneReleaseDetails();

            expectedProject.Releases = new HashSet<AzureDevOpsRelease> { expectedDetailRelease };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Release | DataOptions.ReleaseDetails, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenNoRepositories_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();

            expectedProject.Repositories = Array.Empty<AzureDevOpsRepository>();

            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/{expectedProject.Id}/_apis/git/repositories?api-version=5.0")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":0,\"value\":[]}")
                });
            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRepositories_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneRepository();

            expectedProject.Repositories = new HashSet<AzureDevOpsRepository> { expectedRepository };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }

        [Fact]
        public async Task ScanAsync_WhenRepositoriesWithMinReviewPolicy_ShouldReturnProperInstance()
        {
            // Arrange
            HttpMockOneProject();
            HttpMockOneRepository();
            HttpMockOneRolicy();

            expectedRepository.Policies = new HashSet<AzureDevOpsPolicy> { expectedPolicy };
            expectedProject.Repositories = new HashSet<AzureDevOpsRepository> { expectedRepository };

            var systemUnderTest = new Client(httpClient);

            // Act
            var actual = await systemUnderTest.ScanAsync(DataOptions.Git | DataOptions.GitPolicies, new string[] { expectedCollection }, expectedUrl);

            // Assert
            mockHttpMessageHandler.Verify();
            actual.Should().BeOfType<AzureDevOpsInstance>();
            actual.Collections.Should().HaveCount(1);
            actual.Collections[0].Projects.Should().HaveCount(1);
            actual.Collections[0].Projects[0].Should().BeEquivalentTo(expectedProject);
        }
    }
}
