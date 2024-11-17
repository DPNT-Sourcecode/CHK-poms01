using System.Globalization;
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
            var groupPrice = ComputePriceForGroupItems(items);
            foreach (var sku in items)
            {
                var skuName = sku.Key.ToString();
                var skuPrice = ComputeIndividualPrice(items, skuName, sku.Value);
                if(skuPrice == -1)
                    return -1;

                totalPrice += skuPrice;
            }
            return totalPrice + groupPrice;
        }

        private static Dictionary<char, int> RetrieveItemsFromSkuString(string skus)
        {
            var result = new Dictionary<char, int>();

            result = skus
                .GroupBy(c => c)
                .ToDictionary(item => item.Key, item => item.Count());

            return result;
        }

        private static int ComputePriceForGroupItems(Dictionary<char, int> items)
        {
            var searchedItems = new List<char> { 'Z', 'S', 'T', 'Y', 'X'};
            var count = items.Where(item => searchedItems.Contains(item.Key)).Select(item => item.Value).Sum();
            var numberOfPacks = count / 3;
            var numberOfItemsToRemove = numberOfPacks * 3;
            var removedItemsCount = 0;


            foreach (var item in searchedItems)
            {
                if(removedItemsCount == numberOfItemsToRemove)
                    break;
                
                int zCount;
                if(items.TryGetValue(item, out zCount))
                {
                    if(zCount >= numberOfItemsToRemove - removedItemsCount)
                    {
                        items[item] = zCount - numberOfItemsToRemove;
                        removedItemsCount = numberOfItemsToRemove;
                    } else if(zCount < numberOfItemsToRemove - removedItemsCount) {
                        items[item] = 0;
                        removedItemsCount += zCount;
                    }
                }
            }
            return 45 * numberOfPacks;
        }

        private static (string, int) ComputePriceForGroupItems(string skus)
        {
            var searchedItems = new List<char> { 'S', 'T', 'X', 'Y', 'Z' };
            var skusChars = skus.ToCharArray().ToList();
            var count =  skusChars.Count(item => searchedItems.Contains(item));
            var numberOfPacks = count / 3;
            var newskusChars = new List<char>();
            var charCount = 0;
            var maxCharCount = numberOfPacks * 3;

            foreach (var sku in skusChars)
            {
                if(searchedItems.Contains(sku) && charCount < maxCharCount)
                {
                    charCount += 1;
                    continue;
                }
                newskusChars.Add(sku);
            }
            return (new string(newskusChars.ToArray()), numberOfPacks * 45);
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
                        new ProductPriceList { NumberOfItems = 1, Price = 70},
                        new ProductPriceList { NumberOfItems = 2, Price = 120}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListK);
                case "L":
                    return 90 * numberOfItems;
                case "M":
                    var freeItems = CalculateFreeItems(listOfSKUs, 'N', 3);
                    return 15 * (numberOfItems - freeItems);
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
                    var freeItemsR = CalculateFreeItems(listOfSKUs, 'R', 3);
                    return CalculatePriceIncludingDiscount(numberOfItems - freeItemsR, priceListQ);
                case "R":
                    return 50 * numberOfItems;
                case "S":
                    return 20 * numberOfItems;
                case "T":
                    return 20 * numberOfItems;
                case "U":
                    var freeItemsU = CalculateFreeItems(listOfSKUs, 'U', 4);
                    return 40 * (numberOfItems - freeItemsU);
                case "V":
                    var priceListV = new List<ProductPriceList> 
                    {
                        new ProductPriceList { NumberOfItems = 1, Price = 50},
                        new ProductPriceList { NumberOfItems = 2, Price = 90},
                        new ProductPriceList { NumberOfItems = 3, Price = 130}
                    };
                    return CalculatePriceIncludingDiscount(numberOfItems, priceListV);
                case "W":
                    return 20 * numberOfItems;
                case "X":
                    return 17 * numberOfItems;
                case "Y":
                    return 20 * numberOfItems;
                case "Z":
                    return 21 * numberOfItems;
                default:
                    return -1;
            }
        }

        private static int CalculateFreeItems(IDictionary<char, int> listOfSKUs, char dependentItem, int priviledNumber)
        {
            int ItemsCount;
            listOfSKUs.TryGetValue(dependentItem, out ItemsCount);
            int freeItems = ItemsCount / priviledNumber;
            return freeItems;
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




