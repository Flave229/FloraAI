﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Render
{
    public class GeometryRenderSystem : IRenderSystem
    {
        private List<GameObject> _cylinders;
        private List<GameObject> _leaves;

        public GeometryRenderSystem()
        {
            _leaves = new List<GameObject>();
            _cylinders = new List<GameObject>();
        }

        private GameObject CreateMasterGameObject(string name)
        {
            GameObject masterGameObject = new GameObject(name);
            masterGameObject.AddComponent<MeshFilter>();
            masterGameObject.AddComponent<MeshRenderer>();
            return masterGameObject;
        }

        public void DrawCylinder(Vector3 sourcePosition, Vector3 targetPosition, float diameter)
        {
            PrimitiveType primitiveType;
            float height = Vector3.Distance(sourcePosition, targetPosition);
            //if (diameter > 0.02f)
            //{
            //    primitiveType = PrimitiveType.Cylinder;
            //    height /= 2;
            //}
            //else
            //{
                primitiveType = PrimitiveType.Cube;
            //}


            GameObject primitive = GameObject.CreatePrimitive(primitiveType);

            var capsuleCollider = primitive.GetComponent<CapsuleCollider>();
            Object.Destroy(capsuleCollider);
            primitive.transform.position = Vector3.Lerp(sourcePosition, targetPosition, 0.5f);
            primitive.transform.up = targetPosition - sourcePosition;
            primitive.transform.localScale = new Vector3(diameter, height, diameter);

            _cylinders.Add(primitive);

            //if (_cylinders.Count > 1000)
            //{
            //    Debug.Log("Premature Branch Optimisation");
            //    OptimiseMeshes(ref _cylinders);
            //}
        }

        public void ClearObjects()
        {
            foreach (var gameObject in _leaves)
            {
                Object.Destroy(gameObject);
            }

            _leaves.Clear();

            foreach (var gameObject in _cylinders)
            {
                Object.Destroy(gameObject);
            }

            _cylinders.Clear();
        }

        public void DrawQuad(Vector3 position, Vector3 direction, Color color, ref Vector3 right)
        {
            var quad = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var boxCollider = quad.GetComponent<BoxCollider>();
            Object.Destroy(boxCollider);
            quad.transform.position = position;
            quad.transform.localScale = new Vector3(0.00001f, 0.1f, 0.1f);
            quad.transform.LookAt(direction);
            quad.transform.up = direction;
            quad.GetComponent<Renderer>().material.color = color;

            right = quad.transform.right;

            _leaves.Add(quad);

            //if (_leaves.Count > 2000)
            //{
            //    Debug.Log("Premature Leaf Optimisation");
            //    OptimiseMeshes(ref _leaves);
            //}
        }

        public List<GameObject> FinalisePlant(Color leafColour)
        {
            OptimiseMeshes(ref _leaves, "Leaves");
            if (_leaves.Count > 0)
            {
                Material leafMaterial = _leaves[0].GetComponent<Renderer>().material;
                leafMaterial.shader = Shader.Find("UI/Default");
                leafMaterial.color = leafColour;
                leafMaterial.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
                leafMaterial.SetFloat("_SpecularHighlights", 0f);
                leafMaterial.SetFloat("_Glossiness", 0f);
            }
            OptimiseMeshes(ref _cylinders, "Branches");
            if (_cylinders.Count > 0)
                _cylinders[0].GetComponent<Renderer>().material.color = new Color(0.4f, 0.2f, 0);

            return new List<GameObject> { _leaves.Count > 0 ? _leaves[0] : new GameObject(), _cylinders.Count > 0 ? _cylinders[0] : new GameObject() };
        }


        private void OptimiseMeshes(ref List<GameObject> gameObjects, string name)
        {
            if (gameObjects.Count <= 0)
                return;

            if (gameObjects[0].name != name)
                gameObjects.Insert(0, CreateMasterGameObject(name));

            CombineInstance[] combiners = new CombineInstance[gameObjects.Count];

            for (int i = 0; i < gameObjects.Count; ++i)
            {
                combiners[i] = new CombineInstance
                {
                    subMeshIndex = 0,
                    mesh = gameObjects[i].GetComponent<MeshFilter>().mesh,//sharedMesh,
                    transform = gameObjects[i].transform.localToWorldMatrix
                };
            }

            Mesh finalMesh = new Mesh { indexFormat = IndexFormat.UInt32 };
            finalMesh.CombineMeshes(combiners, true, true);

            GameObject masterObject = gameObjects[0];
            masterObject.GetComponent<MeshFilter>().sharedMesh = finalMesh;

            for (int i = gameObjects.Count - 1; i > 1; --i)
            {
                Object.Destroy(gameObjects[i]);
                gameObjects.RemoveAt(gameObjects.Count - 1);
            }
        }
    }
}