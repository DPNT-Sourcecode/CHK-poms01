using BeFaster.App.Solutions.CHK;
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

        // [TestCase("3A", ExpectedResult = 130)]
        [TestCase("3B", ExpectedResult = 75)]
        public int ComputePrice(string? skus)
        {
            return CheckoutSolution.ComputePrice(skus);
        }
    }
}



