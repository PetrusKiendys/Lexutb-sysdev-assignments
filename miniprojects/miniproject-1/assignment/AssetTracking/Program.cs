using System;
using System.Reflection;
using System.Linq;
using System.ComponentModel;

namespace miniproject1 {
    public static class Program {
        /** run the main application **/
        private static void runProgram(AssetList assetList) {
            var assetTypes = (from type in Assembly.GetAssembly(typeof(Asset)).GetTypes()
                              where type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Asset))
                              orderby type.Name ascending
                              select type).ToArray();
            while(true) {
                Console.WriteLine("Press Enter to add a new asset or \'q\' to exit");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Q)
                    break;
                else if (keyInfo.Key == ConsoleKey.Enter) {
                    Console.WriteLine("\nPlease enter asset details.");

                    // select asset type
                    string selectionPrompt = InputManager.AssetTypePrompt(assetTypes);
                    Type selectedAssetType = InputManager.AssetTypeSelector(assetTypes, selectionPrompt);

                    // create new asset
                    Asset asset = (Asset)Activator.CreateInstance(selectedAssetType);

                    // populate asset with data
                    populateWithInput(asset);
                    populateDynamically(asset);

                    // add asset to list
                    assetList.Add(asset);

                    // print success message
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Successfully registered asset!");
                    Console.ResetColor();
                    Console.WriteLine();
                }
            }

            // print assetList if it's not empty
            if (!assetList.isEmpty()) {
                assetList.Print();
            }
        }

        private static void populateDynamically(Asset asset) {
            asset.price = asset.getLocalPrice(asset.price, asset.location);
            asset.currency = (Asset.Currency)asset.location;
            asset.prettyPrice = $"{asset.price} {asset.currency}";
        }

        // EXTRA: fuzzy string match when sorting, so that similarly named properties are matched with a single string,
        //        eg. "ram" would match both "ram" and "ramSize"
        //        https://en.wikipedia.org/wiki/Approximate_string_matching
        //        https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.propertydescriptorcollection.sort?view=net-6.0
        //          https://docs.microsoft.com/en-us/dotnet/api/system.collections.icomparer?view=net-6.0
        private static void populateWithInput(Asset asset) {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(asset)       // set input order by sorting PropertyDescriptorCollection:
            .Sort(new string[]{ "name", "purchaseDate", "price", "location",                    //   by Asset props, then
                                "brand", "weight",                                              //   by ElectronicDevice props, then
                                "cpu", "gpu", "ram", "ramSize", "storage", "storageSize",       //   by Computer props, then
                                "displayType", "displaySize", "batteryType", "batteryCapacity", //   by PortableComputer props, then
                                "platform", "frontCameraResolution", "backCameraResolution",    //   by Smartphone props, then
                                "psu", "trackpad", "webcam" })) {                               //   by DesktopComputer and LaptopComputer props
                Type type = descriptor.PropertyType;
                string key = descriptor.Name;
                dynamic value = type.FullName switch {
                    "System.String" => key switch {
                        "name" => InputManager.ConvertInputToString("Asset name: "),
                        "brand" => InputManager.ConvertInputToString("Brand: "),
                        "prettyPrice" => default,
                        "cpu" => InputManager.ConvertInputToString("CPU (name): "),
                        "gpu" => InputManager.ConvertInputToString("GPU (name): "),
                        "ram" => InputManager.ConvertInputToString("RAM (name): "),
                        "psu" => InputManager.ConvertInputToString("PSU (name): "),
                        "storage" => InputManager.ConvertInputToString("Storage (name): "),
                        "displayType" => InputManager.ConvertInputToString("Display type: "),
                        "batteryType" => InputManager.ConvertInputToString("Battery type: "),
                        "platform" => InputManager.ConvertInputToString("OS platform: "),
                        _ => throw new NotSupportedException($"{type.FullName} with key '{key}' is not supported!") },
                    "System.Int32" => key switch {
                        "weight" => InputManager.ConvertInputToInt("Weight (g): "),
                        "ramSize" => InputManager.ConvertInputToInt("RAM size (GB): "),
                        "storageSize" => InputManager.ConvertInputToInt("Storage size (GB): "),
                        "batteryCapacity" => InputManager.ConvertInputToInt("Battery capacity (mAh): "),
                        _ => throw new NotSupportedException($"{type.FullName} with key '{key}' is not supported!") },
                    "System.Double" => key switch {
                        "price" => InputManager.ConvertInputToDouble("Price (in USD): "),
                        "displaySize" => InputManager.ConvertInputToDouble("Display size (inches): "),
                        "frontCameraResolution" => InputManager.ConvertInputToDouble("Front camera resolution (MP): "),
                        "backCameraResolution" => InputManager.ConvertInputToDouble("Back camera resolution (MP): "),
                        _ => throw new NotSupportedException($"{type.FullName} with key '{key}' is not supported!") },
                    "System.Boolean" => key switch {
                        "webcam" => InputManager.ConvertInputToBool("Has webcam [Y/n]? "),
                        "trackpad" => InputManager.ConvertInputToBool("Has trackpad [Y/n]? "),
                        _ => throw new NotSupportedException($"{type.FullName} with key '{key}' is not supported!") },
                    "System.DateTime" => key switch {
                        "purchaseDate" => InputManager.ConvertInputToDateTime("Date of purchase: "),
                        _ => throw new NotSupportedException($"{type.FullName} with key '{key}' is not supported!") },
                    "miniproject1.Asset+Location" => InputManager.ConvertInputToLocation("Location of asset (1) USA, (2) Sweden, (3) Japan: "),
                    "miniproject1.Asset+Currency" => default,
                    null => throw new ArgumentNullException(nameof(type), "Type is null"),
                    _ => throw new NotSupportedException($"Type is invalid or unhandled: {type.FullName}")
                };
                descriptor.SetValue(asset, value);
            }
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
