// -----------------------------------------------------------------------
// <copyright file="CsvReportDefinition.cs" company="Freek Giele">
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

    /// <summary>
    /// Base implementation of report functions.
    /// </summary>
    public abstract class CsvReportDefinition
    {
        private readonly StringBuilder reportBuilder = new StringBuilder();

        /// <summary>
        /// Gets CSV seperator character being used.
        /// </summary>
        internal static string Separator => ";";

        /// <summary>
        /// Creates the heaqder for the CSV document.
        /// </summary>
        /// <param name="headers">Array of values to add as column headers.</param>
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

        /// <summary>
        /// Adds a row to the report in the CSV format.
        /// </summary>
        /// <param name="values">Array of values to add in the cells of the row.</param>
        internal void AddLine(params object[] values)
        {
            foreach (var value in values)
            {
                this.reportBuilder.Append(MakeString(value));
                this.reportBuilder.Append(Separator);
            }

            this.reportBuilder.AppendLine();
        }

        /// <summary>
        /// Generate the content of the report.
        /// </summary>
        /// <returns>All data added so far to the report, in CSV format.</returns>
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
