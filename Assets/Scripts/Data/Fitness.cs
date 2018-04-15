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

            //Vector3 colourFilter = new Vector3(1 - LeafColour.r, 1 - LeafColour.g, 1 - LeafColour.b);
            //float totalColourEnergy = (colourFilter.x * sunEnergyWeightings.x + colourFilter.y * sunEnergyWeightings.y + colourFilter.z * sunEnergyWeightings.z) / 3;
            //if (totalColourEnergy > 1.8f)
            //    totalColourEnergy = 1.8f - (totalColourEnergy - 1.8f);

            float fitness = /*(totalColourEnergy * 5) +*/ LeafEnergy - (BranchCost) + punishment; /*- Mathf.Pow(((float) (BranchCount + LeafCount) / 2000), 2)*/; //EnergyLoss;
            return fitness;
        }
    }
}
