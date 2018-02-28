using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Render
{
    public class PersistentPlantGeometryStorage
    {
        public readonly List<Leaf> Leaves;

        public PersistentPlantGeometryStorage()
        {
            Leaves = new List<Leaf>();
        }
        
        public void StoreLeaf(Vector3 position, Vector3 rightVector)
        {
            Leaves.Add(new Leaf
            {
                Position = position,
                Normal = rightVector
            });
        }
    }
}