using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LSystems;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantSelection
    {
        private readonly Random _randomGenerator;

        public PlantSelection(Random randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }
        
        public List<List<ILSystem>> SelectParentPairs(Dictionary<ILSystem, float> plantsAndFitness, int iterations)
        {
            List<List<ILSystem>> parentPairs = new List<List<ILSystem>>();
            for (int i = 0; i < iterations; ++i)
            {
                parentPairs.Add(ChooseParents(plantsAndFitness));
            }
            return parentPairs;
        }

        public List<ILSystem> ChooseParents(Dictionary<ILSystem, float> plantsAndFitness)
        {
            ILSystem firstParent = RouletteWheelChoice(plantsAndFitness);
            ILSystem secondParent = RouletteWheelChoice(plantsAndFitness.Where(x => x.Key != firstParent).ToDictionary(x => x.Key, y => y.Value));

            return new List<ILSystem>
            {
                firstParent,
                secondParent
            };
        }


        public ILSystem RouletteWheelChoice(Dictionary<ILSystem, float> plantsAndFitness)
        {
            float fitnessMagnitude = plantsAndFitness.Sum(x => x.Value > 0 ? x.Value : 0);
            float randomNumber = (float)_randomGenerator.NextDouble();

            if (fitnessMagnitude <= 0.0001f)
                return plantsAndFitness.ElementAt((int)(randomNumber * plantsAndFitness.Count)).Key;
            
            float fitnessSum = 0;

            foreach (var plantFitness in plantsAndFitness)
            {
                float plantFitnessValue = plantFitness.Value > 0 ? plantFitness.Value : 0;
                float normalisedValue = plantFitnessValue / fitnessMagnitude;
                if (fitnessSum + normalisedValue >= randomNumber)
                    return plantFitness.Key;

                fitnessSum += normalisedValue;
            }

            throw new Exception("No Parent could be found. This should not happen");
        }
    }
}