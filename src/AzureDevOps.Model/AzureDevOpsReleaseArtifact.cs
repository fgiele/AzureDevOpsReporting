﻿// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsReleaseArtifact.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    public class AzureDevOpsReleaseArtifact
    {
        public string SourceId { get; set; }

        public string Type { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsRetained { get; set; }

        public AzureDevOpsDefinitionReference DefinitionReference { get; set; }
    }
}
