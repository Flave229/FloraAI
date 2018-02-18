using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantGenetics
    {
        private PlantCrossOver _crossOver;

        public PlantGenetics(Random randomGenerator)
        {
            _crossOver = new PlantCrossOver(randomGenerator);
        }
    }
}
