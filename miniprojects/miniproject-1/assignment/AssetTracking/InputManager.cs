using System;
using System.Linq;
using System.Text;

// IDEA: maybe there's a way to template these methods or just reduce the overall LOC
//       there's a lot of DRY code..
namespace miniproject1 {
    static class InputManager {

        // EXTRA: implement 'LocationPrompt' builder method for selecting asset location
        // build asset type selection prompt
        internal static string AssetTypePrompt(Type[] assetTypes) {
            const string UL = "\x1B[4m";                                                                    // ANSI escape sequence for "underline"
            const string RS = "\x1B[0m";                                                                    // ANSI escape sequence for "reset"
            StringBuilder prompt = new StringBuilder("Select asset type ");
            string[] strArr = assetTypes.Select(t => t.Name).ToArray();                                     // convert to string[]
            for (int i = 0; i < strArr.Length; i++) {
                strArr[i] = strArr[i].Replace("computer", "", true, null);                                  // remove text "Computer" from string
                strArr[i] = UL+strArr[i].Substring(0,1)+RS+strArr[i].Substring(1);                          // underline first character in string
                strArr[i] = $"({i+1}) {strArr[i]}";                                                         // format string
            }
            prompt.Append(string.Join(", ", strArr));                                                       // join strings by ", " (comma character and space)
            prompt.Append(": ");
            return prompt.ToString();
        }

        // handle 'Type' input (for Asset)
        internal static Type AssetTypeSelector(Type[] assetTypes, string prompt) {
            string input;
            int? result = null;
            do {
                Console.ResetColor();
                Console.Write(prompt);
                input = Console.ReadLine();
                result = input.ToLower().Trim() switch {        // check for valid string input
                    "d" or "desktop" => 1,
                    "l" or "laptop" => 2,
                    "s" or "smartphone" => 3,
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
                    result = (Enumerable.Range(1, assetTypes.Length).Contains((int)result)) ? result : null;
                    if (result is null) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Input was incorrect, only numbers within the valid range are allowed!");
                    }
                }
            } while (String.IsNullOrEmpty(input) || !result.HasValue);
            return assetTypes[(int)result-1];
        }

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
                    "u" or "usa" => 1,
                    "s" or "sweden" => 2,
                    "j" or "japan" => 3,
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
