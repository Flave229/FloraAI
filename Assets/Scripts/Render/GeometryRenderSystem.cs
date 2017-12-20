using System;
using UnityEngine;

namespace Assets.Scripts.Render
{
    public class GeometryRenderSystem
    {
        public void DrawCylinder(Vector3 startPosition, Vector3 endPosition, double v)
        {
            // Cylinder default height is 2
            Vector3 distance = endPosition - startPosition;
            float length = distance.magnitude;

            double angleX = Math.Acos(distance.y / Math.Sqrt(distance.y * distance.y + distance.x * distance.x));

            var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinderObject.transform.position = startPosition + (distance / 2);
            cylinderObject.transform.Rotate(0, 0, (float)(angleX * (180 / Math.PI)));
            cylinderObject.transform.localScale = new Vector3(0.1f, length / 2, 0.1f);
            //throw new NotImplementedException();
        }
    }
}