namespace lab_03;

public abstract class Container : IContainerInterface
{
    public double MassOfCargo { get; set; }
    public double Height { get; set; }
    public double TareWeight { get; set; }
    public double Depth { get; set; }
    private int _IdCounter = 0;
    public static int Id;
    public static string Type;
    public string SerialNumber = $"KNO-{Type}-{Id}";
    public int MaxPayload;
    
    public abstract void EmptyCargo();
    public abstract void LoadCargo();
}