using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using UnityEngine;

namespace Assets.Scripts
{
    class PlantSpawner : MonoBehaviour
    {
        private List<Plant> _plants;
        private PlantGenetics _genetics;
        private int _iterations;
        private float _cooldown;
        private TurtlePen _realTurtlePen;

        public int MaximumGrowthIterations;
        public int MaximumGeneticIterations;
        public double WinterAltitude;
        public double SummerAltitude;
        public double Azimuth;
        private float _delayIteration;

        private void Awake()
        {
            _iterations = 0;
            IRenderSystem renderSystem = new NullRenderSystem();
            TurtlePen fakeTurtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 0.1f,
                RotationStep = 22.5f,
                BranchDiameter = 0.06f,
                BranchReductionRate = new MinMax<float>
                {
                    Min = 0.4f,
                    Max = 0.8f
                }
                //RotationStep = 2.0f
            };
            _realTurtlePen = new TurtlePen(new GeometryRenderSystem())
            {
                ForwardStep = 0.1f,
                RotationStep = 22.5f,
                BranchDiameter = 0.06f,
                BranchReductionRate = new MinMax<float>
                {
                    Min = 0.4f,
                    Max = 0.8f
                }
                //RotationStep = 2.0f
            };
            _genetics = new PlantGenetics(new System.Random(), fakeTurtlePen, new SunInformation
            {
                WinterAltitude = WinterAltitude,
                Azimuth = Azimuth,
                SummerAltitude = SummerAltitude
            }, 0.01f);
            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>
            {
                {
                    "A",  new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "[&FL!A]/////'[&FL!A]///////'[&FL!A]"
                        }
                    }
                },
                {
                    "F",  new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "S/////F"
                        }
                    }
                },
                {
                    "S",  new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "FL"
                        }
                    }
                },
                {
                    "L",  new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "['''^^O]"
                        }
                    }
                }
            };
            RuleSet ruleSet = new RuleSet(rules);
            ILSystem lindenMayerSystem = new LSystem(ruleSet, "A");
            Plant plant1 = new Plant(lindenMayerSystem, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
            ILSystem lindenMayerSystem2 = new LSystem(ruleSet, "A");
            Plant plant2 = new Plant(lindenMayerSystem2, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
            _plants = _genetics.GenerateChildPopulation(new List<Plant> {plant1, plant2});

            //LSystemGenerator lindenMayerSystemGenerator = new LSystemGenerator(new System.Random());
            //List<Plant> initialPopulation = new List<Plant>();
            //for (int i = 0; i < 50; ++i)
            //{
            //    ILSystem randomLSystem = lindenMayerSystemGenerator.GenerateRandomLSystem();
            //    initialPopulation.Add(new Plant(randomLSystem, fakeTurtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1)));
            //}

            //_plants = _genetics.GenerateChildPopulation(initialPopulation);
        }
        
        private void Start()
        {
        }

        private void Update()
        {
            if (_delayIteration > 0)
            {
                _delayIteration -= Time.deltaTime;
                return;
            }
            if (_iterations >= MaximumGeneticIterations)
                return;

            if (_iterations % 5 == 0 && _iterations != 0)
            {
                _delayIteration = 10;
                foreach (var plant in _plants)
                {
                    for (int j = 0; j < MaximumGrowthIterations; ++j)
                    {
                        plant.Update();
                    }

                    plant.Generate();
                }

                ++_iterations;
                KeyValuePair<Plant, float> fittestPlant = _genetics.GetFittestPlant(_plants);
                Plant plantToDraw = new Plant(fittestPlant.Key.LindenMayerSystem, _realTurtlePen,
                    new PersistentPlantGeometryStorage(),
                    new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
                plantToDraw.Generate();

                foreach (var rule in fittestPlant.Key.LindenMayerSystem.GetRuleSet().Rules)
                {
                    Debug.Log("Rule " + rule.Key + ": " + rule.Value[0].Rule);
                }
                Debug.Log("Total Command: " + fittestPlant.Key.LindenMayerSystem.GetCommandString());
                Debug.Log("Fitness: " + fittestPlant.Value);
                _plants = _genetics.GenerateChildPopulation(_plants);
            }
            else
            {
                foreach (var plant in _plants)
                {
                    for (int j = 0; j < MaximumGrowthIterations; ++j)
                    {
                        plant.Update();
                    }

                    plant.Generate();
                }

                ++_iterations;
                _plants = _genetics.GenerateChildPopulation(_plants);
            }

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
    }
}