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

            var cylinderObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinderObject.transform.position = startPosition + (distance / 2);
            cylinderObject.transform.localScale = new Vector3(0.1f, length / 2, 0.1f);
            //throw new NotImplementedException();
        }
    }
}
