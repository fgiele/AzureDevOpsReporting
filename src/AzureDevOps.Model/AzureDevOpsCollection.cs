// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsCollection.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// DTO for collection.
    /// </summary>
    public class AzureDevOpsCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsCollection"/> class.
        /// </summary>
        public AzureDevOpsCollection()
        {
            this.Projects = new List<AzureDevOpsProject>();
        }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets propjects.
        /// </summary>
        public List<AzureDevOpsProject> Projects { get; private set; }
    }
}
