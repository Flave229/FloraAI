using UnityEngine;

namespace Assets.Scripts.Render
{
    public interface IRenderSystem
    {
        void DrawCylinder(Vector3 sourcePosition, Vector3 targetPosition, float diameter);
        void DrawQuad(Vector3 position, Vector3 direction, Color color, ref Vector3 right);
        void ClearObjects();
    }
}