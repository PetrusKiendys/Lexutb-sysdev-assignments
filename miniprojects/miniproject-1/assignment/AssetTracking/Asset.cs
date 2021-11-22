using System;

namespace miniproject1 {
    public class Smartphone : PortableComputer {
        public Smartphone(string name, DateTime purchaseDate, double price, Location location, string brand, int weight, string cpu, string gpu, string ram, int ramSize, string storage, int storageSize, string displayType, double displaySize, string batteryType, int batteryCapacity, string platform, double frontCameraResolution, double backCameraResolution)
        : base(name, purchaseDate, price, location, brand, weight, cpu, gpu, ram, ramSize, storage, storageSize, displayType, displaySize, batteryType, batteryCapacity) {
            this.platform = platform;
            this.frontCameraResolution = frontCameraResolution;
            this.backCameraResolution = backCameraResolution;
        }
        public string platform { get; set; }                // OS platform (eg. Android, iOS)
        public double frontCameraResolution { get; set; }   // resolution of front camera (in megapixels)
        public double backCameraResolution { get; set; }    // resolution of back camera (in megapixels)
    }

    public class LaptopComputer : PortableComputer {
        public LaptopComputer(string name, DateTime purchaseDate, double price, Location location, string brand, int weight, string cpu, string gpu, string ram, int ramSize, string storage, int storageSize, string displayType, double displaySize, string batteryType, int batteryCapacity, bool webcam, bool trackpad)
        : base(name, purchaseDate, price, location, brand, weight, cpu, gpu, ram, ramSize, storage, storageSize, displayType, displaySize, batteryType, batteryCapacity) {
            this.webcam = webcam;
            this.trackpad = trackpad;
        }
        public bool webcam { get; set; }            // presence of built-in webcam
        public bool trackpad { get; set; }          // presence of built-in trackpad
    }

    public class DesktopComputer : Computer {
        public DesktopComputer(string name, DateTime purchaseDate, double price, Location location, string brand, int weight, string cpu, string gpu, string ram, int ramSize, string storage, int storageSize, string psu)
        : base(name, purchaseDate, price, location, brand, weight, cpu, gpu, ram, ramSize, storage, storageSize) {
            this.psu = psu;
        }
        public string psu { get; set; }             // name of PSU component
    }

    public abstract class PortableComputer : Computer {
        internal PortableComputer(string name, DateTime purchaseDate, double price, Location location, string brand, int weight, string cpu, string gpu, string ram, int ramSize, string storage, int storageSize, string displayType, double displaySize, string batteryType, int batteryCapacity)
        : base(name, purchaseDate, price, location, brand, weight, cpu, gpu, ram, ramSize, storage, storageSize) {
            this.displayType = displayType;
            this.displaySize = displaySize;
            this.batteryType = batteryType;
            this.batteryCapacity = batteryCapacity;
        }
        public string displayType { get; set; }     // type of display (eg. LCD, OLED, AMOLED, etc.)
        public double displaySize { get; set; }     // size of display (in inches)
        public string batteryType { get; set; }     // type of battery (Li-ion, Li-Po, NiMH, etc.)
        public int batteryCapacity { get; set; }    // battery capacity (in mAh)
    }

    public abstract class Computer : ElectronicDevice {
        internal Computer(string name, DateTime purchaseDate, double price, Location location, string brand, int weight, string cpu, string gpu, string ram, int ramSize, string storage, int storageSize)
        : base(name, purchaseDate, price, location, brand, weight) {
            this.cpu = cpu;
            this.gpu = gpu;
            this.ram = ram;
            this.ramSize = ramSize;
            this.storage = storage;
            this.storageSize = storageSize;
        }
        public string cpu { get; set; }             // name of CPU component
        public string gpu { get; set; }             // name of GPU component
        public string ram { get; set; }             // name of RAM component
        public int ramSize { get; set; }            // size of RAM component (in GB)
        public string storage { get; set; }         // name of storage component
        public int storageSize { get; set; }        // size of storage component (in GB) using decimal notation:
                                                    //   https://www.ibm.com/docs/en/storage-insights?topic=overview-units-measurement-storage-data
    }

    public abstract class ElectronicDevice : Asset {
        internal ElectronicDevice(string name, DateTime purchaseDate, double price, Location location, string brand, int weight)
            : base(name, purchaseDate, price, location) {
            this.brand = brand;
            this.weight = weight;
        }
        public string brand { get; set; }           // name of brand
        public int weight { get; set; }             // weight (in grams)
    }

    /// <summary>Represents an asset.</summary>
    /// <param name="name">The name of the asset</param>
    /// <param name="purchaseDate">The date of purchase of the asset</param>
    /// <param name="price">The original price of the asset (in USD)</param>
    /// <param name="location">The location of the asset</param>
    public abstract class Asset {
        internal Asset(string name, DateTime purchaseDate, double price, Location location) {
            this.name = name;
            this.purchaseDate = purchaseDate;
            this.price = getLocalPrice(price, location);    // assign local price of asset based on location
            this.location = location;
            this.currency = (Currency)location;             // assign currency code based on location
            this.prettyPrice = $"{this.price} {currency}";  // format the price for printing
        }

        private double getLocalPrice(double price, Location location) {
            const double XRATE_SEK = 8.99;
            const double XRATE_JPY = 114.02;
            double result = location switch {
                Location.USA => price,
                Location.Sweden => price*XRATE_SEK,
                Location.Japan => price*XRATE_JPY,
                _ => throw new NotSupportedException("Incorrect location provided: " + (Location)location),
            };
            return Math.Round(result, 2);
        }

        public enum Location { USA=1, Sweden=2, Japan=3 }   // fixed location values
        public enum Currency { USD=1, SEK=2, JPY=3 }        // fixed currency code values
        public string name { get; set; }                    // (model) name of asset
        public DateTime purchaseDate { get; set; }          // date of purchase
        public double price { get; set; }                   // price (in local currency)
        public string prettyPrice { get; set; }             // price (in local currency) incl. currency code
        public Location location { get; set; }              // location of asset
        public Currency currency { get; set; }              // currency code of asset
    }
}
