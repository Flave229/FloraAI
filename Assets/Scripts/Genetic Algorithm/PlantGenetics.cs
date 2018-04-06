using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Data;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;
using Random = System.Random;
using System.Linq;
using System.Threading;
using Assets.Scripts.Common;

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
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Dictionary<ILSystem, float> fitnessPerParent = new Dictionary<ILSystem, float>(parents.Count);
            Tuple<ILSystem, float>[] fitnessPerParent = new Tuple<ILSystem, float>[parents.Count];

            var threads = new List<Thread>();
            for (int i = 0; i < parents.Count; ++i)
            {
                threads.Add(new Thread(x =>
                {
                    object[] threadInput = (object[]) x;
                    int index = (int)threadInput[0];
                    Tuple<ILSystem, float>[] tupleList = (Tuple<ILSystem, float>[]) threadInput[1];
                    Plant parentPlant = (Plant)threadInput[2];
                    float fitness = _fitness.EvaluateFitness(parentPlant);
                    tupleList[index] = new Tuple<ILSystem, float>(parentPlant.LindenMayerSystem, fitness);
                }));
                threads[threads.Count - 1].Start(new object[] { i, fitnessPerParent, parents[i] });
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
            UnityEngine.Debug.Log("Evaluating Fitness for all Parents took " + timer.ElapsedMilliseconds + " milliseconds");
            timer.Reset();
            timer.Start();

            List<List<ILSystem>> parentPairs = _selection.SelectParentPairs(fitnessPerParent.ToList(), 50);
            UnityEngine.Debug.Log("Selecting Parents took " + timer.ElapsedMilliseconds + " milliseconds");
            timer.Reset();
            timer.Start();

            List<Plant> childPlants = new List<Plant>();
            //threads.Clear();
            for (int i = 0; i < parentPairs.Count; ++i)
            {
                //threads.Add(new Thread(x =>
                //{
                    //List<ILSystem> currentParentPair = (List<ILSystem>) x;
                    //RuleSet childRuleSet = _crossOver.CrossOver(parentPair[0].GetRuleSet(), parentPair[1].GetRuleSet());
                    RuleSet childRuleSet = _crossOver.CrossOverV2(parentPairs[i][0].GetRuleSet(), parentPairs[i][1].GetRuleSet());
                    //RuleSet childRuleSet = _crossOver.CrossOverV2(currentParentPair[0].GetRuleSet(), currentParentPair[1].GetRuleSet());
                    childRuleSet = _mutation.Mutate(childRuleSet);
                    childPlants.Add(new Plant(new LSystem(childRuleSet, "A"), _turtlePen, new PersistentPlantGeometryStorage(), Vector3.zero));
                //}));
                //threads[threads.Count - 1].Start(parentPairs[i]);
            }
            //foreach (var thread in threads)
            //{
             //   thread.Join();
            //}
            UnityEngine.Debug.Log("Child Crossover and Mutation took " + timer.ElapsedMilliseconds + " milliseconds");
            timer.Reset();

            return childPlants;
        }

        public Plant GetFittestPlant(List<Plant> parents)
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

            return fittestPlant;
        }
    }
}
