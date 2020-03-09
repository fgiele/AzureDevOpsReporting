// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsDeployStep.cs" company="Freek Giele">
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
    /// DTO for deployment step.
    /// </summary>
    public class AzureDevOpsDeployStep
    {
        /// <summary>
        /// Gets or sets number of deployment attempt.
        /// </summary>
        public int Attempt { get; set; }

        /// <summary>
        /// Gets or sets release deployment phases in attempt.
        /// </summary>
        public IEnumerable<AzureDevOpsReleaseDeployPhase> ReleaseDeployPhases { get; set; }
    }
}
