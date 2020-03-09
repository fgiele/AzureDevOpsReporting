// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsPolicyType.cs" company="Freek Giele">
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

    public class AzureDevOpsPolicyType
    {
        public Guid Id { get; set; }

        public System.Uri Url { get; set; }

        public string DisplayName { get; set; }
    }
}