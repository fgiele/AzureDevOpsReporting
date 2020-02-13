using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using AzureDevOps.Model;
using System;

namespace AzureDevOps.Scanner.Unittest
{
    public class ClientTest
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
            var testUrl = "https://example.com";
            var testCollection = "testcol";
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":1,\"value\":[]}")
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
            var testUrl = "https://example.com";
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
        public async Task ScanAsync_WhenAzureDevOpsUrl_FailsOnMultipleCollection()
        {
            // Arrange
            var testUrl = "https://dev.azure.com";
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
            var actualException = Record.ExceptionAsync(async () => await systemUnderTest.ScanAsync(DataOptions.Build, new string[] { testCollection1, testCollection2 }, testUrl));

            // Assert
            actualException.Should().NotBeNull();
            actualException.Result.Should().BeOfType<InvalidOperationException>();
        }


        [Fact]
        public async Task ScanAsync_WhenNoBuilds_ShouldReturnProperInstance()
        {
            // Arrange
            var testUrl = "https://example.com";
            var testCollection = "testcol";
            var testProjectId = Guid.NewGuid();
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"count\":1,\"value\":[{\"id\":\"" + testProjectId.ToString() + "\",\"name\":\"AzureDevOps\",\"description\":\"All kinds of Azure DevOps related tools and work\",\"url\":\"https://dev.azure.com/fgi/_apis/projects/52f6679b-c352-47ea-af8a-db4c15d02f34\",\"state\":\"wellFormed\",\"revision\":407902160,\"visibility\":\"private\",\"lastUpdateTime\":\"2020-02-12T06:14:14.547Z\"}]}")
                });
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{testUrl}/{testCollection}/{testProjectId}/_apis/build/builds")))
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
            actual.Collections[0].Projects[0].Builds.Should().BeEmpty();
        }
    }
}
