using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Render
{
    public class PersistentPlantGeometryStorage
    {
        public readonly List<Leaf> Leaves;
        public readonly List<Vector3> Branches;

        public PersistentPlantGeometryStorage()
        {
            Leaves = new List<Leaf>();
            Branches = new List<Vector3>();
        }

        public void StoreLeaf(Vector3 position, Vector3 rightVector)
        {
            Leaves.Add(new Leaf
            {
                Position = position,
                Normal = rightVector
            });
        }

        public void StoreBranch(Vector3 position)
        {
            Branches.Add(position);
        }
    }
}