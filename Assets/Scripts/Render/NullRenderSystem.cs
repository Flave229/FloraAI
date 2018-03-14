using UnityEngine;

namespace Assets.Scripts.Render
{
    class NullRenderSystem : IRenderSystem
    {
        public void DrawCylinder(Vector3 sourcePosition, Vector3 targetPosition, float diameter)
        {
            return;
        }

        public void DrawQuad(Vector3 position, Vector3 direction, Color color, ref Vector3 right)
        {
            var fakeObject = new GameObject();
            fakeObject.transform.LookAt(direction);
            fakeObject.transform.up = direction;

            right = fakeObject.transform.right;
            Object.Destroy(fakeObject);
        }

        public void ClearObjects()
        {
            return;
        }

        public void FinalisePlant()
        {
            return;
        }
    }
}
