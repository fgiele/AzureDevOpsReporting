// -----------------------------------------------------------------------
// <copyright file="ReportDefinition.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report
{
    using System.Text;

    public abstract class ReportDefinition
    {
        private readonly StringBuilder reportBuilder = new StringBuilder();

        internal static string Separator => ";";

        internal void CreateHeaders(params string[] headers)
        {
            this.reportBuilder.AppendLine($"SEP={Separator}");

            foreach (var header in headers)
            {
                this.reportBuilder.Append(header);
                this.reportBuilder.Append(Separator);
            }

            this.reportBuilder.AppendLine();
        }

        internal void AddLine(params object[] values)
        {
            foreach (var value in values)
            {
                this.reportBuilder.Append(MakeString(value));
                this.reportBuilder.Append(Separator);
            }

            this.reportBuilder.AppendLine();
        }

        internal string GetReport()
        {
            return this.reportBuilder.ToString();
        }

        private static string MakeString(object input)
        {
            var stringval = $"{input}";
            return (stringval.Contains(';', System.StringComparison.OrdinalIgnoreCase) ? $"\"{stringval}\"" : stringval)
                    .Replace("\r", string.Empty, System.StringComparison.OrdinalIgnoreCase)
                    .Replace("\n", string.Empty, System.StringComparison.OrdinalIgnoreCase)
                    .Replace("\t", " ", System.StringComparison.OrdinalIgnoreCase)
                    .Replace("\"", "\"\"", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
