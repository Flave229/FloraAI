using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    class PlantFitness
    {
        private readonly Vector3 _lightPosition;

        public PlantFitness(Vector3 lightPosition)
        {
            _lightPosition = lightPosition;
        }

        public float EvaluateFitness(Plant plant)
        {
            float fitness = EvaluateUpwardsPhototrophicFitness(plant);
            fitness += EvaluateDynamicPhototrophicFitness(plant);
            return fitness;
        }

        public float EvaluateUpwardsPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                fitness += leaf.Position.y * 3;
            }

            return fitness;
        }

        public float EvaluateDynamicPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                Vector3 toSun = Vector3.Normalize(_lightPosition - leaf.Position);
                fitness += Mathf.Max(Vector3.Dot(Vector3.Normalize(leaf.RightVector), toSun), 0);
            }

            return fitness;
        }
    }
}