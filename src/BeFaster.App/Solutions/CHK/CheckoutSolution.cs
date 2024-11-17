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
                var skuPrice = ComputeIndividualPrice(items, skuName, sku.Value);
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

        private static int ComputeIndividualPrice(IDictionary<char, int> listOfSKUs, string sku, int numberOfItems = 1)
        {
            switch (sku)
            {
                case "A":
                    var priceListA = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 50},
                        new ProductPriceList { NumberOfItems = 3, Price = 130},
                        new ProductPriceList { NumberOfItems = 5, Price = 200}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListA);
                case "B":
                    var priceListB = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 30},
                        new ProductPriceList { NumberOfItems = 2, Price = 45}
                    };
                    int eItemsCount;
                    listOfSKUs.TryGetValue('E', out eItemsCount);
                    int freeItemsB =  eItemsCount / 2;
                    return CalculatePriceIncludingDiscount(numberOfItems - freeItemsB, priceListB);
                case "C":
                    return 20 * numberOfItems;
                case "D":
                    return 15 * numberOfItems;
                case "E":
                    return 40 * numberOfItems;
                case "F":
                    int fItemsCount;
                    listOfSKUs.TryGetValue('F', out fItemsCount);
                    int freeItemsF = fItemsCount / 3;
                    return 10 * (numberOfItems - freeItemsF);
                case "G":
                    return 20 * numberOfItems;
                case "H":
                    var priceListH = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 10},
                        new ProductPriceList { NumberOfItems = 5, Price = 45},
                        new ProductPriceList { NumberOfItems = 10, Price = 80}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListH);
                case "I":
                    return 35 * numberOfItems;
                case "J":
                    return 60 * numberOfItems;
                case "K":
                    var priceListK = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 80},
                        new ProductPriceList { NumberOfItems = 2, Price = 150}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListK);
                case "L":
                    return 90 * numberOfItems;
                case "M":
                    return 15 * numberOfItems;
                case "N":
                    return 40 * numberOfItems;
                case "O":
                    return 10 * numberOfItems;
                case "P":
                    var priceListP = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 50},
                        new ProductPriceList { NumberOfItems = 5, Price = 200}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListP);
                case "Q":
                    var priceListQ = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 30},
                        new ProductPriceList { NumberOfItems = 3, Price = 80}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListQ);
                case "R":
                    return 50 * numberOfItems;
                case "S":
                    return 30 * numberOfItems;
                default:
                    return -1;
| N    | 40    | 3N get one M free      |
| R    | 50    | 3R get one Q free      |
| S    | 30    |                        |
| T    | 20    |                        |
| U    | 40    | 3U get one U free      |
| V    | 50    | 2V for 90, 3V for 130  |
| W    | 20    |                        |
| X    | 90    |                        |
| Y    | 10    |                        |
| Z    | 50    |                    
            }
        }

        private static int CalculatePriceIncludingDiscount(int numberOfItems, IEnumerable<ProductPriceList> priceList)
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


