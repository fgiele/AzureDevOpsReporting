// -----------------------------------------------------------------------
// <copyright file="CsvReportDefinitionTest.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Report.Unittest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AzureDevOps.Model;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// Contains the actual tests being run against the build report.
    /// </summary>
    public class CsvReportDefinitionTest
    {
        [Fact]
        public void AddLine_WhenUsingSeperatorChar_EscapingHappens()
        {
            // Arrange
            var expectedReport = $"\"Test;Name\";{Environment.NewLine}";
            var textString = "Test;Name";
            var vsinstance = new AzureDevOpsInstance();
            vsinstance.Collections.Add(new AzureDevOpsCollection { Name = textString });
            var systemUnderTest = new CsvTestClass();

            // Act
            var actual = systemUnderTest.Generate(vsinstance);

            // Assert
            actual.Should().Be(expectedReport);
        }

        [Theory]
        [InlineData("Deze\rtext","Dezetext")]
        [InlineData("Deze\ntext", "Dezetext")]
        [InlineData("Deze\ttext", "Deze text")]
        [InlineData("Deze\r\n\ttext", "Deze text")]
        [InlineData("Deze\"text", "Deze\"\"text")]
        public void AddLine_WhenWhitespaceChar_Removed(string testInput, string cleaned)
        {
            // Arrange
            var expected = $"{cleaned};{Environment.NewLine}";
            var vsinstance = new AzureDevOpsInstance();
            vsinstance.Collections.Add(new AzureDevOpsCollection { Name = testInput });
            var systemUnderTest = new CsvTestClass();

            // Act
            var actual = systemUnderTest.Generate(vsinstance);

            // Assert
            actual.Should().Be(expected);
        }

        private class CsvTestClass : CsvReportDefinition, IReport
        {
            public DataOptions DataOptions => throw new NotImplementedException();

            public string Title => throw new NotImplementedException();

            public string Generate(AzureDevOpsInstance instance)
            {
                this.AddLine(instance.Collections.FirstOrDefault().Name);

                return this.GetReport();
            }
        }
    }
}
