using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class PlantSpawner : MonoBehaviour
    {
        private List<Plant> _plants;
        private PlantGenetics _genetics;
        private int _iterations;
        private float _cooldown;
        private TurtlePen _realTurtlePen;
        private float _delayIteration;
        private int _currentPlant;

        public int MaximumGrowthIterations;
        public int MaximumGeneticIterations;
        public int DrawEveryXIterations;
        public double WinterAltitude;
        public double SummerAltitude;
        public double Azimuth;
        public Color SunColour;
        private Text _iterationText;
        private Text _debugOutput;
        private Plant _fittestPlant;

        private void Awake()
        {
            _iterations = 0;
            _currentPlant = 0;
            IRenderSystem renderSystem = new NullRenderSystem();
            TurtlePen fakeTurtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 0.1f,
                RotationStep = 22.5f,
                BranchDiameter = 0.1f,
                BranchReductionRate = new MinMax<float>
                {
                    Min = 0.8f,
                    Max = 0.8f
                }
                //RotationStep = 2.0f
            };
            _realTurtlePen = new TurtlePen(new GeometryRenderSystem())
            {
                ForwardStep = 0.1f,
                RotationStep = 22.5f,
                BranchDiameter = 0.1f,
                BranchReductionRate = new MinMax<float>
                {
                    Min = 0.8f,
                    Max = 0.8f
                }
                //RotationStep = 2.0f
            };
            _genetics = new PlantGenetics(new System.Random(), fakeTurtlePen, new SunInformation
            {
                WinterAltitude = WinterAltitude,
                Azimuth = Azimuth,
                SummerAltitude = SummerAltitude,
                Colour = SunColour
            }, 0.01f);
            //Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>
            //{
            //    {
            //        "A",  new List<LSystemRule>
            //        {
            //            new LSystemRule
            //            {
            //                Probability = 1,
            //                Rule = "[&FL!A]/////'[&FL!A]///////'[&FL!A]"
            //            }
            //        }
            //    },
            //    {
            //        "F",  new List<LSystemRule>
            //        {
            //            new LSystemRule
            //            {
            //                Probability = 1,
            //                Rule = "S/////F"
            //            }
            //        }
            //    },
            //    {
            //        "S",  new List<LSystemRule>
            //        {
            //            new LSystemRule
            //            {
            //                Probability = 1,
            //                Rule = "FL"
            //            }
            //        }
            //    },
            //    {
            //        "L",  new List<LSystemRule>
            //        {
            //            new LSystemRule
            //            {
            //                Probability = 1,
            //                Rule = "['''^^O]"
            //            }
            //        }
            //    }
            //};
            //RuleSet ruleSet = new RuleSet(rules);
            //ILSystem lindenMayerSystem = new LSystem(ruleSet, "A");
            //Plant plant1 = new Plant(lindenMayerSystem, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
            //ILSystem lindenMayerSystem2 = new LSystem(ruleSet, "A");
            //Plant plant2 = new Plant(lindenMayerSystem2, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
            //_plants = _genetics.GenerateChildPopulation(new List<Plant> { plant1, plant2 });

            System.Random randomGenerator = new System.Random();
            LSystemGenerator lindenMayerSystemGenerator = new LSystemGenerator(randomGenerator);
            List<Plant> initialPopulation = new List<Plant>();
            for (int i = 0; i < 50; ++i)
            {
                ILSystem randomLSystem = lindenMayerSystemGenerator.GenerateRandomLSystem();
                initialPopulation.Add(new Plant(randomLSystem, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1), new Color((float)randomGenerator.NextDouble(), (float)randomGenerator.NextDouble(), (float)randomGenerator.NextDouble())));
            }

            _plants = initialPopulation;
        }

        public Plant GetFittestPlant()
        {
            return _fittestPlant;
        }

        private void Start()
        {
            GameObject sun = FindObjectOfType<Light>().gameObject;
            sun.transform.rotation.Set((float)(WinterAltitude + SummerAltitude) / 2, (float)Azimuth, 0, 1);
            sun.GetComponent<Light>().color = SunColour;

            _iterationText = GameObject.Find("IterationCount").GetComponent<Text>();
            _debugOutput = GameObject.Find("DebugOutput").GetComponent<Text>();
        }

        private void Update()
        {
            try
            {
                if (_delayIteration > 0)
                {
                    _delayIteration -= Time.deltaTime;
                    return;
                }
                if (_iterations >= MaximumGeneticIterations)
                    return;

                if (_iterations % DrawEveryXIterations == 0 && _iterations != 0)
                {
                    foreach (Plant plant in _plants)
                    {
                        for (int j = 0; j < MaximumGrowthIterations; ++j)
                        {
                            plant.Update();
                        }
                        plant.Generate();
                        GC.Collect();
                    }

                    _delayIteration = 4;
                    _currentPlant = 0;
                    _fittestPlant = _genetics.GetFittestPlant(_plants);
                    Plant plantToDraw = new Plant(_fittestPlant.LindenMayerSystem, _realTurtlePen,
                        new PersistentPlantGeometryStorage(),
                        new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1),
                        _fittestPlant.LindenMayerSystem.GetLeafColor());

                    foreach (var plant in _plants.Where(x => x != _fittestPlant))
                        plant.LindenMayerSystem.ClearCommandString();
                    
                    //Debug.Log("Attempting to draw plant with total geometry count of " + (fittestPlant.Fitness.LeafCount + fittestPlant.Fitness.BranchCount));
                    plantToDraw.Generate();
                }
                else
                {
                    if (_currentPlant < _plants.Count)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            for (int j = 0; j < MaximumGrowthIterations; ++j)
                            {
                                _plants[_currentPlant + i].Update();
                            }
                            _plants[_currentPlant + i].Generate();
                            //_plants[_currentPlant + i].LindenMayerSystem.ClearCommandString();
                            //GC.Collect();
                        }
                        _currentPlant += 5;

                        return;
                    }

                    _fittestPlant = _genetics.GetFittestPlant(_plants);
                    foreach (var plant in _plants.Where(x => x != _fittestPlant))
                        plant.LindenMayerSystem.ClearCommandString();
                }
                
                //Debug.Log("Iteration " + _iterations);
                ++_iterations;
                _plants = _genetics.GenerateChildPopulation(_plants);
                _iterationText.text = "Iterations: " + _iterations;
                GC.Collect();

                //_cooldown -= Time.deltaTime;
                //if (_iterations < MaximumGeneticIterations)
                //{
                //    ++_iterations;

                //    foreach (var plant in _plants)
                //    {
                //        for (int j = 0; j < MaximumGrowthIterations; ++j)
                //        {
                //            plant.Update();
                //        }

                //        plant.Generate();
                //    }

                //    // Need to get fittest plant
                //    if (_cooldown <= 0.0f)
                //    {
                //        _cooldown = 5.0f;
                //        KeyValuePair<Plant, float> fittestPlant = _genetics.GetFittestPlant(_plants);
                //        Plant plantToDraw = new Plant(fittestPlant.Key.LindenMayerSystem, _realTurtlePen,
                //            new PersistentPlantGeometryStorage(),
                //            new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
                //        plantToDraw.Generate();

                //        //foreach (var rule in fittestPlant.Key.LindenMayerSystem.GetRuleSet().Rules)
                //        //{
                //        //    Debug.Log("Rule " + rule.Key + ": " + rule.Value[0].Rule);
                //        //}
                //        //Debug.Log("Total Command: " + fittestPlant.Key.LindenMayerSystem.GetCommandString());
                //        //Debug.Log("Fitness: " + fittestPlant.Value);
                //    }

                //    _plants = _genetics.GenerateChildPopulation(_plants);
                //}

            }
            catch (Exception e)
            {
                _debugOutput.text = "Critical Error occuring: " + e.Message + "\n";
                _debugOutput.text += "StackTrace: " + e.StackTrace + "\n";
                _debugOutput.text += "Source: " + e.Source + "\n";
                _debugOutput.text += "Inner Exception: " + e.InnerException + "\n";
            }
        }
    }
}