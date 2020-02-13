using AzureDevOps.Model;
using Moq;
using System;
using System.Collections.Generic;

namespace AzureDevOps.Scanner.Unittest
{
    public partial class ClientTest
    {
        private const string testUrl = "https://example.com";
        private const string testCollection = "testcol";
        private static readonly DateTime testDateTime = DateTime.Now;
        private static readonly DateTime testDateTimeToSeconds = testDateTime.AddTicks(-(testDateTime.Ticks % TimeSpan.TicksPerSecond));

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

        private static readonly AzureDevOpsIdentity testIdentity = new AzureDevOpsIdentity
        {
            DisplayName = "testDisplayName",
            Id = Guid.NewGuid(),
            UniqueName = "test Unique Name"
        };

        private readonly AzureDevOpsRelease testRelease = new AzureDevOpsRelease
        {
            CreatedBy = testIdentity,
            CreatedOn = testDateTimeToSeconds,
            Id = 10203,
            Name = "testReleaseName",
            Status = "testState",
            Url = $"{testUrl}/testReleaseUrl"
        };

        private readonly AzureDevOpsEnvironment testEnvironment = new AzureDevOpsEnvironment
        {
            CreatedOn = testDateTimeToSeconds,
            Id = 30201,
            Name = "testEnvironment",
            PostApprovalsSnapshot = new AzureDevOpsDeployApprovalsSnapshot
            {
                Approvals = new HashSet<AzureDevOpsApproval> {
                    new AzureDevOpsApproval {
                        Approver = testIdentity,
                        Id = 51243,
                        IsAutomated = true,
                        Rank = 2
                    }
                }
            },
            PostDeployApprovals = new HashSet<AzureDevOpsDeployApproval> {
                new AzureDevOpsDeployApproval
                {
                    ApprovalType = "testApprovalTypePost",
                    ApprovedBy = testIdentity,
                    Approver = testIdentity,
                    Attempt = 11,
                    Comments = "testComments",
                    CreatedOn = testDateTimeToSeconds,
                    Id = 2222,
                    IsAutomated = true,
                    Revision = 8,
                    Status="testStatusApp",
                    Url = "testUrl"
                }
            },
            PreApprovalsSnapshot = new AzureDevOpsDeployApprovalsSnapshot
            {
                Approvals = new HashSet<AzureDevOpsApproval> {
                    new AzureDevOpsApproval {
                        Approver = testIdentity,
                        Id = 5143,
                        IsAutomated = false,
                        Rank = 3
                    }
                }
            },
            PreDeployApprovals = new HashSet<AzureDevOpsDeployApproval> {
                new AzureDevOpsDeployApproval
                {
                    ApprovalType = "testApprovalTypePre",
                    ApprovedBy = testIdentity,
                    Approver = testIdentity,
                    Attempt = 11,
                    Comments = "testComment2",
                    CreatedOn = testDateTimeToSeconds,
                    Id = 222,
                    IsAutomated = false,
                    Revision = 7,
                    Status="testStatusAppPre",
                    Url = "testUrlPre"
                }
            },
            Status = "testDeployStatus",
            TriggerReason = "testTriggerReason"
        };

        private readonly AzureDevOpsReleaseArtifact testReleaseArtifact = Mock.Of<AzureDevOpsReleaseArtifact>();
    }
}
