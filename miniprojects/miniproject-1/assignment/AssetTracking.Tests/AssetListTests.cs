using System.Linq;
using Xunit;

namespace miniproject1;
public class AssetListTests {
    // EXTRA: add tests for:
    //        - Print
    //          - FormatColor (private)
    //          - FormatKey   (private)
    //          - FormatValue (private)

    [Fact]
    public void Add_SingleAsset_CountOne() {
        AssetList sut = new AssetList();
        Asset asset = TestUtils.createAsset(TestUtils.AssetType.LaptopComputer);

        sut.Add(asset);
        var count = sut.Count();

        Assert.Equal(1, count);
    }

    [Fact]
    public void Add_MultipleAssets_CountFive() {
        AssetList sut = new AssetList();
        Asset asset = TestUtils.createAsset(TestUtils.AssetType.LaptopComputer);

        foreach (var i in Enumerable.Range(1, 5)) {
            sut.Add(asset);
        }
        var count = sut.Count();

        Assert.Equal(5, count);
    }

    [Fact]
    public void isEmpty_Empty_True() {
        AssetList sut = new AssetList();
        var actual = sut.isEmpty();
        Assert.True(actual);
    }

    [Fact]
    public void isEmpty_Populated_False() {
        AssetList sut = new AssetList();
        Asset asset = TestUtils.createAsset(TestUtils.AssetType.LaptopComputer);

        sut.Add(asset);
        var actual = sut.isEmpty();

        Assert.False(actual);
    }
}
