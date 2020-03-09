// -----------------------------------------------------------------------
// <copyright file="DataOptions.cs" company="Freek Giele">
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

    [Flags]
    public enum DataOptions
    {
        Git = 1,
        GitPolicies = 2,
        Build = 4,
        BuildArtifacts = 8,
        Release = 16,
        ReleaseDetails = 32,
    }
}
