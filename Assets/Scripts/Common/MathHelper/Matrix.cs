using System;
using UnityEngine;

namespace Assets.Scripts.Common.MathHelper
{
    public class Matrix
    {
        public static Vector3 RotateVector(Vector4 vector, Vector3 rotation)
        {
            Matrix4x4 xRotationMatrix = new Matrix4x4
            {
                m00 = 1, m01 = 0, m02 = 0, m03 = 0,
                m10 = 0, m11 = (float)Math.Cos(rotation.x), m12 = (float)-Math.Sin(rotation.x), m13 = 0,
                m20 = 0, m21 = (float)Math.Sin(rotation.x), m22 = (float)Math.Cos(rotation.x), m23 = 0,
                m30 = 0, m31 = 0, m32 = 0, m33 = 1
            };
            Matrix4x4 yRotationMatrix = new Matrix4x4
            {
                m00 = (float)Math.Cos(rotation.y), m01 = 0, m02 = (float)Math.Sin(rotation.y), m03 = 0,
                m10 = 0, m11 = 1, m12 = 0, m13 = 0,
                m20 = (float)-Math.Sin(rotation.y), m21 = 0, m22 = (float)Math.Cos(rotation.y), m23 = 0,
                m30 = 0, m31 = 0, m32 = 0, m33 = 1
            };
            Matrix4x4 zRotationMatrix = new Matrix4x4
            {
                m00 = (float)Math.Cos(rotation.z), m01 = (float)-Math.Sin(rotation.z), m02 = 0, m03 = 0,
                m10 = (float)Math.Sin(rotation.z), m11 = (float)Math.Cos(rotation.z), m12 = 0, m13 = 0,
                m20 = 0, m21 = 0, m22 = 1, m23 = 0,
                m30 = 0, m31 = 0, m32 = 0, m33 = 1
            };

            Vector4 rotatedVector = (zRotationMatrix * vector);
            rotatedVector = (yRotationMatrix * rotatedVector);
            rotatedVector = (xRotationMatrix * rotatedVector);

            return new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z);
        }

        public static Vector3 RotateVector(Vector3 vector, Vector3 rotation)
        {
            Vector4 rotatableVector = new Vector4(vector.x, vector.y, vector.z, 1);
            return RotateVector(rotatableVector, rotation);
        }
    }
}
