// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsTrigger.cs" company="Freek Giele">
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
    /// DTO for release definition trigger.
    /// </summary>
    public class AzureDevOpsTrigger
    {
        /// <summary>
        /// Gets or sets the type of trigger.
        /// </summary>
        public AzureDevOpsTriggerType TriggerType { get; set; }

        /// <summary>
        /// Gets or sets the trigger conditions.
        /// </summary>
        public IEnumerable<AzureDevOpsTriggerCondition> TriggerConditions { get; set; }
    }
}