using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace Assets.Scripts.Data
{
    public class Fitness
    {
        public float BranchCost { get; set; }
        public float LeafEnergy { get; set; }
        public float EnergyLoss { get; set; }
        public int BranchesTooThin { get; set; }
        public int BranchCount { get; set; }
        public int LeafCount { get; set; }
        public float CumulativeHeight { get; set; }
        public Color LeafColour { get; set; }

        public float TotalFitness(Vector3 sunEnergyWeightings)
        {
            //// Trying to discourage low geometry plants
            //if (LeafCount + BranchCount < 200)
            //    //return ((LeafCount + BranchCount) / 10) - 20;
            //    return (LeafCount + BranchCount) - 200;
            //    //return 0;

            float punishment = 0;
            if (CumulativeHeight / LeafCount < 1)
                punishment += ((CumulativeHeight / LeafCount) - 1) * 10;

            if (LeafCount < 100)
                punishment += LeafCount - 100;

            //if (LeafCount < 20)
            //    return LeafCount - 20;
            //    //return 0;
            float totalColourEnergy = CalculateColourEnergyFactor(sunEnergyWeightings);

            float fitness = totalColourEnergy + LeafEnergy - (BranchCost * 5) + punishment; /*- Mathf.Pow(((float) (BranchCount + LeafCount) / 2000), 2)*/; //EnergyLoss;
            return fitness;
        }

        public float CalculateColourEnergyFactor(Vector3 sunEnergyWeightings)
        {
            Vector3 energyWeightings = LeafEnergyWeightings(sunEnergyWeightings);
            float totalColourEnergy = (energyWeightings.x + energyWeightings.y + energyWeightings.z);
            //float minimumEnergy = Mathf.Min(energyWeightings.x, energyWeightings.y, energyWeightings.z);
            //totalColourEnergy -= minimumEnergy * 2;
            //if (totalColourEnergy > 0.5f)
            //    totalColourEnergy = 0.5f - (totalColourEnergy - 0.5f);


            return totalColourEnergy * 10;
        }

        public Vector3 LeafEnergyWeightings(Vector3 sunEnergyWeightings)
        {
            Vector3 colour = new Vector3(LeafColour.r, LeafColour.g, LeafColour.b);
            //float colourMagnitude = colour.x + colour.y + colour.z;
            //if (colourMagnitude > 1.8f)
            //{
            //    float newMagnitude = 1.8f;
            //    float difference = newMagnitude / colourMagnitude;
            //    colour = new Vector3(colour.x * difference, colour.y * difference, colour.z * difference);
            //}
            
            Vector3 colourFilter = new Vector3(1 - colour.x, 1 - colour.y, 1 - colour.z);
            if (colourFilter.x <= colourFilter.y && colourFilter.x <= colourFilter.z)
                colourFilter.x *= -1;
            else if (colourFilter.y <= colourFilter.z)
                colourFilter.y *= -1;
            else
                colourFilter.z *= -1;

            return new Vector3(colourFilter.x * sunEnergyWeightings.x, colourFilter.y * sunEnergyWeightings.y, colourFilter.z * sunEnergyWeightings.z);
        }
    }
}