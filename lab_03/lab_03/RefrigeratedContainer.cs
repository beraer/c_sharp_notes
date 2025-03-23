namespace lab_03;

public class RefrigeratedContainer : Container, IContainerInterface
{
    public string ProductType { get; set; }
    public double ContainerTemperature { get; set; }
    public string CurrentProduct {get;set;}

    private Dictionary<string, double> _productsAndTemperatures = new()
    {
        { "Banana", 13.3 },
        { "Chocolate", 18 },
        {"Fish", 2},
        {"Meat", -15},
        {"Ice Cream", -18},
        {"Froze Pizza", -30},
        {"Cheese", 7.2},
        {"Sausages", 5},
        {"Butter", 20.15},
        {"Eggs", 19}
    };

    public RefrigeratedContainer(double massOfCargo, double height, double tareWeight, double depth, int maxPayload, string productType, double temperature) : base(massOfCargo, height, tareWeight, depth, maxPayload)
    {
        ProductType = productType;
        ContainerTemperature = temperature;
        CurrentProduct = productType;
    }

    public void loadCargo(string productName)
    {
        if (!_productsAndTemperatures.ContainsKey(productName))
        {
            Console.WriteLine("there is no product with name " + productName);
        }
        
        if (productName != ProductType)
        {
            Console.WriteLine("you can't load different products");
        }

        if (ContainerTemperature < _productsAndTemperatures[productName])
        {
            Console.WriteLine("container's temperature is less than necessary temperature");
        }
    }
}