using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            //var threads = new List<Thread>();
            //for (int i = 0; i < iterations; ++i)
            //{
            //    threads.Add(new Thread(x =>
            //    {
            //        parentPairs.Add(ChooseParents(plantsAndFitness));
            //    }));
            //    threads[threads.Count - 1].Start();
            //}
            //foreach (var thread in threads)
            //{
            //    thread.Join();
            //}
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
            float lowestFitness = plantsAndFitness.Min(x => x.Value);
            float fitnessMagnitude = plantsAndFitness.Sum(x => x.Value - lowestFitness);
            float randomNumber = (float)_randomGenerator.NextDouble();

            if (fitnessMagnitude <= 0.0001f)
                return plantsAndFitness.ElementAt((int)(randomNumber * plantsAndFitness.Count)).Key;
            
            float fitnessSum = 0;

            foreach (var plantFitness in plantsAndFitness)
            {
                float plantFitnessValue = plantFitness.Value - lowestFitness;
                float normalisedValue = plantFitnessValue / fitnessMagnitude;
                if (fitnessSum + normalisedValue >= randomNumber)
                    return plantFitness.Key;

                fitnessSum += normalisedValue;
            }

            throw new Exception("No Parent could be found. This should not happen. The end fitness sum was " + fitnessSum + ". The chosen random number was " + randomNumber + ". The magnitude of all fitness values was " + fitnessMagnitude);
        }
    }
}