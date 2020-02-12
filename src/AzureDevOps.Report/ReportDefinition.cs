using System.Text;

namespace AzureDevOps.Report
{
    public abstract class ReportDefinition
    {
        internal readonly string separator = ";";

        private readonly StringBuilder reportBuilder = new StringBuilder();

        internal void CreateHeaders(params string[] headers)
        {
            reportBuilder.AppendLine($"SEP={separator}");

            foreach (var header in headers)
            {
                reportBuilder.Append(header);
                reportBuilder.Append(separator);
            }
            reportBuilder.AppendLine();
        }

        internal void AddLine(params object[] values)
        {
            foreach (var value in values)
            {
                reportBuilder.Append(MakeString(value));
                reportBuilder.Append(separator);
            }
            reportBuilder.AppendLine();
        }

        internal string GetReport()
        {
            return reportBuilder.ToString();
        }

        private static string MakeString(object input)
        {
            var stringval = $"{input}";
            return (stringval.Contains(';') ? $"\"{stringval}\"" : stringval)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\t", " ")
                    .Replace("\"", "\"\"");
        }
    }
}
