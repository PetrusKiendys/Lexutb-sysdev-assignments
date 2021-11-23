using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace miniproject1 {
    public class AssetList {
        List<Asset> list;
        int columnWidth = 15;

        public AssetList() {
            list = new List<Asset>();
        }

        public void Add(Asset asset) {
            list.Add(asset);
        }

        public void Print() {
            // print header
            Console.WriteLine("\nAssets");
            Console.WriteLine(new string('=', columnWidth*2));

            // order list
            list = list
                .OrderBy(a => (int)a.location)
                .ThenBy(a => a.GetType().Name)
                .ThenBy(a => a.purchaseDate)
                .ToList();

            // print asset index & type
            foreach (var asset in list.Select((item, index) => new {index, item})) {
                PrintAssetIndex(asset.item.purchaseDate, asset.index);
                PrintAssetType(asset.item.GetType().Name);

                // print name and value for every property of the asset
                // formatting is applied to improve readability for the end-user
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(asset.item)  // sort output order of printing properties
                .Sort(new string[]{ "name", "purchaseDate", "prettyPrice", "location",              //   by Asset props, then
                                    "brand", "weight",                                              //   by ElectronicDevice props, then
                                    "cpu", "gpu", "ram", "ramSize", "storage", "storageSize",       //   by Computer props, then
                                    "displayType", "displaySize", "batteryType", "batteryCapacity", //   by PortableComputer props, then
                                    "platform", "frontCameraResolution", "backCameraResolution",    //   by Smartphone props, then
                                    "psu", "trackpad", "webcam" })) {                               //   by DesktopComputer and LaptopComputer props
                    string key = descriptor.Name;
                    string value = descriptor.GetValue(asset.item).ToString();
                    Type type = descriptor.PropertyType;
                    if (key != "price" && key != "currency") {       // exclude specific fields from being printed (the printing of 'price' is handled elsewhere)
                        value = FormatValue(key, value, type);
                        key = FormatKey(key);
                        Console.WriteLine("  - {0}: {1}", key, value, type);
                    }
                }
            }
            // print footer
            Console.WriteLine(new string('-', columnWidth*2) + "\n");
        }

        private void PrintAssetType(string assetType) {
            assetType = string.Concat(assetType.Select(c => Char.IsUpper(c) ? " " + c : c.ToString()));
            Console.WriteLine("  {0}", assetType);
        }

        private void PrintAssetIndex(DateTime purchaseDate, int index) {
            var redDate = purchaseDate.AddMonths(2 * 12 + 9);
            var yellowDate = purchaseDate.AddMonths(2 * 12 + 6);                            // conditionally change console color:
            Console.ForegroundColor = DateTime.Now >= redDate ? ConsoleColor.Red :          //  - change color to red
                                      DateTime.Now >= yellowDate ? ConsoleColor.Yellow :    //  - change color to yellow
                                      (System.ConsoleColor)(-1);                            //  - don't change color
            Console.Write($"Asset #{index+1}:");                                            // print asset index
            Console.ResetColor();
            Console.WriteLine();
        }

        // PERF:  due to string being immutable, assigning new string multiple times is not performant
        //        use of something like StringBuilder or Regex.Replace(...) might be preferable
        private string FormatKey(string key) {
            key = key.Replace("prettyPrice", "Price");                                                       // field 'prettyPrice' is renamed to 'Price'
            key = string.Concat(key.Select(c => Char.IsUpper(c) ? " " + c : c.ToString())).TrimStart(' ');   // add spaces before capital letters
            key = key.ToLower();                                                                             // lower case all letters
            key = key[0].ToString().ToUpper() + key.Substring(1);                                            // upper case only the first letter
            key = string.Join(" ", key.Split(' ').Select(w => w.Length == 3 ? w.ToUpper() : w));             // upper case three-character words (substrings) like 'cpu' or 'ram'
            return key;
        }

        private string FormatValue(string key, string value, Type type) {
            TypeCode tc = Type.GetTypeCode(type);
            string result = tc switch {
                TypeCode.Int32 => type.FullName switch {
                    "System.Int32" => key switch {
                        "batteryCapacity" => value + " mAh",
                        "ramSize" => value + " GB",
                        "storageSize" => value + " GB",
                        "weight" => value + "g",        // BONUS: convert and print in kg when applicable
                        _ => value
                    },
                    "miniproject1.Asset+Location" => value,
                    "miniproject1.Asset+Currency" => value,
                },
                TypeCode.Double => key switch {
                    "displaySize" => value + "\"",
                    "price" => Math.Round(Convert.ToDouble(value), 2).ToString(),
                    "frontCameraResolution" or "backCameraResolution" => value + " MP",
                    _ => value
                },
                TypeCode.Boolean => value switch {
                    "True" => "yes",
                    "False" => "no", },
                TypeCode.String => value,
                TypeCode.DateTime => Convert.ToDateTime(value).ToString("yyyy-MM-dd"),
                TypeCode.Empty => throw new ArgumentNullException(nameof(type), "Type is null"),
                _ => throw new NotSupportedException($"Type is invalid or unhandled: {type.FullName}")
            };
            return result;
        }

        public bool isEmpty() {
            return !list.Any();
        }

        public int Count() {
            return list.Count;
        }
    }
}
