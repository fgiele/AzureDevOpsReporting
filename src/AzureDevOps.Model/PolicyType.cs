// -----------------------------------------------------------------------
// <copyright file="PolicyType.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    public static class PolicyType
    {
        public static string PathLengthRestriction => "001a79cf-fda1-4c4e-9e7c-bac40ee5ead8";

        public static string ReservedNamesRestriction => "db2b9b4c-180d-4529-9701-01541d19f36b";

        public static string RequireMergeStrategy => "fa4e907d-c16b-4a4c-9dfa-4916e5d171ab";

        public static string ActiveComments => "c6a1889d-b943-4856-b76f-9e46bb6b0df2";

        public static string SuccessfulBuild => "0609b952-1397-4640-95ec-e00a01b2c241";

        public static string FileSizeRestriction => "2e26e725-8201-4edd-8bf5-978563c34a80";

        public static string RequiredReviewers => "fd2167ab-b0be-447a-8ec8-39368250530e";

        public static string MinimumNumberOfReviewers => "fa4e907d-c16b-4a4c-9dfa-4906e5d171dd";

        public static string WorkItemLink => "40e92b44-2fe1-4dd6-b3d8-74a9c21d0c6e";

        public static string Unknow => string.Empty;
    }
}