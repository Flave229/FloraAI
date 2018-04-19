using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Export
{
    public class ObjExporter
    {
        private static int _startIndex = 0;

        public static string ExportGameObject(List<GameObject> objectsToExport)
        {
            string fileName = "plantExport_" + DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
            string fileLocation = Path.Combine(Environment.CurrentDirectory, "ExportedPlants");
            FileInfo fileInfo = new FileInfo(fileLocation + "\\" + fileName);
            fileInfo.Directory.Create();

            Start();

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("#" + objectsToExport[0].name + ".obj"
                                    + "\n#" + System.DateTime.Now.ToLongDateString()
                                    + "\n#" + System.DateTime.Now.ToLongTimeString()
                                    + "\n#--------"
                                    + "\n\n");
            Transform transform = objectsToExport[0].transform;
            Vector3 originalPosition = transform.position;
            transform.position = Vector3.zero;

            ProcessTransformation(transform, ref stringBuilder);
            WriteToFile(stringBuilder.ToString(), fileLocation, fileName);
            transform.position = originalPosition;

            End();
            return fileLocation + "/" + fileName + ".obj";
        }

        private static void Start()
        {
            _startIndex = 0;
        }

        private static void End()
        {
            _startIndex = 0;
        }

        private static void ProcessTransformation(Transform transform, ref StringBuilder stringBuilder)
        {
            stringBuilder.Append("#" + transform.name
                                 + "\n#--------"
                                 + "\n");

            stringBuilder.Append("g " + transform.name + "\n");

            MeshFilter meshFilter = transform.GetComponent<MeshFilter>();
            if (meshFilter)
                MeshToString(meshFilter, transform, ref stringBuilder);

            for (int i = 0; i < transform.childCount; ++i)
                ProcessTransformation(transform.GetChild(i), ref stringBuilder);
        }

        private static void MeshToString(MeshFilter meshFilter, Transform transform, ref StringBuilder stringBuilder)
        {
            Vector3 scale = transform.localScale;
            Vector3 position = transform.localPosition;
            Quaternion rotation = transform.localRotation;

            int vertexCount = 0;
            Mesh mesh = meshFilter.sharedMesh;
            if (!mesh)
                stringBuilder.Append("####Error####");

            Material[] mats = meshFilter.GetComponent<Renderer>().sharedMaterials;

            foreach (var vertex in mesh.vertices)
            {
                Vector3 transformedVertex = transform.TransformPoint(vertex);
                ++vertexCount;
                stringBuilder.Append(string.Format("v {0} {1} {2}\n", transformedVertex.x, transformedVertex.y, -transformedVertex.z));
            }
            stringBuilder.Append("\n");
            foreach (var normal in mesh.normals)
            {
                Vector3 rotatedNormal = rotation * normal;
                stringBuilder.Append(string.Format("vn {0} {1} {2}\n", -rotatedNormal.x, -rotatedNormal.y, rotatedNormal.z));
            }
            stringBuilder.Append("\n");
            foreach (var textureCoord in mesh.uv)
            {
                stringBuilder.Append(string.Format("vt {0} {1}\n", textureCoord.x, textureCoord.y));
            }
            for (int materialIndex = 0; materialIndex < mesh.subMeshCount; ++materialIndex)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append("usemtl ").Append(mats[materialIndex].name).Append("\n");
                stringBuilder.Append("usemap ").Append(mats[materialIndex].name).Append("\n");

                int[] triangles = mesh.GetTriangles(materialIndex);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    stringBuilder.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                        triangles[i] + 1 + _startIndex, 
                        triangles[i + 1] + 1 + _startIndex,
                        triangles[i + 2] + 1 + _startIndex));
                }
            }

            _startIndex += vertexCount;
        }

        private static void WriteToFile(string objectInfo, string fileDirectory, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileDirectory + "/" + fileName + ".obj"))
            {
                writer.Write(objectInfo);
            }
        }
    }
}