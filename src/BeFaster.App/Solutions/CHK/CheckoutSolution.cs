using BeFaster.Runner.Exceptions;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {
        public static int ComputePrice(string? skus)
        {
            if(string.IsNullOrEmpty(skus))
                return -1;
            


            return ComputeIndividualPrice();
        }

        private static int ComputeIndividualPrice(string sku, int numberOfItems = 1)
        {
            switch (sku)
            {
                case "A":
                    return CalculatePriceIncludingDiscount(numberOfItems, 50, 130, 3);
                case "B":
                    return CalculatePriceIncludingDiscount(numberOfItems, 30, 45, 2);
                case "C":
                    return 20 * numberOfItems;
                case "D":
                    return 15 * numberOfItems;
                default:
                    return -1;
            }
        }

        private static int CalculatePriceIncludingDiscount(int numberOfItems, int individualPrice, int discountPrice, int numberOfItemsForDiscount)
        {
            int discountItemsTotalPrice = numberOfItems / 3 * discountPrice;
            int individualTotalPrice = numberOfItems % 3 * individualPrice;
            return discountItemsTotalPrice + individualTotalPrice;
        }
    }
}

