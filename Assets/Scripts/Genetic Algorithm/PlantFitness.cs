using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    class PlantFitness
    {
        private readonly SunInformation _sunInformation;

        public PlantFitness(SunInformation sunInformation)
        {
            _sunInformation = sunInformation;
        }

        public float EvaluateFitness(Plant plant)
        {
            float fitness = EvaluateUpwardsPhototrophicFitness(plant);
            fitness += EvaluateDynamicPhototrophicFitness(plant);
            fitness += EvaluateGeometryEquilibriumFitness(plant);
            return fitness;
        }

        public float EvaluateUpwardsPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                fitness += leaf.Position.y;
            }

            return fitness * 3;
        }

        public float EvaluateDynamicPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                //Vector3 summerToSun = Quaternion.Euler(0, (float) _sunInformation.Azimuth, (float)_sunInformation.SummerAltitude) * new Vector3(1, 0, 0);
                //Vector3 winterToSun = Quaternion.Euler(0, (float) _sunInformation.Azimuth, (float)_sunInformation.WinterAltitude) * new Vector3(1, 0, 0);
                Vector3 summerToSun = Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0);
                Vector3 winterToSun = Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0);

                fitness += Mathf.Max(Vector3.Dot(Vector3.Normalize(leaf.Normal), summerToSun), 0);
                fitness += Mathf.Max(Vector3.Dot(Vector3.Normalize(leaf.Normal), winterToSun), 0);
            }

            return fitness;
        }

        public float EvaluateGeometryEquilibriumFitness(Plant plant)
        {
            int leafCount = plant.GeometryStorage.Leaves.Count;
            int branchCount = plant.GeometryStorage.Branches.Count;

            return (float)(leafCount - branchCount) / 10;
        }
    }
}