using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    class PlantFitness
    {
        private readonly ILeafFitness _leafFitness;
        private float _unitBranchVolume;
        private float _minimumBranchDiameter;

        public PlantFitness(ILeafFitness leafFitness)
        {
            _leafFitness = leafFitness;
            _unitBranchVolume = Mathf.PI * Mathf.Pow(0.01f, 2);
            _minimumBranchDiameter = 0.001f;
        }

        public float EvaluateFitness(Plant plant)
        {
            //float fitness = EvaluateUpwardsPhototrophicFitness(plant) * 5;
            //Debug.Log("UpwardsPhototrophicFitness: " + fitness);
            //fitness += EvaluateDynamicPhototrophicFitness(plant);
            //return fitness;
            //float phloemTransportFitness = EvaluatePhloemTransportationFitness(plant);
            //fitness += phloemTransportFitness;
            //plant.Fitness = EvaluatePhloemTransportationFitness(plant);
            //Debug.Log("PhloemTransportationFitness: " + fitness);
            //return plant.Fitness.TotalFitness();
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

        public Fitness EvaluatePhloemTransportationFitness(Plant plant)
        {
            Fitness fitness = new Fitness();
            Branch rootBranch = plant.GeometryStorage.GetRootBranch();

            if (rootBranch == null)
                return fitness;
            
            foreach (var childBranch in rootBranch.ChildBranches)
            {
                TransportEnergyToParent(childBranch, ref fitness);
            }

            foreach (var childLeaf in rootBranch.ChildLeaves)
            {
                fitness.LeafCount++;
                fitness.EnergyLoss += Mathf.Max(_leafFitness.EvaluatePhotosyntheticRate(childLeaf), 0);
            }

            return fitness;
        }

        private void TransportEnergyToParent(Branch branch, ref Fitness fitness)
        {
            float branchVolume = Mathf.PI * Mathf.Pow(branch.Diameter, 2);
            fitness.BranchCost += branchVolume * branch.Length;
            ++fitness.BranchCount;

            if (branch.Diameter < _minimumBranchDiameter)
            {
                ++fitness.BranchesTooThin;

                foreach (var childBranch in branch.ChildBranches)
                    TransportEnergyToParent(childBranch, ref fitness);

                foreach (var childLeaf in branch.ChildLeaves)
                {
                    fitness.LeafCount++;
                    fitness.EnergyLoss += Mathf.Max(_leafFitness.EvaluatePhotosyntheticRate(childLeaf), 0);
                }

                return;
            }

            foreach (var childBranch in branch.ChildBranches)
            {
                Fitness fitnessForChildBranch = new Fitness();
                TransportEnergyToParent(childBranch, ref fitnessForChildBranch);
                fitness.LeafEnergy += fitnessForChildBranch.LeafEnergy;
                fitness.BranchCost += fitnessForChildBranch.BranchCost;
                fitness.BranchCount += fitnessForChildBranch.BranchCount;
                fitness.LeafCount += fitnessForChildBranch.LeafCount;
                fitness.BranchesTooThin += fitnessForChildBranch.BranchesTooThin;
                fitness.EnergyLoss += fitnessForChildBranch.EnergyLoss;
            }

            foreach (var childLeaf in branch.ChildLeaves)
            {
                float branchToLeafRelation = 1 - Mathf.InverseLerp(0.01f, 0.06f, branch.Diameter);
                ++fitness.LeafCount;
                fitness.LeafEnergy += branchToLeafRelation * _leafFitness.EvaluatePhotosyntheticRate(childLeaf);
            }

            // Making the assumption that a branch with radius 0.01 is able to support 1 leaf at 100% photosynthetic rate
            // π(r^2)h where r = 0.01 and h = 1 : 0.000314
            if (fitness.LeafEnergy * _unitBranchVolume > branchVolume)
            {
                float previousEnergy = fitness.LeafEnergy;
                fitness.LeafEnergy = branchVolume / _unitBranchVolume;
                fitness.EnergyLoss += Mathf.Max(previousEnergy - fitness.LeafEnergy, 0);
            }

            //Debug.Log("Fitness After Leaf Transport: " + totalEnergy);
            // Subtract the energy the branch needs to survive
            //Debug.Log("Fitness After Branch Cost Deduction: " + totalEnergy);
        }
    }
}