// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsInstance.cs" company="Freek Giele">
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
    /// DTO for Azure DevOps data.
    /// </summary>
    public class AzureDevOpsInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureDevOpsInstance"/> class.
        /// </summary>
        public AzureDevOpsInstance()
        {
            this.Collections = new List<AzureDevOpsCollection>();
        }

        /// <summary>
        /// Gets collections in this Azure DevOps instance.
        /// </summary>
        public List<AzureDevOpsCollection> Collections { get; private set; }
    }
}
