using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plant
    {
        public readonly ILSystem LindenMayerSystem;
        private readonly TurtlePen _turtlePen;
        public readonly Vector3 Position;
        public PersistentPlantGeometryStorage GeometryStorage;

        public Plant(ILSystem lindenMayerSystem, TurtlePen turtlePen, PersistentPlantGeometryStorage geometryStorage, Vector3 position)
        {
            LindenMayerSystem = lindenMayerSystem;
            _turtlePen = turtlePen;
            GeometryStorage = geometryStorage;
            Position = position;
        }

        public void Update()
        {
            LindenMayerSystem.Iterate();
        }

        public void Generate()
        {
            _turtlePen.Draw(GeometryStorage, Position, LindenMayerSystem.GetCommandString());
        }
    }
}