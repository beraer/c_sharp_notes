using System.ComponentModel;

namespace lab_03;

public class GasContainer : Container, IHazardNotifier, IContainerInterface
{
    public bool IsHazard;
    public GasContainer(double massOfCargo, double height, double tareWeight, double depth, int maxPayload, bool isHazard) : base(massOfCargo, height, tareWeight, depth, maxPayload)
    {
        IsHazard = isHazard;
    }

    public override void EmptyCargo()
    {
        MassOfCargo = MassOfCargo * 5 / 100;
        base.EmptyCargo();
    }

    public override void LoadCargo()
    {
        base.LoadCargo();
    }

    public void Notify()
    {
        if (IsHazard == true)
        {
            Console.WriteLine("This cargo '" + SerialNumber + "' is hazardous");
        }   
    }
}