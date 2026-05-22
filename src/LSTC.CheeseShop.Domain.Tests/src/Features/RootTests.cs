namespace LSTC.CheeseShop.Domain.Tests;

[Trait("Category", "Unit")]
public class RootTests
{
    [Fact]
    public void CreateProduct()
    {
        var root = new Root();
        var result = root.CreateProduct(Guid.NewGuid(), "Test Product", "This is a test product");
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Product", result.Value.Name);
        Assert.Equal("This is a test product", result.Value.Description);
    }

    [Fact]
    public void CreateLocation()
    {
        var root = new Root();
        var result = root.CreateLocation(Guid.NewGuid(), "Test Location", "This is a test location");
        Assert.True(result.IsSuccess);
        Assert.Equal("Test Location", result.Value.Name);
        Assert.Equal("This is a test location", result.Value.Description);
    }
}