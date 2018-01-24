using System;
using System.Collections.Generic;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    class PlantSpawner : MonoBehaviour
    {
        private DateTime _startTime;
        private int _iterations;
        private Plant _plant;

        public int MaximumIterations;

        private void Start()
        {
            GeometryRenderSystem renderSystem = new GeometryRenderSystem();
            TurtlePen turtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 0.3f,
                RotationStep = 22.5f
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
                            Rule = ""
                        }
                    }
                }
            };
            RuleSet ruleSet = new RuleSet(rules);
            LSystem lindenMayerSystem = new LSystem(ruleSet, "A");
            _plant = new Plant(lindenMayerSystem, turtlePen, new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
        }

        private void Update()
        {
            var currentTime = DateTime.Now;
            if (_iterations < MaximumIterations && (currentTime - _startTime).Seconds > 3)
            {
                _plant.Update();
                _plant.Generate();
                _startTime = currentTime;
                _iterations++;
            }
        }
    }
}