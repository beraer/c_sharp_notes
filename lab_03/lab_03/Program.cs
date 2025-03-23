using System;
using System.Collections.Generic;

namespace lab_03
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create two ships
            var shipA = new Ship(maxSpeed: 25, maxContainerCapacity: 3, name: "Poseidon");
            var shipB = new Ship(maxSpeed: 20, maxContainerCapacity: 2, name: "Neptune");

            // create different containers
            var gasContainer = new GasContainer(
                massOfCargo: 100, height: 2, tareWeight: 50, depth: 2,
                maxPayload: 200, isHazard: true
            );

            var liquidContainer = new LiquidContainer(
                massOfCargo: 70, height: 2, tareWeight: 40, depth: 2,
                maxPayload: 100, isHazardous: true
            );

            var fridgeContainer = new RefrigeratedContainer(
                massOfCargo: 50, height: 2, tareWeight: 30, depth: 2,
                maxPayload: 80, productType: "Ice Cream", temperature: -18
            );

            // load cargo
            gasContainer.LoadCargo();
            liquidContainer.LoadCargo();
            fridgeContainer.LoadCargo();

            // load containers onto shipA
            shipA.LoadContainer(gasContainer);
            shipA.LoadContainer(liquidContainer);
            shipA.LoadContainer(fridgeContainer);
            
            shipA.PrintShipInfo();

            // unload cargo from the gas container (but it stays on the ship)
            shipA.UnloadContainer(gasContainer);
            shipA.PrintShipInfo();

            // remove the liquid container from shipA
            shipA.RemoveContainer(liquidContainer);
            shipA.PrintShipInfo();

            // replace the fridge container with a new gas container
            var newGasContainer = new GasContainer(
                massOfCargo: 150, height: 2, tareWeight: 60, depth: 2,
                maxPayload: 200, isHazard: false
            );
            shipA.ReplaceContainer(fridgeContainer, newGasContainer);
            shipA.PrintShipInfo();

            // transfer the newly replaced gas container from shipA to shipB
            shipA.TransferContainer(newGasContainer, shipB);
            shipA.PrintShipInfo();
            shipB.PrintShipInfo();

            // keep console open
            Console.WriteLine("=== Application Finished ===");
            Console.ReadLine();
        }
    }
}
