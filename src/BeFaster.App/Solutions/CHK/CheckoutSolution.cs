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
                    var priceList = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 50},
                        new ProductPriceList { NumberOfItems = 3, Price = 130},
                        new ProductPriceList { NumberOfItems = 5, Price = 200}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, 50, 130, 3);
                case "B":
                    return CalculatePriceIncludingDiscount(numberOfItems, 30, 45, 2);
                case "C":
                    return 20 * numberOfItems;
                case "D":
                    return 15 * numberOfItems;
                case "E":

                    return 40;
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
        private static int CalculatePriceIncludingDiscoun2(int numberOfItems, IEnumerable<ProductPriceList> priceList)
        {
            var price = 0;
            while(numberOfItems > 0)
            {
                var orderedPriceList = priceList.OrderByDescending(item => item.NumberOfItems);
                foreach (var priceItem in orderedPriceList)
                {
                    var discountItemsCount = numberOfItems / priceItem.NumberOfItems;
                    var discountItemsTotalPrice = discountItemsCount * priceItem.Price;
                    price += discountItemsTotalPrice;
                    numberOfItems -= discountItemsCount * priceItem.NumberOfItems;
                }
            }

            return price;
        }
    }

    public class ProductPriceList
    {
        public int Price { get; set; }

        public int NumberOfItems { get; set; } = 1;
        
    }
}

