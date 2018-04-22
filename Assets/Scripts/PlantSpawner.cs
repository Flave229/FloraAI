using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
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
        public double WinterAltitude;
        public double SummerAltitude;
        public double Azimuth;
        private Color _sunColour;
        private Text _iterationText;
        private Text _debugOutput;
        private Plant _fittestPlant;
        private bool _paused;
        private bool _finalRender;
        private TurtlePen _fakeTurtlePen;

        private void Awake()
        {
            _paused = true;
            IRenderSystem renderSystem = new NullRenderSystem();
            _fakeTurtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 0.1f,
                RotationStep = 22.5f,
                BranchDiameter = 0.1f,
                BranchReductionRate = new MinMax<float>
                {
                    Min = 0.8f,
                    Max = 0.8f
                }
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
            };
            _genetics = new PlantGenetics(new System.Random(), _fakeTurtlePen, new SunInformation
            {
                WinterAltitude = WinterAltitude,
                Azimuth = Azimuth,
                SummerAltitude = SummerAltitude,
                Light = FindObjectOfType<Light>().color
            }, 0.01f);

            StartGeneticAlgorithm();
        }

        public void StartGeneticAlgorithm()
        {
            _iterations = 0;
            _currentPlant = 0;
            _finalRender = false;
            if (_iterationText != null)
                _iterationText.text = "Iterations: " + _iterations;

            System.Random randomGenerator = new System.Random();
            LSystemGenerator lindenMayerSystemGenerator = LSystemGenerator.Instance();
            List<Plant> initialPopulation = new List<Plant>();
            //List<LSystem> staticLSystemSet = lindenMayerSystemGenerator.GetStaticLSystemSets().ConvertAll(x => new LSystem(x.GetRuleSet(), "A"));
            for (int i = 0; i < 50; ++i)
            {
                //ILSystem randomLSystem = staticLSystemSet[i];
                ILSystem randomLSystem = lindenMayerSystemGenerator.GenerateRandomLSystem();
                initialPopulation.Add(new Plant(randomLSystem, _fakeTurtlePen, new PersistentPlantGeometryStorage(), Vector3.zero, new Color((float)randomGenerator.NextDouble(), (float)randomGenerator.NextDouble(), (float)randomGenerator.NextDouble())));
            }

            _plants = initialPopulation;
        }

        public Plant GetFittestPlant()
        {
            return _fittestPlant;
        }

        public bool GetPaused()
        {
            return _paused;
        }

        public void UpdateSunColour()
        {
            Color newColour = FindObjectOfType<Light>().color;
            _genetics.UpdateSunInformation(newColour);
        }

        public void TogglePause()
        {
            _paused = !_paused;
        }

        public void SetMaxIterationCount(int iterations)
        {
            MaximumGeneticIterations = iterations;
        }

        public int GetIterationCount()
        {
            return _iterations;
        }

        private void Start()
        {
            GameObject sun = FindObjectOfType<Light>().gameObject;
            sun.transform.rotation.Set((float)(WinterAltitude + SummerAltitude) / 2, (float)Azimuth, 0, 1);
            _sunColour = sun.GetComponent<Light>().color;

            _iterationText = GameObject.Find("IterationCount").GetComponent<Text>();
            _debugOutput = GameObject.Find("DebugOutput").GetComponent<Text>();
        }

        private void Update()
        {
            try
            {
                bool renderPlants = Input.GetKeyDown(KeyCode.R);

                if (_paused)
                    return;

                if (_delayIteration > 0)
                {
                    _delayIteration -= Time.deltaTime;
                    return;
                }
                if (_iterations >= MaximumGeneticIterations - 1)
                {
                    if (_finalRender)
                        return;

                    _finalRender = true;
                    renderPlants = true;
                }

                if (renderPlants)
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
                        Vector3.zero, 
                        _fittestPlant.LindenMayerSystem.GetLeafColor());

                    foreach (var plant in _plants.Where(x => x != _fittestPlant))
                        plant.LindenMayerSystem.ClearCommandString();
                    
                    //Debug.Log("Attempting to draw plant with total geometry count of " + (fittestPlant.Fitness.LeafCount + fittestPlant.Fitness.BranchCount));
                    plantToDraw.Generate();
                    _fittestPlant.RenderedGeometry = plantToDraw.RenderedGeometry;
                    foreach (var geometry in _fittestPlant.RenderedGeometry)
                    {
                        geometry.transform.position = new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1);
                    }
                }
                else
                {
                    if (_currentPlant + 1 < _plants.Count)
                    {
                        for (int i = 0; i < 5; ++i)
                        {
                            for (int j = 0; j < MaximumGrowthIterations; ++j)
                            {
                                _plants[_currentPlant + i].Update();
                            }
                            _plants[_currentPlant + i].Generate();
                            _plants[_currentPlant + i].LindenMayerSystem.ClearCommandString();
                            GC.Collect();
                        }
                        _currentPlant += 5;

                        return;
                    }
                    _currentPlant = 0;
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