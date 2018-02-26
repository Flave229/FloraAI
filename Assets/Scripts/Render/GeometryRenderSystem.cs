using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Render
{
    public interface IRenderSystem
    {
        void DrawCylinder(Vector3 sourcePosition, Vector3 targetPosition, float diameter);
        void DrawQuad(Vector3 position, Vector3 direction, Color color, ref Vector3 right);
        void ClearObjects();
    }

    public class GeometryRenderSystem : IRenderSystem
    {
        private readonly List<GameObject> _gameObjects;

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

        public void ClearObjects()
        {
            foreach (var gameObject in _gameObjects)
            {
                Object.Destroy(gameObject);
            }

            _gameObjects.Clear();
        }

        public void DrawQuad(Vector3 position, Vector3 direction, Color color, ref Vector3 right)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Cube);
            quad.transform.position = position;
            quad.transform.localScale = new Vector3(0.00001f, 0.1f, 0.1f);
            quad.transform.LookAt(direction);
            quad.transform.up = direction;
            quad.GetComponent<Renderer>().material.color = color;

            right = quad.transform.right;

            _gameObjects.Add(quad);
        }
    }
}