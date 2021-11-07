using System;
using System.Collections.Generic;
using System.Linq;

namespace checkpoint2 {
    class ProductList {
        public enum Sort { NONE, PRICE_ASCENDING };
        List<Product> productList;
        int columnWidth = 15;

        public ProductList() {
            productList = new List<Product>();
        }

        public void Add(Product product) {
            productList.Add(product);
        }

        // TODO:
        //  - (extra) handle scenarios where cell values exceed `columnWidth` characters length
        public void Print(Sort sortBy = Sort.NONE) {
            // print header
            if (sortBy == Sort.NONE)
                Console.WriteLine("\nProducts (unsorted)");
            else if (sortBy == Sort.PRICE_ASCENDING)
                Console.WriteLine("\nProducts (sorted by ascending price)");

            // print separator (header)
            Console.WriteLine(new string('=', columnWidth*3));
            Console.WriteLine("Category".PadRight(columnWidth) + "Name".PadRight(columnWidth) + "Price".PadRight(columnWidth));

            // print contents of productList
            if (sortBy == Sort.PRICE_ASCENDING)
                productList = productList.OrderBy(p => p.price).ToList();
            foreach (Product p in productList) {
                Console.WriteLine(p.category.PadRight(columnWidth) + p.name.PadRight(columnWidth) + p.price.ToString().PadRight(columnWidth));
            }

            // print separator (footer #1)
            Console.WriteLine(new string('-', columnWidth*3));

            // print sum of price (total expenses)
            Console.WriteLine("TOTAL PRICE: " + calcTotalPrice().ToString());

            // print separator (footer #2)
            Console.WriteLine(new string('-', columnWidth*3) + "\n");
        }

        public bool isEmpty() {
            return !productList.Any();
        }

        private double calcTotalPrice() {
            double result = 0.0;
            foreach (Product p in productList) {
                result += p.price;
            }
            // NOTE: it's necessary to round here due to floating point arithmetic being inaccurate and unreliable at times.
            //       eg. `return result;` yields the following outcome: 32.44 + 54555.55 = 54587.990000000005
            //       read more: https://stackoverflow.com/questions/10056117/c-sharp-double-addition-strange-behaviour
            //                  https://stackoverflow.com/questions/618535/difference-between-decimal-float-and-double-in-net/618596
            return Math.Round(result, 2);
        }
    }
}
