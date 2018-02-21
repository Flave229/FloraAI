using System;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantGenetics
    {
        private PlantCrossOver _crossOver;
        private PlantMutation _mutation;
        private PlantSelection _selection;

        public PlantGenetics(Random randomGenerator, float mutationChance)
        {
            _crossOver = new PlantCrossOver(randomGenerator);
            _mutation = new PlantMutation(randomGenerator, mutationChance);
            _selection = new PlantSelection(randomGenerator);
        }
    }
}
