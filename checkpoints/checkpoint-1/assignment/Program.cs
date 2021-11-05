using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace checkpoint1
{
    class Program
    {
        static void Main(string[] args)
        {
            // print instructions
            Console.WriteLine("Please provide products. Exit this program by typing 'exit' (case-insensitive).\n");
            
            // initialize variables and flags
            string[] entries = new string[1];
            int index = 0;
            bool validationError = false;
            bool regexMatchInput = false;

            // input loop
            while(true)
            {
                // input data
                Console.Write("Input product: ");
                string input = Console.ReadLine();
                
                // program termination conditional
                if (input.ToLower().Trim() == "exit")
                    break;
                
                // split up input components
                string[] inputComponents = input.Split("-");

                // define local function for generating validation errors
                void generateValidationError(string errorMessage) {
                    validationError = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(errorMessage);
                }

                // make sure input is not empty
                if (!validationError && (String.IsNullOrEmpty(input))) {
                    generateValidationError("Input error: input is empty");
                }

                // make sure input contains exactly one hyphen
                if (!validationError && inputComponents.Length != 2) {      // alternative conditional: input.Count(c => c == '-') != 1
                    generateValidationError("Format error: hyphen missing, or more than one hyphen present in input");
                }

                // make sure either input components are not empty
                if (!validationError && (String.IsNullOrEmpty(inputComponents[0]) || String.IsNullOrEmpty(inputComponents[1]))) {
                    generateValidationError("Input error: left-hand or right-hand side component is empty");
                }

                // check the "letter component" of the input
                if (!validationError) {
                    foreach (char c in inputComponents[0]) {    // could possibly use string.All(...) here:
                                                                // https://docs.microsoft.com/en-us/dotnet/api/system.linq.enumerable.all?view=net-5.0
                        if (!Char.IsLetter(c)) {
                            generateValidationError("Format error: left-hand side contains a non-letter character");
                            break;
                        }
                    }
                }

                // check the "number component" of the input
                if (!validationError) {
                    bool isNumber = int.TryParse(inputComponents[1], out int productNumber);
                    if (!isNumber) {
                        generateValidationError("Format error: right-hand side is not a number");
                    } else if (!Enumerable.Range(201, 299).Contains(productNumber)) {
                        generateValidationError("Value error: right-hand side number is out of range, must be between 200 and 500");
                    }
                }

                // EXTRA: regex match input
                //        note: skips some validations, so it does not completely conform to the requirements of this exercise!
                if (regexMatchInput) {
                    string pattern = @"^[a-zA-Z]+\-[0-9]+$";
                    bool match = Regex.IsMatch(input, pattern);
                    if (match) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Matched pattern \"{pattern}\" in input string \"{input}\"");
                    } else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Did not match pattern \"{pattern}\" in input string \"{input}\"");
                    }
                }

                // add data to array
                if (!validationError) {
                    Array.Resize(ref entries, index+1);
                    entries[index] = input;
                    index++;
                }

                // reset error flags and other transient configurations
                validationError = false;
                Console.ResetColor();
            }

            // sort array
            Array.Sort(entries);

            // print array
            Console.WriteLine("\nThe following products have been registered (sorted): \n");
            foreach (string product in entries) {
                Console.WriteLine("- " + product);
            }
        }
    }
}
