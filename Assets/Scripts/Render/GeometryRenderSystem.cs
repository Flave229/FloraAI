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

        public void DrawCylinder(Vector3 sourcePosition, Vector3 targetPosition, float diameter)
        {
            var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            float height = Vector3.Distance(sourcePosition, targetPosition);
            cylinderObject.transform.position = Vector3.Lerp(sourcePosition, targetPosition, 0.5f);
            cylinderObject.transform.up = targetPosition - sourcePosition;
            cylinderObject.transform.localScale = new Vector3(diameter, height / 2, diameter);

            _gameObjects.Add(cylinderObject);
        }

        public void DrawSphere(Vector3 position, float uniformScale)
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            sphere.transform.localScale = new Vector3(uniformScale, uniformScale, uniformScale);
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