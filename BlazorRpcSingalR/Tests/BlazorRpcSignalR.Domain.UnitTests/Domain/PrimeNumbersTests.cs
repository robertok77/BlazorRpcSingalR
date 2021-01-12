using System;
using System.Linq;
using BlazorRpcSingalR.Shared.Domain;
using FluentAssertions;
using Xunit;

namespace BlazorRpcSignalR.Domain.UnitTests.Domain
{
    public class PrimeNumbersTests
    {
        [Fact]
        public void FindPrimeNumbersWrongParams_ExpectArgumentOutOfRangeException()
        {
            Action actionArg1 = () => _ = PrimeCalculation.EratosthenesSieve(0, 1, null).ToArray();
            Action actionArg2 = () => _ = PrimeCalculation.EratosthenesSieve(2, 0, null).ToArray();
            actionArg1.Should().ThrowExactly<ArgumentOutOfRangeException>();
            actionArg2.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Theory]
        [InlineData(2, 1, new int[] { }, new int[] { 2 })]
        [InlineData(2, 18, new int[] { }, new int[] { 2, 3, 5, 7, 11, 13, 17, 19 })]
        [InlineData(20, 10, new int[] { 2, 3, 5, 7, 11, 13, 17, 19 }, new int[] { 23, 29 })]
        public void FindPrimeNumbers_ShouldReturnArray(int beginNo, int count, int[] primes, int[] expected)
        {
            var result = PrimeCalculation.EratosthenesSieve(beginNo, count, primes).ToArray();
            result.Should().BeEquivalentTo(expected);
        }
    }
}
