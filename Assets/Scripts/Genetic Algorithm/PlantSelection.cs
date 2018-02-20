using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LSystems;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantSelection
    {
        private Random _randomGenerator;

        public PlantSelection(Random randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public List<LSystem> ChooseParents(Dictionary<LSystem, float> plantsAndFitness)
        {
            LSystem firstParent = RouletteWheelChoice(plantsAndFitness);
            LSystem secondParent = RouletteWheelChoice(plantsAndFitness.Where(x => x.Key != firstParent).ToDictionary(x => x.Key, y => y.Value));

            return new List<LSystem>
            {
                firstParent,
                secondParent
            };
        }

        public LSystem RouletteWheelChoice(Dictionary<LSystem, float> plantsAndFitness)
        {
            float fitnessMagnitude = plantsAndFitness.Sum(x => x.Value);
            float randomNumber = (float)_randomGenerator.NextDouble();

            if (fitnessMagnitude == 0.0f)
                return plantsAndFitness.ElementAt((int)(randomNumber * plantsAndFitness.Count)).Key;
            
            float fitnessSum = 0;

            foreach (var plantFitness in plantsAndFitness)
            {
                if (fitnessSum + plantFitness.Value / fitnessMagnitude >= randomNumber)
                    return plantFitness.Key;
            }

            throw new Exception("No Parent could be found. This should not happen");
        }
    }
}
