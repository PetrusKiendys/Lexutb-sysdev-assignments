using System;
using System.Linq;

// EXTRA: maybe there's a way to template these methods or just reduce the overall LOC, there's a lot of DRY code..
namespace miniproject1 {
    static class InputManager {

        // handle 'int' input
        internal static int ConvertInputToInt(string prompt) {
            string input;
            int? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();
                try { result = Convert.ToInt32(input); }
                catch (System.FormatException) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input was incorrect, only whole numbers allowed!");
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return (int)result;
        }

        // handle 'double' input
        internal static double ConvertInputToDouble(string prompt) {
            string input;
            double? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();
                try { result = Convert.ToDouble(input); }
                catch (System.FormatException) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input was incorrect, only floating-point numbers allowed!");
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return Math.Round((double)result, 2);   // NOTE: rounds the input value to the nearest 2 decimals
        }

        // handle 'bool' input
        internal static bool ConvertInputToBool(string prompt) {
            ConsoleKeyInfo keyInfo;
            do {
                Console.Write(prompt);
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Y) {
                    Console.Write("Y");
                    Console.WriteLine();
                    return true;
                }
                else if (keyInfo.Key == ConsoleKey.N) {
                    Console.Write("N");
                    Console.WriteLine();
                    return false;
                }
                Console.WriteLine();
            } while (keyInfo.Key != ConsoleKey.Y || keyInfo.Key != ConsoleKey.N);
            return default;
        }

        // handle 'string' input
        internal static string ConvertInputToString(string prompt) {
            string input;
            do {
                Console.Write(prompt);
                input = Console.ReadLine();
            } while (String.IsNullOrEmpty(input));
            return input;
        }

        // handle 'DateTime' input
        internal static DateTime ConvertInputToDateTime(string prompt) {
            string input;
            DateTime? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();
                try { result = Convert.ToDateTime(input); }
                catch (System.FormatException) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Input was incorrect, wrong format!");
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return (DateTime)result;
        }

        // handle 'Asset.Location' input
        internal static Asset.Location ConvertInputToLocation(string prompt) {
            string input;
            int? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();
                result = input.ToLower().Trim() switch {        // check for valid string input
                    "usa" => 1,
                    "sweden" => 2,
                    "japan" => 3,
                    _ => null
                };
                if (result is null) {
                    try { result = Convert.ToInt32(input); }    // try to convert input to int
                    catch (System.FormatException) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input was incorrect, only whole numbers or text match is allowed!");
                    }
                }
                if (result is int) {                            // check for valid input range
                    result = (Enumerable.Range(1, 3).Contains((int)result)) ? result : null;
                    if (result is null) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input was incorrect, only numbers within the valid range are allowed!");
                    }
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return (Asset.Location)result;
        }
    }
}
