using Assets.Scripts.LSystems;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plant
    {
        private readonly LSystem _lindenMayerSystem;
        private readonly TurtlePen _turtlePen;
        private readonly Vector3 _position;

        public Plant(LSystem lindenMayerSystem, TurtlePen turtlePen, Vector3 position)
        {
            _lindenMayerSystem = lindenMayerSystem;
            _turtlePen = turtlePen;
            _position = position;
        }

        public void Update()
        {
            _lindenMayerSystem.Iterate();
        }

        public void Generate()
        {
            Debug.Log(_lindenMayerSystem.GetCommandString());
            _turtlePen.Draw(_position, _lindenMayerSystem.GetCommandString());
        }
    }
}