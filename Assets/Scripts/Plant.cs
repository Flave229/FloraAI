using Assets.Scripts.LSystems;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plant
    {
        private readonly LSystem _lindenMayerSystem;
        private readonly Vector3 _position;

        public Plant(LSystem lindenMayerSystem, Vector3 position)
        {
            _lindenMayerSystem = lindenMayerSystem;
            _position = position;
        }

        public void Update()
        {
            _lindenMayerSystem.Update();
        }

        public void Generate()
        {
            _lindenMayerSystem.Draw(_position);
            //var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            //cylinderObject.transform.position = _position;
            //cylinderObject.transform.localScale = new Vector3(0.1f, 1, 0.1f);
        }
    }
}