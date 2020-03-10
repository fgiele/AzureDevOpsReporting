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
    /// <summary>
    /// Explicit list of policy types with their associated GUIDs.
    /// </summary>
    public static class PolicyType
    {
        /// <summary>
        /// Gets path length restriction policy GUID.
        /// </summary>
        public static string PathLengthRestriction => "001a79cf-fda1-4c4e-9e7c-bac40ee5ead8";

        /// <summary>
        /// Gets reserved name restriction policy GUID.
        /// </summary>
        public static string ReservedNamesRestriction => "db2b9b4c-180d-4529-9701-01541d19f36b";

        /// <summary>
        /// Gets required merge strategy policy GUID.
        /// </summary>
        public static string RequireMergeStrategy => "fa4e907d-c16b-4a4c-9dfa-4916e5d171ab";

        /// <summary>
        /// Gets active comments policy GUID.
        /// </summary>
        public static string ActiveComments => "c6a1889d-b943-4856-b76f-9e46bb6b0df2";

        /// <summary>
        /// Gets required successful build policy GUID.
        /// </summary>
        public static string SuccessfulBuild => "0609b952-1397-4640-95ec-e00a01b2c241";

        /// <summary>
        /// Gets filesize restriction policy GUID.
        /// </summary>
        public static string FileSizeRestriction => "2e26e725-8201-4edd-8bf5-978563c34a80";

        /// <summary>
        /// Gets required reviewers policy GUID.
        /// </summary>
        public static string RequiredReviewers => "fd2167ab-b0be-447a-8ec8-39368250530e";

        /// <summary>
        /// Gets minimum required reviewers policy GUID.
        /// </summary>
        public static string MinimumNumberOfReviewers => "fa4e907d-c16b-4a4c-9dfa-4906e5d171dd";

        /// <summary>
        /// Gets work item association policy GUID.
        /// </summary>
        public static string WorkItemLink => "40e92b44-2fe1-4dd6-b3d8-74a9c21d0c6e";

        /// <summary>
        /// Gets unknown/placeholder value.
        /// </summary>
        public static string Unknow => string.Empty;
    }
}