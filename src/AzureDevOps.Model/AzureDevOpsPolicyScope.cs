// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsPolicyScope.cs" company="Freek Giele">
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
    /// DTO for policy scope.
    /// </summary>
    public class AzureDevOpsPolicyScope
    {
        /// <summary>
        /// Gets or sets reference name.
        /// </summary>
        public string RefName { get; set; }

        /// <summary>
        /// Gets or sets type of match.
        /// </summary>
        public string MatchKind { get; set; }
    }
}
