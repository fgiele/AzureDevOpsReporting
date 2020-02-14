using AzureDevOps.Model;
using System.Collections.Generic;
using FizzWare.NBuilder;

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

        private readonly AzureDevOpsEnvironment expectedEnvironment = Builder<AzureDevOpsEnvironment>.CreateNew()
            .Do(moq => moq.PostApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval> { Builder<AzureDevOpsApproval>.CreateNew().Build() })
                .Build())
            .Do(moq => moq.PostDeployApprovals = new HashSet<AzureDevOpsDeployApproval> { Builder<AzureDevOpsDeployApproval>.CreateNew().Build() })
            .Do(moq => moq.PreApprovalsSnapshot = Builder<AzureDevOpsDeployApprovalsSnapshot>.CreateNew()
                .Do(moq => moq.Approvals = new HashSet<AzureDevOpsApproval> { Builder<AzureDevOpsApproval>.CreateNew().Build() })
                .Build())
            .Do(moq => moq.PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval> { Builder<AzureDevOpsDeployApproval>.CreateNew().Build() })
                .Build();

        private readonly AzureDevOpsReleaseArtifact expectedReleaseArtifact = Builder<AzureDevOpsReleaseArtifact>.CreateNew()
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
