﻿using BeFaster.App.Solutions.CHK;
using BeFaster.App.Solutions.SUM;
using NUnit.Framework;

namespace BeFaster.App.Tests.Solutions.SUM
{
    [TestFixture]
    public class SumSolutionTest
    {
        [TestCase(1, 1, ExpectedResult = 2)]
        public int ComputeSum(int x, int y)
        {
            return SumSolution.Sum(x, y);
        }

        [TestCase("A", ExpectedResult = 50)]
        [TestCase("AAA", ExpectedResult = 130)]
        [TestCase("BBB", ExpectedResult = 75)]
        [TestCase("AAAAAAA", ExpectedResult = 310)]
        public int ComputePrice(string? skus)
        {
            return CheckoutSolution.ComputePrice(skus);
        }
    }
}


