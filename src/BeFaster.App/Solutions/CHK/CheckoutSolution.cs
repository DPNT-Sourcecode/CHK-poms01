using System.Text.RegularExpressions;
using BeFaster.Runner.Exceptions;

namespace BeFaster.App.Solutions.CHK
{
    public static class CheckoutSolution
    {
        public static int ComputePrice(string? skus)
        {
            if(string.IsNullOrEmpty(skus))
                return 0;
            
            return ComputeTotalPrice(skus);
        }

        private static int ComputeTotalPrice(string skus)
        {
            var totalPrice = 0;
            var items = RetrieveItemsFromSkuString(skus.Trim());
            foreach (var sku in items)
            {
                var skuName = sku.Key.ToString();
                var skuPrice = ComputeIndividualPrice(skuName, sku.Value);
                if(skuPrice == -1)
                    return -1;

                totalPrice += skuPrice;
            }
            return totalPrice;
        }

        private static Dictionary<char, int> RetrieveItemsFromSkuString(string skus)
        {
            var result = new Dictionary<char, int>();

            result = skus
                .GroupBy(c => c)
                .ToDictionary(item => item.Key, item => item.Count());

            return result;
        }


        private static int RetrieveNumberOfItems(string sku)
        {
            var numberOfItemsString = Regex.Match(sku, @"^\d+").Value;
            if(string.IsNullOrEmpty(numberOfItemsString))
                return 1;
            return int.Parse(numberOfItemsString);
        }

        private static string RetrieveSKUName(string sku)
        {
            var skuName = Regex.Match(sku, @"[a-zA-Z]+$").Value;
            return skuName;
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
            int discountItemsTotalPrice = numberOfItems / numberOfItemsForDiscount * discountPrice;
            int individualTotalPrice = numberOfItems % numberOfItemsForDiscount * individualPrice;
            return discountItemsTotalPrice + individualTotalPrice;
        }
    }
}
