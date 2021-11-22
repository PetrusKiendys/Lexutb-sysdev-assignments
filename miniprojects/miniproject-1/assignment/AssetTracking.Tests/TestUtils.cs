using System;

namespace miniproject1;
static class TestUtils {
    public enum AssetType { DesktopComputer, LaptopComputer, Smartphone }
    
    internal static Asset createAsset(AssetType assetType) => assetType switch {
        AssetType.DesktopComputer => throw new NotImplementedException($"Generating {assetType} is not yet supported!"),
        AssetType.Smartphone => throw new NotImplementedException($"Generating {assetType} is not yet supported!"),
        AssetType.LaptopComputer => createAssetLaptopComputer(),
        _ => throw new ArgumentException("Unable to create asset with input: " + nameof(assetType))
    };

    // sources:
    //   https://www.laptopmag.com/reviews/dell-xps-13-2020
    //   https://www.pcmag.com/reviews/dell-xps-13-9310
    private static Asset createAssetLaptopComputer() {
        string name = "XPS 13";
        DateTime purchaseDate = DateTime.Now;
        double price = 69.99;
        Asset.Location location = Asset.Location.USA;
        string brand = "Dell";
        int weight = 1300;
        string cpu = "Intel Core i7-1065G7";
        string gpu = "Intel Iris Plus";
        string ram = "unspecified";
        int ramSize = 16;
        string storage = "M.2 NVMe SSD";
        int storageSize = 512;
        string displayType = "OLED";
        double displaySize = 13.4;
        string batteryType = "Li-ion";
        int batteryCapacity = 7435;
        bool webcam = true;
        bool trackpad = true;
        return new LaptopComputer(name, purchaseDate, price,
                                  location, brand, weight,
                                  cpu, gpu, ram, ramSize,
                                  storage, storageSize,
                                  displayType, displaySize,
                                  batteryType, batteryCapacity,
                                  webcam, trackpad);
    }
}
