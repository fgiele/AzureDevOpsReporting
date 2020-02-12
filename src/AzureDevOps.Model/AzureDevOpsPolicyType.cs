using System;

namespace AzureDevOps.Model
{
    public enum PolicyType
    {
        PathLengthRestriction,
        ReservedNamesRestriction,
        RequireMergeStrategy,
        ActiveComments,
        SuccessfulBuild,
        FileSizeRestriction,
        RequiredReviewers,
        MinimumNumberOfReviewers,
        WorkItemLink,
        Unknown
    }

    public class AzureDevOpsPolicyType
    {
        public Guid Id { get; set; }

        public string Url { get; set; }

        public string DisplayName { get; set; }

        public PolicyType PolicyType
        {
            get
            {
                var polType = PolicyType.Unknown;
                switch (Id.ToString())
                {
                    case "001a79cf-fda1-4c4e-9e7c-bac40ee5ead8":
                        polType = PolicyType.PathLengthRestriction;
                        break;
                    case "db2b9b4c-180d-4529-9701-01541d19f36b":
                        polType = PolicyType.ReservedNamesRestriction;
                        break;
                    case "fa4e907d-c16b-4a4c-9dfa-4916e5d171ab":
                        polType = PolicyType.RequireMergeStrategy;
                        break;
                    case "c6a1889d-b943-4856-b76f-9e46bb6b0df2":
                        polType = PolicyType.ActiveComments;
                        break;
                    case "0609b952-1397-4640-95ec-e00a01b2c241":
                        polType = PolicyType.SuccessfulBuild;
                        break;
                    case "2e26e725-8201-4edd-8bf5-978563c34a80":
                        polType = PolicyType.FileSizeRestriction;
                        break;
                    case "fd2167ab-b0be-447a-8ec8-39368250530e":
                        polType = PolicyType.RequiredReviewers;
                        break;
                    case "fa4e907d-c16b-4a4c-9dfa-4906e5d171dd":
                        polType = PolicyType.MinimumNumberOfReviewers;
                        break;
                    case "40e92b44-2fe1-4dd6-b3d8-74a9c21d0c6e":
                        polType = PolicyType.WorkItemLink;
                        break;
                }

                return polType;
            }
        }
    }
}