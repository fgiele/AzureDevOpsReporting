// -----------------------------------------------------------------------
// <copyright file="ClientTest.HelperClasses.cs" company="Freek Giele">
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
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using AzureDevOps.Model;
    using FizzWare.NBuilder;
    using Moq;

    /// <summary>
    /// Contains the helper functions and variables to run the unit tests.
    /// </summary>
    public partial class ClientTest
    {
        private const string ExpectedUrl = "https://example.com";
        private const string ExpectedCollection = "expectedcol";

        private readonly AzureDevOpsProject expectedProject = Builder<AzureDevOpsProject>.CreateNew().Build();

        private readonly AzureDevOpsBuild expectedBuild = Builder<AzureDevOpsBuild>.CreateNew().Do(moq => moq.Url = new Uri($"{ExpectedUrl}/expectedbuild")).Build();

        private readonly AzureDevOpsBuildArtifact expectedArtifact = Builder<AzureDevOpsBuildArtifact>.CreateNew()
            .Do(moq => moq.Resource = Builder<AzureDevOpsArtifactResource>.CreateNew().Build()).Build();

        private readonly AzureDevOpsBuildTimeline expectedTimeline = Builder<AzureDevOpsBuildTimeline>.CreateNew()
            .Do(moq => moq.Records = new HashSet<AzureDevOpsTimelineRecord>
            {
                Builder<AzureDevOpsTimelineRecord>.CreateNew()
                    .Do(moq=>moq.Task = Builder<AzureDevOpsTask>.CreateNew().Build())
                .Build(),
            }).Build();

        private readonly AzureDevOpsRelease expectedRelease = Builder<AzureDevOpsRelease>.CreateNew()
            .Do(moq => moq.CreatedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
            .Do(moq => moq.Url = new Uri($"{ExpectedUrl}/expectedrelease"))
                .Build();

        private readonly AzureDevOpsRelease expectedDetailRelease = Builder<AzureDevOpsRelease>.CreateNew()
            .Do(moq => moq.CreatedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
            .Do(moq => moq.Url = new Uri($"{ExpectedUrl}/expecteddetailrelease"))
            .Do(moq => moq.Environments = new HashSet<AzureDevOpsEnvironment>
            {
                Builder<AzureDevOpsEnvironment>.CreateNew()
                    .Do(moq => moq.PostApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                        .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval>
                        {
                            Builder<AzureDevOpsApproval>.CreateNew()
                                .Do(moq => moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                    .Build(),
                        }).Build())
                    .Do(moq => moq.PreApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                        .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval>
                        {
                            Builder<AzureDevOpsApproval>.CreateNew()
                                .Do(moq => moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                    .Build(),
                        }).Build())
                    .Do(moq => moq.PostDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                    {
                        Builder<AzureDevOpsDeployApproval>.CreateNew()
                            .Do(moq => moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                            .Do(moq => moq.ApprovedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                .Build(),
                    })
                    .Do(moq => moq.PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval>
                    {
                        Builder<AzureDevOpsDeployApproval>.CreateNew()
                            .Do(moq => moq.Approver = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                            .Do(moq => moq.ApprovedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
                                .Build(),
                    })
                    .Build(),
            })
            .Do(moq => moq.Artifacts = new HashSet<AzureDevOpsReleaseArtifact>
            {
                Builder<AzureDevOpsReleaseArtifact>.CreateNew()
                    .Do(moq => moq.DefinitionReference = Builder<AzureDevOpsDefinitionReference>.CreateNew()
                        .Do(moq => moq.ArtifactSourceDefinitionUrl = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.ArtifactSourceVersionUrl = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.Branch = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.BuildUri = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.Definition = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.Project = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.PullRequestMergeCommitId = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.Repository = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.RequestedFor = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.SourceVersion = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                        .Do(moq => moq.Version = Builder<AzureDevOpsReferenceField>.CreateNew().Build())
                            .Build())
                        .Build(),
            })
                .Build();

        private readonly AzureDevOpsRepository expectedRepository = Builder<AzureDevOpsRepository>.CreateNew().Build();

        private readonly AzureDevOpsPolicy expectedPolicy = Builder<AzureDevOpsPolicy>.CreateNew()
            .Do(moq => moq.PolicyType = Builder<AzureDevOpsPolicyType>.CreateNew()
                .Do(moq => moq.Id = new System.Guid(PolicyType.MinimumNumberOfReviewers))
                    .Build())
            .Do(moq => moq.Settings = Builder<AzureDevOpsPolicySettings>.CreateNew()
                .Do(moq => moq.Scope = new HashSet<AzureDevOpsPolicyScope> { Builder<AzureDevOpsPolicyScope>.CreateNew().Build() })
                    .Build())
                .Build();

        private static HttpResponseMessage EmptyResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent("{\"count\":0,\"value\":[]}"),
        };

        private HttpResponseMessage OneProjectResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":\"{this.expectedProject.Id}\",\"name\":\"{this.expectedProject.Name}\",\"description\":\"{this.expectedProject.Description}\",\"url\":\"{this.expectedProject.Url}\"}}]}}"),
        };

        private HttpResponseMessage OneBuildResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{this.expectedBuild.Id},\"buildNumber\":\"{this.expectedBuild.BuildNumber}\",\"status\":\"{this.expectedBuild.Status}\",\"result\":\"{this.expectedBuild.Result}\",\"sourceBranch\":\"{this.expectedBuild.SourceBranch}\",\"startTime\":\"{this.expectedBuild.StartTime}\",\"finishTime\":\"{this.expectedBuild.FinishTime}\",\"queueTime\":\"{this.expectedBuild.QueueTime}\",\"url\":\"{this.expectedBuild.Url}\"}}]}}"),
        };

        private HttpResponseMessage OneArtifactResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{this.expectedArtifact.Id},\"name\":\"{this.expectedArtifact.Name}\",\"resource\":{{\"type\":\"{this.expectedArtifact.Resource.Type}\",\"downloadUrl\":\"{this.expectedArtifact.Resource.DownloadUrl}\"}}}}]}}"),
        };

        private HttpResponseMessage OneTimelineResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"records\":[{{\"type\":\"{this.expectedTimeline.Records.First().Type}\",\"name\":\"{this.expectedTimeline.Records.First().Name}\",\"state\":\"{this.expectedTimeline.Records.First().State}\",\"result\":\"{this.expectedTimeline.Records.First().Result}\",\"order\":{this.expectedTimeline.Records.First().Order},\"task\":{{\"id\":\"{this.expectedTimeline.Records.First().Task.Id}\",\"name\":\"{this.expectedTimeline.Records.First().Task.Name}\",\"version\":\"{this.expectedTimeline.Records.First().Task.Version}\"}}}}]}}"),
        };

        private HttpResponseMessage OneReleaseResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":{this.expectedRelease.Id},\"name\":\"{this.expectedRelease.Name}\",\"status\":\"{this.expectedRelease.Status}\",\"createdOn\":\"{this.expectedRelease.CreatedOn}\"" +
                                $",\"createdBy\":{{\"displayName\":\"{this.expectedRelease.CreatedBy.DisplayName}\",\"id\":\"{this.expectedRelease.CreatedBy.Id}\",\"uniqueName\":\"{this.expectedRelease.CreatedBy.UniqueName}\"}},\"url\":\"{this.expectedRelease.Url}\"}}]}}"),
        };

        private HttpResponseMessage OneRepositoryResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"id\":\"{this.expectedRepository.Id}\",\"name\":\"{this.expectedRepository.Name}\",\"url\":\"{this.expectedRepository.Url}\",\"defaultBranch\":\"{this.expectedRepository.DefaultBranch}\",\"size\":{this.expectedRepository.Size}}}]}}"),
        };

        private HttpResponseMessage OneReleaseDetailResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(this.ReleaseDetailJson),
        };

        private HttpResponseMessage OnePolicyResponse => new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent($"{{\"count\":1,\"value\":[{{\"isEnabled\":\"{this.expectedPolicy.IsEnabled}\",\"isBlocking\":\"{this.expectedPolicy.IsBlocking}\",\"settings\":{{\"minimumApproverCount\":\"{this.expectedPolicy.Settings.MinimumApproverCount}\"" +
                                $",\"creatorVoteCounts\":\"{this.expectedPolicy.Settings.CreatorVoteCounts}\",\"allowDownvotes\":\"{this.expectedPolicy.Settings.AllowDownvotes}\",\"builddefinitionid\":\"{this.expectedPolicy.Settings.BuildDefinitionId}\"" +
                                $",\"displayname\":\"{this.expectedPolicy.Settings.DisplayName}\",\"validduration\":\"{this.expectedPolicy.Settings.ValidDuration}\",\"resetOnSourcePush\":\"{this.expectedPolicy.Settings.ResetOnSourcePush}\"" +
                                $",\"scope\":[{{\"refName\":\"{this.expectedPolicy.Settings.Scope.First().RefName}\",\"matchKind\":\"{this.expectedPolicy.Settings.Scope.First().MatchKind}\"}}]}}" +
                                $",\"type\":{{\"id\":\"{this.expectedPolicy.PolicyType.Id}\",\"url\":\"{this.expectedPolicy.PolicyType.Url}\",\"displayName\":\"{this.expectedPolicy.PolicyType.DisplayName}\"}}}}]}}"),
        };

        // Since releaseDetail is very complex, this has been split over multiple property getters for readability
        private string ReleaseDetailJson =>
                                $"{{\"id\":{this.expectedDetailRelease.Id}" +
                                $",\"name\":\"{this.expectedDetailRelease.Name}\"" +
                                $",\"status\":\"{this.expectedDetailRelease.Status}\"" +
                                $",\"createdOn\":\"{this.expectedDetailRelease.CreatedOn}\"" +
                                $",\"createdBy\":{{{this.ReleaseDetailCreatedIdentityJson}}}" +
                                $",\"environments\":[{this.ReleaseDetailEnvironmentJson}]" +
                                $",\"artifacts\":[{this.ReleaseDetailArtifactJson}]" +
                                $",\"url\":\"{this.expectedDetailRelease.Url}\"}}";

        private string ReleaseDetailCreatedIdentityJson =>
                                $"\"displayName\":\"{this.expectedDetailRelease.CreatedBy.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.CreatedBy.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.CreatedBy.UniqueName}\"";

        private string ReleaseDetailEnvironmentJson =>
                                $"{{\"id\":{this.expectedDetailRelease.Environments.First().Id}" +
                                $",\"name\":\"{this.expectedDetailRelease.Environments.First().Name}\"" +
                                $",\"status\":\"{this.expectedDetailRelease.Environments.First().Status}\"" +
                                $",\"preDeployApprovals\":[{this.ReleaseDetailPreDeployApprovalsJson}]" +
                                $",\"postDeployApprovals\":[{this.ReleaseDetailPostDeployApprovalsJson}]" +
                                $",\"preApprovalsSnapshot\":{this.ReleaseDetailPreApprovalSnapshotJson}" +
                                $",\"postApprovalsSnapshot\":{this.ReleaseDetailPostApprovalSnapshotJson}" +
                                $",\"createdOn\":\"{this.expectedDetailRelease.Environments.First().CreatedOn}\"" +
                                $",\"triggerReason\":\"{this.expectedDetailRelease.Environments.First().TriggerReason}\"}}";

        private string ReleaseDetailPreDeployApprovalsJson =>
                                $"{{\"id\":{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Id}" +
                                $",\"revision\":{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Revision}" +
                                $",\"approvalType\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovalType}\"" +
                                $",\"createdOn\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().CreatedOn}\"" +
                                $",\"status\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Status}\"" +
                                $",\"approver\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"approvedBy\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().ApprovedBy.UniqueName}\"}}" +
                                $",\"comments\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Comments}\"" +
                                $",\"isAutomated\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().IsAutomated}\"" +
                                $",\"attempt\":{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Attempt}" +
                                $",\"url\":\"{this.expectedDetailRelease.Environments.First().PreDeployApprovals.First().Url}\"}}";

        private string ReleaseDetailPostDeployApprovalsJson =>
                                $"{{\"id\":{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Id}" +
                                $",\"revision\":{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Revision}" +
                                $",\"approvalType\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().ApprovalType}\"" +
                                $",\"createdOn\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().CreatedOn}\"" +
                                $",\"approver\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Approver.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Approver.UniqueName}\"}}" +
                                $",\"approvedBy\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().ApprovedBy.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().ApprovedBy.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().ApprovedBy.UniqueName}\"}}" +
                                $",\"status\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Status}\"" +
                                $",\"comments\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Comments}\"" +
                                $",\"isAutomated\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().IsAutomated}\"" +
                                $",\"attempt\":{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Attempt}" +
                                $",\"url\":\"{this.expectedDetailRelease.Environments.First().PostDeployApprovals.First().Url}\"}}";

        private string ReleaseDetailPreApprovalSnapshotJson =>
                                $"{{\"approvals\":[{{" +
                                $"\"approver\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Approver.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Approver.UniqueName}\"}}" +
                                $",\"rank\":{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Rank}" +
                                $",\"isAutomated\":\"{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().IsAutomated}\"" +
                                $",\"id\":{this.expectedDetailRelease.Environments.First().PreApprovalsSnapshot.Approvals.First().Id}}}]}}";

        private string ReleaseDetailPostApprovalSnapshotJson =>
                                $"{{\"approvals\":[{{\"rank\":{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Rank}" +
                                $",\"approver\":{{\"displayName\":\"{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Approver.DisplayName}\"" +
                                $",\"id\":\"{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Approver.Id}\"" +
                                $",\"uniqueName\":\"{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Approver.UniqueName}\"}}" +
                                $",\"rank\":{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Rank}" +
                                $",\"isAutomated\":\"{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().IsAutomated}\"" +
                                $",\"id\":{this.expectedDetailRelease.Environments.First().PostApprovalsSnapshot.Approvals.First().Id}}}]}}";

        private string ReleaseDetailArtifactJson =>
                                $"{{\"sourceId\":\"{this.expectedDetailRelease.Artifacts.First().SourceId}\"" +
                                $",\"type\":\"{this.expectedDetailRelease.Artifacts.First().Type}\"" +
                                $",\"definitionReference\":{this.ReleaseDetailArtifactDefinitionReferenceJson}" +
                                $",\"isPrimary\":\"{this.expectedDetailRelease.Artifacts.First().IsPrimary}\"" +
                                $",\"isRetained\":\"{this.expectedDetailRelease.Artifacts.First().IsRetained}\"}}";

        private string ReleaseDetailArtifactDefinitionReferenceJson =>
                                $"{{\"buildUri\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.BuildUri.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.BuildUri.Name}\"}}" +
                                $",\"definition\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Definition.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Definition.Name}\"}}" +
                                $",\"pullRequestMergeCommitId\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.PullRequestMergeCommitId.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.PullRequestMergeCommitId.Name}\"}}" +
                                $",\"project\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Project.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Project.Name}\"}}" +
                                $",\"repository\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Repository.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Repository.Name}\"}}" +
                                $",\"requestedFor\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.RequestedFor.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.RequestedFor.Name}\"}}" +
                                $",\"sourceVersion\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.SourceVersion.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.SourceVersion.Name}\"}}" +
                                $",\"version\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Version.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Version.Name}\"}}" +
                                $",\"artifactSourceVersionUrl\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceVersionUrl.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceVersionUrl.Name}\"}}" +
                                $",\"artifactSourceDefinitionUrl\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceDefinitionUrl.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.ArtifactSourceDefinitionUrl.Name}\"}}" +
                                $",\"branch\":{{\"id\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Branch.Id}\"" +
                                $",\"name\":\"{this.expectedDetailRelease.Artifacts.First().DefinitionReference.Branch.Name}\"}}}}";

        private void HttpMockOneProject()
        {
            this.mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/_apis/projects")))
                .Returns(this.OneProjectResponse);
        }

        private void HttpMockOneBuild()
        {
            this.mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/build/builds")))
                            .Returns(this.OneBuildResponse);
        }

        private void HttpMockOneArtifact()
        {
            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{this.expectedBuild.Url}/artifacts?api-version=5.1")))
                .Returns(this.OneArtifactResponse);
        }

        private void HttpMockOneTimeline()
        {
            this.mockHttpMessageHandler.Setup(
                    mh => mh.Send(
                        It.Is<HttpRequestMessage>(
                            req => req.RequestUri.ToString() == $"{this.expectedBuild.Url}/timeline")))
                .Returns(this.OneTimelineResponse);
        }

        private void HttpMockOneRelease()
        {
            this.mockHttpMessageHandler.Setup(
                                mh => mh.Send(
                                    It.Is<HttpRequestMessage>(
                                        req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/release/releases")))
                            .Returns(this.OneReleaseResponse);
        }

        private void HttpMockOneRepository()
        {
            this.mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/git/repositories?api-version=5.0")))
                .Returns(this.OneRepositoryResponse);
        }

        private void HttpMockOneRolicy()
        {
            this.mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{ExpectedUrl}/{ExpectedCollection}/{this.expectedProject.Id}/_apis/git/policy/configurations?repositoryId={this.expectedRepository.Id}&refName=refs/heads/master")))
                .Returns(this.OnePolicyResponse);
        }

        private void HttpMockOneReleaseDetails()
        {
            this.mockHttpMessageHandler.Setup(
                mh => mh.Send(
                    It.Is<HttpRequestMessage>(
                        req => req.RequestUri.ToString() == $"{this.expectedRelease.Url}")))
                .Returns(this.OneReleaseDetailResponse);
        }
    }
}
