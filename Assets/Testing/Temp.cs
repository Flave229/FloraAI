using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Common;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing
{
    class Temp
    {
        [Test]
        public void ThenTemp()
        {
            Plant plant = new Plant(new LSystem(new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                {
                    "A", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Rule = "![AFAFF]LF\\S",
                            Probability = 1
                        }
                    }
                },
                {
                    "L", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Rule = "[OO&-O-O]&O^O",
                            Probability = 1
                        }
                    }
                },
                {
                    "S", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Rule = "!&+[+F^A[!-&!\\F]]!F",
                            Probability = 1
                        }
                    }
                },
                {
                    "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Rule = "-[^FAL-L]L+F",
                            Probability = 1
                        }
                    }
                }
            }), "A"), new TurtlePen(new NullRenderSystem())
                {
                    BranchReductionRate = new MinMax<float>
                    {
                        Max = 0.8f,
                        Min = 0.8f
                    },
                    ForwardStep = 0.1f,
                    RotationStep = 22.5f,
                    BranchDiameter = 0.1f
            }, new PersistentPlantGeometryStorage(), Vector3.zero,
            Color.white);

            for (int i = 0; i < 4; ++i)
            {
                plant.Update();
            }
            plant.Generate();

            PlantFitness fitnessEval = new PlantFitness(new LeafFitness(new SunInformation
            {
                Azimuth = 240,
                WinterAltitude = 30,
                SummerAltitude = 60,
                Light = Color.green
            }));

            float fitness = fitnessEval.EvaluateFitness(plant);
            Debug.Log(fitness);
            Debug.Log("Leaf Fitness: " + plant.Fitness.LeafEnergy);
        }
    }
}
