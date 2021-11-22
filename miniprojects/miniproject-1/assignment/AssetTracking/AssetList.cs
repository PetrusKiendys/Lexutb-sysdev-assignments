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
            Console.WriteLine("\nAssets");
            Console.WriteLine(new string('=', columnWidth*2));
            list = list
                .OrderBy(a => (int)a.location)
                .ThenBy(a => a.GetType().Name)
                .ThenBy(a => a.purchaseDate)
                .ToList();
            foreach (var asset in list.Select((item, index) => new {index, item})) {
                FormatColor(asset.item.purchaseDate, asset.index);
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(asset.item)) {
                    string key = descriptor.Name;
                    string value = descriptor.GetValue(asset.item).ToString();
                    Type type = descriptor.PropertyType;
                    if (key != "price" && key != "currency") {       // exclude specific fields from being printed, the printing of 'price' is handled elsewhere
                        value = FormatValue(key, value, type);
                        key = FormatKey(key);
                        Console.WriteLine("  - {0}: {1}", key, value, type);
                    }
                }
            }
            Console.WriteLine(new string('-', columnWidth*2) + "\n");
        }

        private void FormatColor(DateTime purchaseDate, int index) {
            var redDate = purchaseDate.AddMonths(2 * 12 + 9);
            var yellowDate = purchaseDate.AddMonths(2 * 12 + 6);
            Console.ForegroundColor = DateTime.Now >= redDate ? ConsoleColor.Red :
                                      DateTime.Now >= yellowDate ? ConsoleColor.Yellow :
                                      (System.ConsoleColor)(-1);    // don't change color
            Console.Write("Asset #{0}:", index+1);
            Console.ResetColor();
            Console.WriteLine();
        }

        // PERF:  due to string being immutable, assigning new string multiple times is not performant
        //        use of something like StringBuilder or Regex.Replace(...) might be preferable
        private string FormatKey(string key) {
            key = key.Replace("prettyPrice", "Price");                                                       // field 'prettyPrice' is renamed to 'Price'
            key = string.Concat(key.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');   // add spaces before capital letters
            key = key.ToLower();                                                                             // lower case all letters
            key = key[0].ToString().ToUpper() + key.Substring(1);                                            // upper case only the first letter
            key = key.Length == 3 ? key.ToUpper() : key;                                                     // upper case three-character strings like 'cpu' or 'ram'
            return key;
        }

        // TODO: add more conditional branches (later) to accomodate for currently unhandled derived types of Asset (Smartphone, DesktopComputer)
        // XXX Preferably pattern matching in the switch statements should be handled dynamically,
        // XXX and not statically as they currently are. But not sure how to do implement this without compile errors..
        // ANSWER: I think it's because the pattern matching is done on `Type` and not on `Object`.
        private string FormatValue(string key, string value, Type type) {
            TypeCode tc = Type.GetTypeCode(type);
            string result = tc switch {
                TypeCode.Boolean => value switch {
                    "True" => "yes",
                    "False" => "no", },
                TypeCode.Int32 => type.FullName switch {
                    "System.Int32" => key switch {
                        "batteryCapacity" => value + " mAh",
                        "ramSize" => value + " GB",
                        "storageSize" => value + " GB",
                        "weight" => value + "g",        // BONUS: convert and print in kg when applicable
                    },
                    "miniproject1.Asset+Location" => value,
                    "miniproject1.Asset+Currency" => value,
                },
                TypeCode.Double => key switch {
                    "displaySize" => value + "\"",
                    "price" => Math.Round(Convert.ToDouble(value), 2).ToString()
                },
                TypeCode.String => value,
                TypeCode.DateTime => Convert.ToDateTime(value).ToString("yyyy-MM-dd"),
                TypeCode.Empty => throw new ArgumentNullException(nameof(type), "Type is null"),
                _ => throw new NotSupportedException("Type is invalid or unhandled: " + type.FullName),
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
