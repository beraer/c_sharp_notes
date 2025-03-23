namespace lab_03;

public class Ship
{
    public List<Container> _containers = new List<Container>();
    public double MaxSpeed {get; set;}
    public int MaxContainerCapacity {get; set;}
    public string Name {get; set;}

    public Ship(double maxSpeed, int maxContainerCapacity, string name)
    {
        MaxSpeed = maxSpeed;
        MaxContainerCapacity = maxContainerCapacity;
        Name = name;
    }

    public void LoadContainer(Container container)
    {
        if (_containers.Count > MaxContainerCapacity)
        {
            throw new InvalidOperationException("you reached max capacity");
        }
        _containers.Add(container);
        Console.WriteLine($"Container {container.SerialNumber} loaded onto ship {Name}");
    }

    public void LoadContainers(List<Container> containers)
    {
        foreach (var cont in containers)
        {
            LoadContainer(cont);
        }
    }

    public void RemoveContainer(Container container)
    {
        _containers.Remove(container);
    }

    public void UnloadContainer(Container container)
    {
        container.EmptyCargo();
    }
    
    public void ReplaceContainer(Container oldContainer, Container newContainer)
    {
        RemoveContainer(oldContainer);
        LoadContainer(newContainer);
        Console.WriteLine($"Container {oldContainer} replaced with {newContainer.SerialNumber} on ship {Name}.");
    }

    public void TransferContainer(Container container, Ship otherShip)
    {
        _containers.Remove(container);
        otherShip._containers.Add(container);
        Console.WriteLine($"Container {container.SerialNumber} transfered to {otherShip.Name}.");
    }
    
    public void PrintShipInfo()
    {
        Console.WriteLine($"\n=== Ship: {Name} ===");
        Console.WriteLine($"MaxSpeed: {MaxSpeed} knots");
        Console.WriteLine($"MaxContainerCapacity: {MaxContainerCapacity}");
        Console.WriteLine($"Currently loaded containers: {_containers.Count}");
        foreach (var c in _containers)
        {
            Console.WriteLine($"  - {c.SerialNumber} (Type: {c.GetType().Name}, Cargo: {c.MassOfCargo})");
        }
        Console.WriteLine("====================\n");
    }
}