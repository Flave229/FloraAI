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

        public float TotalFitness()
        {
            // Trying to discourage low geometry plants
            if (LeafCount + BranchCount < 200)
                return (LeafCount + BranchCount) - 200;

            if (LeafCount < 20)
                return LeafCount - 20;

            float fitness = LeafEnergy - (BranchCost * 7) - Mathf.Pow(((float) (BranchCount + LeafCount) / 2000), 2); //EnergyLoss;

            return fitness;
        }
    }
}
