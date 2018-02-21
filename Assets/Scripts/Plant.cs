using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plant
    {
        private readonly ILSystem _lindenMayerSystem;
        private readonly TurtlePen _turtlePen;
        private readonly Vector3 _position;
        public PersistentPlantGeometryStorage GeometryStorage;

        public Plant(ILSystem lindenMayerSystem, TurtlePen turtlePen, PersistentPlantGeometryStorage geometryStorage, Vector3 position)
        {
            _lindenMayerSystem = lindenMayerSystem;
            _turtlePen = turtlePen;
            GeometryStorage = geometryStorage;
            _position = position;
        }

        public void Update()
        {
            _lindenMayerSystem.Iterate();
        }

        public void Generate()
        {
            _turtlePen.Draw(GeometryStorage, _position, _lindenMayerSystem.GetCommandString());
        }
    }
}