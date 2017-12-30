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

        private void Start()
        {
            GeometryRenderSystem renderSystem = new GeometryRenderSystem();
            TurtlePen turtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 0.05f,
                RotationStep = 15 * (float)(Math.PI / 180)
            };
            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>
            {
                {
                    "F",  new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "F[+F]F[-F]F"
                        } 
                    }
                }
            };
            RuleSet ruleSet = new RuleSet(rules);
            LSystem lindenMayerSystem = new LSystem(turtlePen, ruleSet, "F");
            _plant = new Plant(lindenMayerSystem, new Vector3(transform.position.x + 1, transform.position.y + 0.775f, transform.position.z + 1));
        }

        private void Update()
        {
            var currentTime = DateTime.Now;
            if (_iterations < 5 && (currentTime - _startTime).Seconds > 5)
            {
                _plant.Update();
                _plant.Generate();
                _startTime = currentTime;
                _iterations++;
            }
        }
    }
}