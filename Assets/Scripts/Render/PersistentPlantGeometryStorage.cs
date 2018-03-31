using System.Collections.Generic;
using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Render
{
    public class PersistentPlantGeometryStorage
    {
        public readonly List<Leaf> Leaves;

        private Branch _currentParentBranch;
        private Branch _currentBranch;
        private Stack<Branch> _previousBranches;
        private Branch _rootBranch;

        public PersistentPlantGeometryStorage()
        {
            Leaves = new List<Leaf>();
            _previousBranches = new Stack<Branch>();
        }

        public void StartPlant()
        {
            _currentParentBranch = null;
            _currentBranch = new Branch();
            _rootBranch = _currentBranch;
        }

        public void StoreLeaf(Vector3 position, Vector3 rightVector)
        {
            var leaf = new Leaf
            {
                Position = position,
                Normal = rightVector
            };
            if (_currentBranch != null)
                _currentBranch.ChildLeaves.Add(leaf);
            else
                _currentParentBranch.ChildLeaves.Add(leaf);
            Leaves.Add(leaf);
        }

        public void ExtendBranch(Vector3 sourcePosition, Vector3 targetPosition, float diameter)
        {
            _currentParentBranch = _currentBranch;
            _currentBranch = new Branch
            {
                ParentBranch = _currentParentBranch,
                Diameter = diameter,
                Length = Vector3.Distance(sourcePosition, targetPosition)
            };
            if (_currentParentBranch != null)
                _currentParentBranch.ChildBranches.Add(_currentBranch);
        }

        public void StartNewBranch()
        {
            _previousBranches.Push(_currentBranch);
            //if (_currentBranch == null)
            //    return;
        }           

        public void ReturnToLastBranch()
        {
            _currentBranch = _previousBranches.Pop();
            _currentParentBranch = _currentBranch.ParentBranch;
        }

        public Branch GetRootBranch()
        {
            return _rootBranch;
        }
    }
}