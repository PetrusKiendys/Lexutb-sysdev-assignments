// TODO:
//     - implement all requirements of miniproject 1
//       - make sure sorting works as intended: location -> className -> purchaseDate
using System;

namespace miniproject1 {
    public static class Program {
        // BOOKMARK
        // TODO: complete this method..
        /** run the main application **/
        private static void runProgram(AssetList assetList) {
            while(true) {
                Console.WriteLine("Press Enter to add a new asset or \'q\' to exit");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Q)
                    break;
                else if (keyInfo.Key == ConsoleKey.Enter) {
                    Console.WriteLine("\nPlease enter asset details.");

                    // TODO: Asset creation here..
                    // >>> TODO: Rewrite Asset creation flow (probably in InputManager)
                    //     - Asset creation flow:
                    //         1. display asset selector
                    //             - (eg. [ AssetType(non-abstract) | int | enum ] <-- InputManager.AssetTypeSelector())
                    //               - present int selection options depending on the selectable types
                    //               - return int or type, which following statements can use to create new Asset of the given type
                    //               - should be similar to: InputManager.ConvertInputToLocation(string)
                    //         2. ...
                    //     - create new asset type based on selection
                    //       - probably need to add empty constructors for all instantiable assets
                    //     - iterate over asset properties and invoke corresponding ConvertInputTo... method based on that
                    //       - make a map (InputFuncMap) of properties-to-function:
                    //         https://stackoverflow.com/questions/22599425/how-to-map-strings-to-methods-with-a-dictionary

                    // REMOVEME: this is just a mock flow for the time being..
                    var successfullyAddedAsset = AddNewAsset(assetList);
                    Console.WriteLine();
                    if (successfullyAddedAsset) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Successfully registered asset!");
                        Console.ResetColor();
                    }

                    // ...?
                }
            }

            // print assetList if it's not empty
            if (!assetList.isEmpty()) {
                assetList.Print();
            }
        }

        // REMOVEME: this is just a mock function for the time being...
        private static bool AddNewAsset(AssetList assetList) {
            try {
                // collect user input
                var name = InputManager.ConvertInputToString("Asset name: ");
                DateTime purchaseDate = InputManager.ConvertInputToDateTime("Date of purchase: ");
                Asset.Location location = InputManager.ConvertInputToLocation("Location of asset (1) USA, (2) Sweden, (3) Japan: ");
                double price = InputManager.ConvertInputToDouble("Price (in USD): ");
                string brand = InputManager.ConvertInputToString("Brand: ");
                int weight = InputManager.ConvertInputToInt("Weight (g): ");
                string cpu = InputManager.ConvertInputToString("CPU (name): ");
                string gpu = InputManager.ConvertInputToString("GPU (name): ");
                string ram = InputManager.ConvertInputToString("RAM (name): ");
                int ramSize = InputManager.ConvertInputToInt("RAM size (GB): ");
                string storage = InputManager.ConvertInputToString("Storage (name): ");
                int storageSize = InputManager.ConvertInputToInt("Storage size (GB): ");
                string displayType = InputManager.ConvertInputToString("Display type: ");
                double displaySize = InputManager.ConvertInputToDouble("Display size (inches): ");
                string batteryType = InputManager.ConvertInputToString("Battery type: ");
                int batteryCapacity = InputManager.ConvertInputToInt("Battery capacity (mAh): ");
                bool webcam = InputManager.ConvertInputToBool("Has webcam [Y/n]? ");
                bool trackpad = InputManager.ConvertInputToBool("Has trackpad [Y/n]? ");

                // add new asset to list
                LaptopComputer asset = new LaptopComputer(name, purchaseDate, price, location, brand, weight, cpu, gpu, ram, ramSize, storage, storageSize, displayType, displaySize, batteryType, batteryCapacity, webcam, trackpad);
                assetList.Add(asset);
            } catch (Exception e) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error has occurred during asset registration: {e.Message}");
                Console.ResetColor();
                return false;
            }
            return true;
        }

        /** program entry point (main method) **/
        public static void Main(string[] args) {
            AssetList assetList = new AssetList();

            // first run
            runProgram(assetList);

            // prompt to run again
            ConsoleKeyInfo keyInfo;
            do {
                Console.WriteLine("Would you like to run the program again and add more items? [Y/n]");
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Y) {
                    Console.WriteLine();
                    runProgram(assetList);
                }
                if (keyInfo.Key == ConsoleKey.N)
                    break;
            } while (keyInfo.Key != ConsoleKey.N || keyInfo.Key != ConsoleKey.Y);

            // print goodbye message
            Console.WriteLine("Goodbye!👋");
        }
    }
}
