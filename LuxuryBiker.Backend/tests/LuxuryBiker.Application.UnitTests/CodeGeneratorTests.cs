using FluentAssertions;
using LuxuryBiker.Application.Common;
using Xunit;

namespace LuxuryBiker.Application.UnitTests
{
    public class CodeGeneratorTests
    {
        [Theory]
        [InlineData("CLB", null, "CLB1")]
        [InlineData("CLB", "", "CLB1")]
        [InlineData("CLB", "CLB12", "CLB13")]
        [InlineData("VLB", "VLB007", "VLB8")]
        [InlineData("CLB", "sin-numeros", "CLB1")]
        public void Next_returns_the_incremented_code(string prefix, string? lastCode, string expected)
        {
            CodeGenerator.Next(prefix, lastCode).Should().Be(expected);
        }
    }
}
