using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    class PlantFitness
    {
        public float EvaluatePositivePhototrophicFitness(Plant plant)
        {
            List<Vector3> leafPositions = plant.GeometryStorage.LeafPositions;
            float fitness = 0;

            foreach (var leafPosition in leafPositions)
            {
                fitness += leafPosition.y;
            }

            return fitness;
        }
    }
}
