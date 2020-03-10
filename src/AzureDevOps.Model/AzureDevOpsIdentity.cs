// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsIdentity.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System;

    /// <summary>
    /// DTO for identity.
    /// </summary>
    public class AzureDevOpsIdentity
    {
        /// <summary>
        /// Gets or sets id.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets displayname.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets unique identifying name.
        /// </summary>
        public string UniqueName { get; set; }
    }
}
