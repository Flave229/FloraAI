using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Render
{
    public class GeometryRenderSystem
    {
        private List<GameObject> _gameObjects;

        public GeometryRenderSystem()
        {
            _gameObjects = new List<GameObject>();
        }

        public void DrawCylinder(Vector3 position, Vector3 rotation, float height)
        {
            var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinderObject.transform.position = position;
            cylinderObject.transform.Rotate(rotation);
            cylinderObject.transform.localScale = new Vector3(0.01f, height, 0.01f);

            _gameObjects.Add(cylinderObject);
        }

        public void DrawSphere(Vector3 position)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            sphere.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            _gameObjects.Add(sphere);
        }

        public void ClearObjects()
        {
            foreach (var gameObject in _gameObjects)
            {
                Object.Destroy(gameObject);
            }

            _gameObjects.Clear();
        }
    }
}