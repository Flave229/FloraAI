using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            float fitness = LeafEnergy - (BranchCost * 5) /*- ((float)(BranchCount + LeafCount) / 500)*/ - EnergyLoss;

            return fitness;
        }
    }
}
