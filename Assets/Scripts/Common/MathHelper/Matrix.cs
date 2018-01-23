using UnityEngine;

namespace Assets.Scripts.Common.MathHelper
{
    public class Matrix
    {
        public static Vector3 RotateVector(Vector4 vector, Vector3 rotation)
        {

            return Quaternion.Euler(rotation.x, rotation.y, rotation.z) * vector;
        }

        public static Vector3 RotateVector(Vector3 vector, Vector3 rotation)
        {
            Vector4 rotatableVector = new Vector4(vector.x, vector.y, vector.z, 1);
            return RotateVector(rotatableVector, rotation);
        }
    }
}