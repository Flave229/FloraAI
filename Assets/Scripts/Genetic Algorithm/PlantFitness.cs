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
        private Vector3 _sunEnergyWeightings;

        public PlantFitness(ILeafFitness leafFitness)
        {
            _leafFitness = leafFitness;
            _unitBranchVolume = Mathf.PI * Mathf.Pow(0.02f, 2);
            _minimumBranchDiameter = 0.005f;
            Color sunColour = leafFitness.GetSunInformation().Light;
            _sunEnergyWeightings = new Vector3(Mathf.Pow(sunColour.r / (670 / 437.5f) * 4.1f, 2), Mathf.Pow(sunColour.g / (532.5f / 437.5f) * 3, 2), Mathf.Pow(sunColour.b * 2.9f, 2)).normalized;
        }

        public float EvaluateFitness(Plant plant)
        {
            //float fitness = EvaluateUpwardsPhototrophicFitness(plant) * 2;
            //Debug.Log("UpwardsPhototrophicFitness: " + fitness);
            //fitness += EvaluateDynamicPhototrophicFitness(plant);
            //return fitness;
            //float phloemTransportFitness = EvaluatePhloemTransportationFitness(plant);
            //fitness += phloemTransportFitness;
            plant.Fitness = EvaluatePhloemTransportationFitness(plant);
            //Debug.Log("PhloemTransportationFitness: " + fitness);
            plant.Fitness.LeafColour = plant.LindenMayerSystem.GetLeafColor();
            return plant.Fitness.TotalFitness(_sunEnergyWeightings);
        }
        
        public float EvaluateUpwardsPhototrophicFitness(Plant plant)
        {
            List<Leaf> leaves = plant.GeometryStorage.Leaves;
            float fitness = 0;

            foreach (var leaf in leaves)
            {
                fitness += Mathf.Min(leaf.Position.y, 5);
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
            
            TransportEnergyToParent(rootBranch, ref fitness);

            //Clean up to save memory
            plant.GeometryStorage.Delete();

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
                    fitness.CumulativeHeight += childLeaf.Position.y;
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
                fitness.CumulativeHeight += fitnessForChildBranch.CumulativeHeight;
                fitness.BranchesTooThin += fitnessForChildBranch.BranchesTooThin;
                fitness.EnergyLoss += fitnessForChildBranch.EnergyLoss;
            }

            foreach (var childLeaf in branch.ChildLeaves)
            {
                float branchToLeafRelation = 1 - Mathf.InverseLerp(0.02f, 0.06f, branch.Diameter);
                ++fitness.LeafCount;
                fitness.CumulativeHeight += childLeaf.Position.y;
                float leafFitness = _leafFitness.EvaluatePhotosyntheticRate(childLeaf);
                fitness.LeafEnergy += branchToLeafRelation * leafFitness;
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