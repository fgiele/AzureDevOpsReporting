using AzureDevOps.Model;
using FizzWare.NBuilder;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AzureDevOps.Scanner.Unittest
{
    public partial class ClientTest
    {
        private const string expectedUrl = "https://example.com";
        private const string expectedCollection = "expectedcol";

        private readonly AzureDevOpsProject expectedProject = Builder<AzureDevOpsProject>.CreateNew().Build();

        private readonly AzureDevOpsBuild expectedBuild = Builder<AzureDevOpsBuild>.CreateNew().Do(moq => moq.Url = $"{expectedUrl}/expectedbuild").Build();

        private readonly AzureDevOpsBuildArtifact expectedArtifact = Builder<AzureDevOpsBuildArtifact>.CreateNew()
            .Do(moq => moq.Resource = Builder<AzureDevOpsArtifactResource>.CreateNew().Build()).Build();

        private readonly AzureDevOpsRelease expectedRelease = Builder<AzureDevOpsRelease>.CreateNew()
            .Do(moq => moq.CreatedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
            .Do(moq => moq.Url = $"{expectedUrl}/expectedrelease")
                .Build();

        private readonly AzureDevOpsRelease expectedDetailRelease = Builder<AzureDevOpsRelease>.CreateNew()
            .Do(moq => moq.CreatedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
            .Do(moq => moq.Url = $"{expectedUrl}/expecteddetailrelease")
            .Do(moq => moq.Environments = new HashSet<AzureDevOpsEnvironment> {
                Builder<AzureDevOpsEnvironment>.CreateNew()
                    .Do(moq=>moq.PostApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                        .Do(moq=>moq.Approvals = new HashSet<AzureDevOpsApproval>{
                            Builder<AzureDevOpsApproval>.CreateNew()
                                .Do(moq=>moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                    .Build()
                        })
                            .Build())
                    .Do(moq=>moq.PreApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                        .Do(moq=>moq.Approvals = new HashSet<AzureDevOpsApproval>{
                            Builder<AzureDevOpsApproval>.CreateNew()
                                .Do(moq=>moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                    .Build()
                        })
                            .Build())
                    .Do(moq=>moq.PostDeployApprovals = new HashSet<AzureDevOpsDeployApproval>{
                        Builder<AzureDevOpsDeployApproval>.CreateNew()
                            .Do(moq=>moq.Approver =Builder<AzureDevOpsIdentity>.CreateNew().Build())
                            .Do(moq=>moq.ApprovedBy =Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                .Build()
                    })
                    .Do(moq=>moq.PreDeployApprovals =new HashSet<AzureDevOpsDeployApproval>{
                        Builder<AzureDevOpsDeployApproval>.CreateNew()
                            .Do(moq=>moq.Approver =Builder<AzureDevOpsIdentity>.CreateNew().Build())
                            .Do(moq=>moq.ApprovedBy =Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                .Build()
                })
                    .Build()})
            .Do(moq => moq.Artifacts = new HashSet<AzureDevOpsReleaseArtifact> {
                Builder<AzureDevOpsReleaseArtifact>.CreateNew()
                    .Do(moq=>moq.DefinitionReference = Builder<AzureDevOpsDefinitionReference>.CreateNew()
                        .Do(moq=>moq.ArtifactSourceDefinitionUrl = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.ArtifactSourceVersionUrl = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.Branch = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.BuildUri = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.Definition = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.Project = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.PullRequestMergeCommitId = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.Repository = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.RequestedFor = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.SourceVersion = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq=>moq.Version = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                            .Build())
                        .Build()
            })
                .Build();

        private void HttpMockOneProject()
        {
            mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/_apis/projects")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":\"{expectedProject.Id}\",\"name\":\"{expectedProject.Name}\",\"description\":\"{expectedProject.Description}\",\"url\":\"{expectedProject.Url}\"}}]}}")
                });
        }

        private void HttpMockOneBuild()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/{expectedProject.Id}/_apis/build/builds")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{expectedBuild.Id},\"buildNumber\":\"{expectedBuild.BuildNumber}\",\"status\":\"{expectedBuild.Status}\",\"result\":\"{expectedBuild.Result}\",\"url\":\"{expectedBuild.Url}\"}}]}}")
                            });
        }

        private void HttpMockOneArtifact()
        {
            mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{expectedBuild.Url}/artifacts?api-version=5.1")))
                .Returns(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{expectedArtifact.Id},\"name\":\"{expectedArtifact.Name}\",\"resource\":{{\"type\":\"{expectedArtifact.Resource.Type}\",\"downloadUrl\":\"{ expectedArtifact.Resource.DownloadUrl }\"}}}}]}}")
                });
        }

        private void HttpMockOneRelease()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{expectedUrl}/{expectedCollection}/{expectedProject.Id}/_apis/release/releases")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{expectedRelease.Id},\"name\":\"{expectedRelease.Name}\",\"status\":\"{expectedRelease.Status}\",\"createdOn\":\"{expectedRelease.CreatedOn}\",\"createdBy\":{{\"displayName\":\"{expectedRelease.CreatedBy.DisplayName}\",\"id\":\"{expectedRelease.CreatedBy.Id}\",\"uniqueName\":\"{expectedRelease.CreatedBy.UniqueName}\"}},\"url\":\"{expectedRelease.Url}\"}}]}}")
                            });
        }

        private void HttpMockOneReleaseDetails()
        {
            mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{expectedRelease.Url}")))
                            .Returns(new HttpResponseMessage
                            {
                                StatusCode = HttpStatusCode.OK,
                                Content = new StringContent($"{{\"id\":{expectedDetailRelease.Id},\"name\":\"{expectedDetailRelease.Name}\",\"status\":\"{expectedDetailRelease.Status}\"" +
                                $",\"createdOn\":\"{expectedDetailRelease.CreatedOn}\",\"createdBy\":{{\"displayName\":\"{expectedDetailRelease.CreatedBy.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.CreatedBy.Id}\",\"uniqueName\":\"{expectedDetailRelease.CreatedBy.UniqueName}\"}},\"environments\":" +
                                $"[{{\"id\":{expectedDetailRelease.Environments.First().Id},\"name\":\"{expectedDetailRelease.Environments.First().Name}\"" +
                                $",\"status\":\"{expectedDetailRelease.Environments.First().Status}\",\"preDeployApprovals\":[{{\"id\":{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Id}" +
                                $",\"revision\":{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Revision},\"approvalType\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovalType}\"" +
                                $",\"createdOn\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().CreatedOn}\",\"status\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Status}\"" +
                                $",\"approver\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"approvedBy\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.UniqueName}\"}}" +
                                $",\"comments\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Comments}\",\"isAutomated\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().IsAutomated}\"" +
                                $",\"attempt\":{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Attempt},\"url\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Url}\"}}]" +
                                $",\"postDeployApprovals\":[{{\"id\":{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Id}" +
                                $",\"revision\":{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Revision},\"approvalType\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().ApprovalType}\"" +
                                $",\"createdOn\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().CreatedOn}\"" +
                                $",\"approver\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"approvedBy\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.UniqueName}\"}}" +
                                $",\"status\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Status}\",\"comments\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Comments}\"" +
                                $",\"isAutomated\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().IsAutomated}\"" +
                                $",\"attempt\":{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Attempt},\"url\":\"{expectedDetailRelease.Environments.First().PostDeployApprovals.First().Url}\"}}],\"preApprovalsSnapshot\":{{\"approvals\":[{{" +
                                $"\"approver\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"rank\":{expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Rank},\"isAutomated\":\"{expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().IsAutomated}\"" +
                                $",\"id\":{expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Id}}}]}}" +
                                $",\"postApprovalsSnapshot\":{{\"approvals\":[{{\"rank\":{expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Rank}" +
                                $",\"approver\":{{\"displayName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.Id}\",\"uniqueName\":\"{expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"isAutomated\":\"{expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().IsAutomated}\"" +
                                $",\"id\":{expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Id}}}]}},\"createdOn\":\"{expectedDetailRelease.Environments.First().CreatedOn}\",\"triggerReason\":\"{expectedDetailRelease.Environments.First().TriggerReason}\"}}]" +
                                $",\"artifacts\":[{{\"sourceId\":\"{expectedDetailRelease.Artifacts.First().SourceId}\",\"type\":\"{expectedDetailRelease.Artifacts.First().Type}\"" +
                                $",\"definitionReference\":{{\"buildUri\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.BuildUri.Id}\",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.BuildUri.Name}\"}}" +
                                $",\"definition\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Definition.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Definition.Name}\"}},\"pullRequestMergeCommitId\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.PullRequestMergeCommitId.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.PullRequestMergeCommitId.Name}\"}},\"project\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Project.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Project.Name}\"}}" +
                                $",\"repository\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Repository.Id}\",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Repository.Name}\"}}" +
                                $",\"requestedFor\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.RequestedFor.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.RequestedFor.Name}\"}},\"sourceVersion\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.SourceVersion.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.SourceVersion.Name}\"}}" +
                                $",\"version\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Version.Id}\",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Version.Name}\"}}" +
                                $",\"artifactSourceVersionUrl\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceVersionUrl.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceVersionUrl.Name}\"}},\"artifactSourceDefinitionUrl\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceDefinitionUrl.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceDefinitionUrl.Name}\"}},\"branch\":{{\"id\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Branch.Id}\"" +
                                $",\"name\":\"{expectedDetailRelease.Artifacts.First().DefinitionReference.Branch.Name}\"}}}}" +
                                $",\"isPrimary\":\"{expectedDetailRelease.Artifacts.First().IsPrimary}\",\"isRetained\":\"{expectedDetailRelease.Artifacts.First().IsRetained}\"}}],\"url\":\"{expectedDetailRelease.Url}\"}}")
                            });
        }
    }
}
