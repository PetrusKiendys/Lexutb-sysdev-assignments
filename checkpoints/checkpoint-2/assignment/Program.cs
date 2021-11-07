using System;

namespace checkpoint2 {
    class Program {

        // NOTE: rounds the input value to the nearest 2 decimals
        private static double? ConvertInputToDouble(string prompt) {
            string input;
            double? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();

                try {
                    result = Convert.ToDouble(input);
                }
                catch (System.FormatException) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Price input was incorrect, only floating-point numbers allowed!");
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return (double)Math.Round((double)result, 2);
        }

        private static string ConvertInputToString(string prompt) {
            string input;
            do {
                Console.Write(prompt);
                input = Console.ReadLine();
            } while (String.IsNullOrEmpty(input));
            return input;
        }

        private static void runProgram(ProductList productList) {
            while(true) {
                Console.WriteLine("Press Enter to add a new product or \'q\' to exit");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Q)
                    break;
                else if (keyInfo.Key == ConsoleKey.Enter) {
                    string category;
                    string name;
                    double? price;

                    Console.WriteLine();
                    // QUESTION: Is there any way to call the same (overloaded) method here
                    //           and get a different return datatype depending on what we want to assign (string or double)?
                    //           Instead of using two differently named methods.
                    //           see also: https://stackoverflow.com/questions/20705643/method-overloading-with-different-return-type
                    //                     https://stackoverflow.com/questions/45011889/overloading-function-with-different-return-type/45011931
                    category = ConvertInputToString("Add product category: ");
                    name = ConvertInputToString("Add product name: ");
                    price = ConvertInputToDouble("Add product price: ");

                    Product product = new Product(category, name, Convert.ToDouble(price));
                    productList.Add(product);
                    Console.WriteLine();
                }
            }

            // print productList if it's not empty
            if (!productList.isEmpty()) {
                productList.Print(ProductList.Sort.PRICE_ASCENDING);
            }
        }

        // program entry point (main method)
        static void Main(string[] args) {
            ProductList productList = new ProductList();

            // first run
            runProgram(productList);

            // prompt to run again
            ConsoleKeyInfo keyInfo;
            do {
                Console.WriteLine("Would you like to run the program again and add more items? [Y/n]");
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Y) {
                    Console.WriteLine();
                    runProgram(productList);
                }
                if (keyInfo.Key == ConsoleKey.N)
                    break;
            } while (keyInfo.Key != ConsoleKey.N || keyInfo.Key != ConsoleKey.Y);

            // print goodbye message
            Console.WriteLine("Goodbye!👋");
        }
    }
}
