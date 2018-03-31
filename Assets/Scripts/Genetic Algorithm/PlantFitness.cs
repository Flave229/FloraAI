using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    class PlantFitness
    {
        private readonly ILeafFitness _leafFitness;
        private float _unitBranchDiameter;
        private float _minimumBranchVolume;

        public PlantFitness(ILeafFitness leafFitness)
        {
            _leafFitness = leafFitness;
            _unitBranchDiameter = Mathf.PI * Mathf.Pow(0.01f, 2);
            _minimumBranchVolume = Mathf.PI * Mathf.Pow(0.001f, 2);
        }


        public float EvaluateFitness(Plant plant)
        {
            float fitness = EvaluateUpwardsPhototrophicFitness(plant);
            //Debug.Log("UpwardsPhototrophicFitness: " + fitness);
            fitness += EvaluateDynamicPhototrophicFitness(plant);
            //float phloemTransportFitness = EvaluatePhloemTransportationFitness(plant);
            //fitness += phloemTransportFitness;
            //float fitness = EvaluatePhloemTransportationFitness(plant);
            //Debug.Log("PhloemTransportationFitness: " + fitness);
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

            return fitness;
        }

        public float EvaluateDynamicPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                fitness += _leafFitness.EvaluatePhotosyntheticRate(leaf) * 2;
            }

            return fitness;
        }

        public float EvaluatePhloemTransportationFitness(Plant plant)
        {
            Branch rootBranch = plant.GeometryStorage.GetRootBranch();
            if (rootBranch == null)
                return 0;

            float totalEnergy = 0;

            foreach (var childBranch in rootBranch.ChildBranches)
            {
                totalEnergy += TransportEnergyToParent(childBranch);
            }
            
            return totalEnergy;
        }

        private float TransportEnergyToParent(Branch branch)
        {
            float branchVolume = Mathf.PI * Mathf.Pow(branch.Diameter, 2);

            if (branchVolume < _minimumBranchVolume)
                return -(branchVolume * branch.Length);

            float totalEnergy = 0;
            
            foreach (var childBranch in branch.ChildBranches)
            {
                totalEnergy += TransportEnergyToParent(childBranch);
            }

            foreach (var childLeaf in branch.ChildLeaves)
            {
                totalEnergy += _leafFitness.EvaluatePhotosyntheticRate(childLeaf);
            }

            // Making the assumption that a branch with radius 0.01 is able to support 1 leaf at 100% photosynthetic rate
            // π(r^2)h where r = 0.01 and h = 1 : 0.000314
            if (totalEnergy * _unitBranchDiameter > branchVolume)
                totalEnergy = branchVolume / _unitBranchDiameter;

            //Debug.Log("Fitness After Leaf Transport: " + totalEnergy);
            // Subtract the energy the branch needs to survive
            totalEnergy -= branchVolume * branch.Length;
            //Debug.Log("Fitness After Branch Cost Deduction: " + totalEnergy);

            return totalEnergy;
        }
    }
}