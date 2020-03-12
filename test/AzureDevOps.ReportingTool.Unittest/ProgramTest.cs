// -----------------------------------------------------------------------
// <copyright file="ProgramTest.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.ReportingTool.Unittest
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class ProgramTest
    {
        [Fact]
        public void Main_WhenCalledWithoutArguments_DoesNotReturnError()
        {
            // Arrange
            string[] arguments = null;

            // Act
            var actualException = Record.Exception(() => Program.Main(arguments));

            // Assert
            actualException.Should().BeNull();
        }

        [Fact]
        public void Main_WhenCalledWithoutConfigFile_DoesNotReturnError()
        {
            // Arrange
            var arguments = Array.Empty<string>();

            // Act
            var actualException = Record.Exception(() => Program.Main(arguments));

            // Assert
            actualException.Should().BeNull();
        }

        [Fact]
        public void Main_WhenCalledWithInvalidConfigFile_DoesNotReturnError()
        {
            // Arrange
            var arguments = new string[] { "--config", "nosuchfile" };

            // Act
            var actualException = Record.Exception(() => Program.Main(arguments));

            // Assert
            actualException.Should().BeNull();
        }
    }
}
