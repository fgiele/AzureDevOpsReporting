﻿// -----------------------------------------------------------------------
// <copyright file="AzureDevOpsBuildArtifact.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Model
{
    public class AzureDevOpsBuildArtifact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public AzureDevOpsArtifactResource Resource { get; set; }
    }
}
