using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    class PlantSpawner : MonoBehaviour
    {
        private Plant _plant;

        public int MaximumIterations;

        private void Awake()
        {
            GeometryRenderSystem renderSystem = new GeometryRenderSystem();
            TurtlePen turtlePen = new TurtlePen(renderSystem)
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
            _plant = new Plant(lindenMayerSystem, turtlePen, new PersistentPlantGeometryStorage(), new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
        }

        private void Start()
        {
            for (int i = 0; i < MaximumIterations; ++i)
            {
                _plant.Update();
            }

            _plant.Generate();
        }
    }
}