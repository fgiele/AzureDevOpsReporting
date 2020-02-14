using AzureDevOps.Model;
using Moq;
using System;
using System.Collections.Generic;
using FizzWare.NBuilder;

namespace AzureDevOps.Scanner.Unittest
{
    public partial class ClientTest
    {
        private const string testUrl = "https://example.com";
        private const string testCollection = "testcol";

        private readonly AzureDevOpsProject testProject = Builder<AzureDevOpsProject>.CreateNew().Build();

        private readonly AzureDevOpsBuild testBuild = Builder<AzureDevOpsBuild>.CreateNew().Do(moq => moq.Url = $"{testUrl}/testbuild").Build();

        private readonly AzureDevOpsBuildArtifact testArtifact = Builder<AzureDevOpsBuildArtifact>.CreateNew()
            .Do(moq => moq.Resource = Builder<AzureDevOpsArtifactResource>.CreateNew().Build()).Build();

        private readonly AzureDevOpsRelease testRelease = Builder<AzureDevOpsRelease>.CreateNew()
            .Do(moq => moq.CreatedBy = Builder<AzureDevOpsIdentity>.CreateNew().Build())
            .Do(moq => moq.Url = $"{testUrl}/testrelease")
                .Build();

        private readonly AzureDevOpsEnvironment testEnvironment = Builder<AzureDevOpsEnvironment>.CreateNew()
            .Do(moq => moq.PostApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval> { Builder<AzureDevOpsApproval>.CreateNew().Build() })
                .Build())
            .Do(moq => moq.PostDeployApprovals = new HashSet<AzureDevOpsDeployApproval> { Builder<AzureDevOpsDeployApproval>.CreateNew().Build() })
            .Do(moq => moq.PreApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval> { Builder<AzureDevOpsApproval>.CreateNew().Build() })
                .Build())
            .Do(moq => moq.PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval> { Builder<AzureDevOpsDeployApproval>.CreateNew().Build() })
                .Build();

        private readonly AzureDevOpsReleaseArtifact testReleaseArtifact = Builder<AzureDevOpsReleaseArtifact>.CreateNew()
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
                .Build();
    }
}
