using System.Reflection;
using Tutorial2.Classes;

namespace Tutorial2.Tests;

public class ProductTests
{
    [Fact]
    public void Product_ShouldHaveProperties_WithSpecificTypes()
    {
        // Arrange
        Type type = typeof(Product);

        var expectedProperties = new (string PropertyName, Type ExpectedType)[]
        {
            ("Id", typeof(int)),
            ("Name", typeof(string)),
            ("Price", typeof(double)),
            ("IsActive", typeof(bool)),
            ("CreatedAt", typeof(DateTime)),
            ("Type", typeof(string)),
            ("SerialNumber", typeof(string))
        };

        foreach (var (propertyName, expectedType) in expectedProperties)
        {
            PropertyInfo? propertyInfo = type.GetProperty(propertyName);
            if(propertyInfo is null) Assert.Fail($"There is no {propertyName} property!");
            Assert.Equal(expectedType, propertyInfo.PropertyType);
        }
    }
    
    [Fact]
    public void SerialNumber_ShouldBeFormattedCorrectly()
    {
        PropertyInfo? typeProperty = typeof(Product).GetProperty("Type");
        PropertyInfo? serialNumberProperty = typeof(Product).GetProperty("SerialNumber");

        if (typeProperty is null)
        {
            Assert.Fail("There is no Type property!");
        }
        
        if (serialNumberProperty is null)
        {
            Assert.Fail("There is no SerialNumber property!");
        }
        
        // Arrange
        var product = new Product("name", 2345, true, "technology" );
        typeof(Product).GetProperty("Id")!.SetValue(product, 123);
        typeof(Product).GetProperty("Type")!.SetValue(product, "ELECTRONICS");

        // Act
        string expectedSerialNumber = "PROD-ELECTRONICS-123";

        // Assert
        var serialNumber = product.GetType().GetProperty("SerialNumber");
        Assert.NotNull(serialNumber);
        Assert.Equal(expectedSerialNumber, serialNumber.GetValue(product));
    }
    
    [Fact]
    public void Product_ShouldHave_UniqueId()
    {
        PropertyInfo? idProperty = typeof(Product).GetProperty("Id");

        if (idProperty is null)
        {
            Assert.Fail("There is no Id property!");
        }
        // Arrange
        var product1 = new Product("name", 2345, true, "technology");
        var product2 = new Product("name", 2345, true, "technology");
    
        // Act & Assert
        var id1 = product1.GetType().GetProperty("Id");
        var id2 = product2.GetType().GetProperty("Id");
        Assert.NotEqual(id1?.GetValue(product1), id2?.GetValue(product2));
    }
}