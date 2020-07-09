// -----------------------------------------------------------------------
// <copyright file="MDResportDefinitionTest.cs" company="Freek Giele">
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
    using System.Linq;
    using AzureDevOps.Model;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// Contains the actual tests being run against the build report.
    /// </summary>
    public class MDResportDefinitionTest
    {
        [Fact]
        public void AddLine_WhenUsingPipeChar_EscapingHappens()
        {
            // Arrange
            var expectedReport = $"|Test\\|Name|{Environment.NewLine}";
            var textString = "Test|Name";
            var vsinstance = new AzureDevOpsInstance();
            vsinstance.Collections.Add(new AzureDevOpsCollection { Name = textString });
            var systemUnderTest = new MDTestClass();

            // Act
            var actual = systemUnderTest.Generate(vsinstance);

            // Assert
            actual.Should().Be(expectedReport);
        }

        [Theory]
        [InlineData("Deze\rtext", "Dezetext")]
        [InlineData("Deze\ntext", "Dezetext")]
        [InlineData("Deze\ttext", "Deze text")]
        [InlineData("Deze\r\n\ttext", "Deze text")]
        public void AddLine_WhenWhitespaceChar_Removed(string testInput, string cleaned)
        {
            // Arrange
            var expected = $"|{cleaned}|{Environment.NewLine}";
            var vsinstance = new AzureDevOpsInstance();
            vsinstance.Collections.Add(new AzureDevOpsCollection { Name = testInput });
            var systemUnderTest = new MDTestClass();

            // Act
            var actual = systemUnderTest.Generate(vsinstance);

            // Assert
            actual.Should().Be(expected);
        }

        private class MDTestClass : MDReportDefinition, IReport
        {
            public DataOptions DataOptions => throw new NotImplementedException();

            public string Title => throw new NotImplementedException();

            public string Generate(AzureDevOpsInstance instance)
            {
                this.AddRow(instance.Collections.FirstOrDefault().Name);

                return this.GetReport();
            }
        }
    }
}
