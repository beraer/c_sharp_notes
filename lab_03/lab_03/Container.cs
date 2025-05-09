namespace lab_03;

public class Container : IContainerInterface
{
    public double MassOfCargo { get; set; }
    public double Height { get; set; }
    public double TareWeight { get; set; }
    public double Depth { get; set; }
    private static int _IdCounter = 0;
    public int Id { get; set; }
    public static string Type {get;set;}
    public string SerialNumber { get; set; }
    public int MaxPayload {get;set;}

    public Container(double massOfCargo, double height, double tareWeight, double depth, int maxPayload)
    {
        Id = _IdCounter++;
        MassOfCargo = massOfCargo;
        Height = height;
        TareWeight = tareWeight;
        Depth = depth;
        MaxPayload = maxPayload;
        SerialNumber = $"KNO-{Type}-{Id}";
    }

    public virtual void EmptyCargo()
    {
        MassOfCargo = 0;
        Console.WriteLine("Container is Empty");
    }

    public virtual void LoadCargo()
    {
        if (MassOfCargo > MaxPayload)
        {
            throw new InvalidOperationException("you are trying to load more payload than max payload");
        }   
        Console.WriteLine("Loading cargo");
    }

    public override string ToString()
    {
        return $"{SerialNumber} (mass: {MassOfCargo}, maxPayload: {MaxPayload})";
    }
}