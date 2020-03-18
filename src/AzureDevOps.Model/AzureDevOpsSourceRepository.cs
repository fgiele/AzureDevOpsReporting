// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsSourceRepository.cs" company="Freek Giele">
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
    /// DTO for Source repository for a build.
    /// </summary>
    public class AzureDevOpsSourceRepository
    {
        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }
    }
}