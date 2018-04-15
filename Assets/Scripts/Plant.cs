using System;
using Assets.Scripts.Data;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plant
    {
        private readonly TurtlePen _turtlePen;

        public readonly ILSystem LindenMayerSystem;
        public readonly Vector3 Position;
        public PersistentPlantGeometryStorage GeometryStorage;
        public Fitness Fitness;

        public Plant(ILSystem lindenMayerSystem, TurtlePen turtlePen, PersistentPlantGeometryStorage geometryStorage, Vector3 position, Color leafColour)
        {
            LindenMayerSystem = lindenMayerSystem;
            _turtlePen = turtlePen;
            GeometryStorage = geometryStorage;
            Position = position;
            LindenMayerSystem.SetLeafColour(leafColour);
        }

        public void Update()
        {
            try
            {
                LindenMayerSystem.Iterate();
            }
            catch (Exception e)
            {
                string message = e.Message + "\nCurrent String Length: " + LindenMayerSystem.GetCommandString().Length + "\n";
                foreach (var rule in LindenMayerSystem.GetRuleSet().Rules)
                    message += rule.Key + ": " + rule.Value[0].Rule + "\n";

                Exception newException = new Exception(message, e);
                throw newException;
            }
        }

        public void Generate()
        {
            _turtlePen.Draw(GeometryStorage, Position, LindenMayerSystem.GetCommandString(), LindenMayerSystem.GetLeafColor());
        }
    }
}