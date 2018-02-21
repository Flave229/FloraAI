using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Render
{
    public class PersistentPlantGeometryStorage
    {
        public readonly List<Vector3> LeafPositions;

        public PersistentPlantGeometryStorage()
        {
            LeafPositions = new List<Vector3>();
        }
        
        public void StoreLeaf(Vector3 position)
        {
            LeafPositions.Add(position);
        }
    }
}