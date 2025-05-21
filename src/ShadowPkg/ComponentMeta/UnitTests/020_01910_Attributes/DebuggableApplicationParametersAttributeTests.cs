using System;
using Cozyupk.Shadow.Flow.ShadowPkg.ComponentMeta.Attributes;
using Xunit;

namespace Cozyupk.Shadow.Flow.ShadowPkg.ComponentMeta.UnitTests.Attributes
{
    /// <summary>
    /// Unit tests for the <see cref="DebuggableApplicationParametersAttribute"/> class.
    /// Verifies the behavior of the IsDebugOutputTarget property and its associated logic.
    /// </summary>
    public class DebuggableApplicationParametersAttributeTests
    {
        /// <summary>
        /// Tests that the IsDebugOutputTargetBool property correctly parses valid boolean strings.
        /// </summary>
        /// <param name="input">The input string to parse.</param>
        /// <param name="expected">The expected boolean value.</param>
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("TRUE", true)]
        [InlineData("FALSE", false)]
        [InlineData("TrUe", true)]
        [InlineData("FaLsE", false)]
        public void IsDebugOutputTargetBool_Should_Return_Parsed_Value_When_Input_Is_Valid(string input, bool expected)
        {
            // Arrange
            var attr = new DebuggableApplicationParametersAttribute
            {
                IsDebugOutputTarget = input
            };

            // Act & Assert
            Assert.Equal(expected, attr.IsDebugOutputTargetBool);
        }

        /// <summary>
        /// Tests that setting an invalid value to IsDebugOutputTarget throws an ArgumentException.
        /// </summary>
        /// <param name="input">The invalid input string.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("yes")]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("maybe")]
        public void IsDebugOutputTarget_Should_Throw_ArgumentException_When_Input_Is_Invalid(string? input)
        {
            // Arrange
            var attr = new DebuggableApplicationParametersAttribute();

            // Act
#pragma warning disable CS8601 // Null reference assignment possibility
            var ex = Assert.Throws<ArgumentException>(() => attr.IsDebugOutputTarget = input);
#pragma warning restore CS8601 // Null reference assignment possibility

            // Assert
            Assert.Contains("Invalid boolean string", ex.Message);
        }
    }
}
