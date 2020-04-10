// -----------------------------------------------------------------------
// <copyright file="BuildDurationReport.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using AzureDevOps.Model;

    public class BuildDurationReport : ReportDefinition, IReport
    {
        public DataOptions DataOptions => throw new System.NotImplementedException();

        public string Title => throw new System.NotImplementedException();

        public string Generate(AzureDevOpsInstance instance)
        {
            throw new System.NotImplementedException();
        }
    }
}
