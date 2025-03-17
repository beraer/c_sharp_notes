namespace lab_03;

public class LiquidContainer : Container, IContainerInterface, IHazardNotifier
{
    public bool IsHazardous {get; set;}
    public LiquidContainer(double massOfCargo, double height, double tareWeight, double depth, int maxPayload, bool isHazardous) : base(massOfCargo, height, tareWeight, depth, maxPayload)
    {
        IsHazardous = isHazardous;
    }

    public override void EmptyCargo()
    {
        base.EmptyCargo();
    }
    

    public override void LoadCargo()
    {
        base.LoadCargo();
    }

    public void Notify()
    {
        //maxpayload 100, mass 70 
        double fiftyPercentCapacity = MaxPayload / 2;
        double ninetyPercentCapacity = MaxPayload / 100 * 90;

        if (IsHazardous == true && MassOfCargo > fiftyPercentCapacity)
        {
            Console.WriteLine("don't bro, haram bro");
        }

        if (IsHazardous == false && MassOfCargo > ninetyPercentCapacity)
        {
            Console.WriteLine("brooooo, it's dangerous bro");
        }
    }
}