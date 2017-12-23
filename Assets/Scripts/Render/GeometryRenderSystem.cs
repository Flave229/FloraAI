using System;
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

        public void DrawCylinder(Vector3 startPosition, Vector3 endPosition, double v)
        {
            // Cylinder default height is 2
            Vector3 distance = endPosition - startPosition;
            float length = distance.magnitude;

            double angleX = Math.Acos(distance.y / Math.Sqrt(distance.y * distance.y + distance.x * distance.x));

            if (distance.x > 0)
                angleX *= -1;

            var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinderObject.transform.position = startPosition + (distance / 2);
            cylinderObject.transform.Rotate(0, 0, (float)(angleX * (180 / Math.PI)));
            cylinderObject.transform.localScale = new Vector3(0.01f, length / 2, 0.01f);

            _gameObjects.Add(cylinderObject);
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