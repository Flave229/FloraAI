using System.Collections.Generic;
using Assets.Scripts.Data;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantGenetics
    {
        private readonly TurtlePen _turtlePen;
        private readonly PlantCrossOver _crossOver;
        private readonly PlantMutation _mutation;
        private readonly PlantSelection _selection;
        private readonly PlantFitness _fitness;

        public PlantGenetics(Random randomGenerator, TurtlePen turtlePen, SunInformation sunInformation, float mutationChance)
        {
            _turtlePen = turtlePen;
            _crossOver = new PlantCrossOver(randomGenerator);
            _mutation = new PlantMutation(randomGenerator, mutationChance);
            _selection = new PlantSelection(randomGenerator);
            _fitness = new PlantFitness(new LeafFitness(sunInformation));
        }

        public List<Plant> GenerateChildPopulation(List<Plant> parents)
        {
            Dictionary<ILSystem, float> fitnessPerParent = new Dictionary<ILSystem, float>();

            foreach (Plant plant in parents)
            {
                float fitness = _fitness.EvaluateFitness(plant);
                fitnessPerParent.Add(plant.LindenMayerSystem, fitness);
            }

            List<List<ILSystem>> parentPairs = _selection.SelectParentPairs(fitnessPerParent, 50);

            List<Plant> childPlants = new List<Plant>();
            foreach (List<ILSystem> parentPair in parentPairs)
            {
                //RuleSet childRuleSet = _crossOver.CrossOver(parentPair[0].GetRuleSet(), parentPair[1].GetRuleSet());
                RuleSet childRuleSet = _crossOver.CrossOverV2(parentPair[0].GetRuleSet(), parentPair[1].GetRuleSet());
                childRuleSet = _mutation.Mutate(childRuleSet);
                childPlants.Add(new Plant(new LSystem(childRuleSet, "A"), _turtlePen, new PersistentPlantGeometryStorage(), Vector3.zero));
            }

            return childPlants;
        }

        public KeyValuePair<Plant, float> GetFittestPlant(List<Plant> parents)
        {
            Plant fittestPlant = parents[0];
            float maxFitnessValue = 0;
            foreach (Plant plant in parents)
            {
                float fitness = _fitness.EvaluateFitness(plant);

                if (fitness > maxFitnessValue)
                {
                    maxFitnessValue = fitness;
                    fittestPlant = plant;
                }
            }

            return new KeyValuePair<Plant, float>(fittestPlant, maxFitnessValue);
        }
    }
}
