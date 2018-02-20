using System;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantGenetics
    {
        private PlantCrossOver _crossOver;
        private PlantMutation _mutation;

        public PlantGenetics(Random randomGenerator, float mutationChance)
        {
            _crossOver = new PlantCrossOver(randomGenerator);
            _mutation = new PlantMutation(randomGenerator, mutationChance);
        }
    }
}
