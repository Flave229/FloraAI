using System;
using System.Collections.Generic;
using Assets.Scripts.Render;
using UnityEngine;

namespace Assets.Scripts.TurtleGeometry
{
    public class TurtlePen
    {
        private readonly GeometryRenderSystem _renderSystem;
        private Stack<Vector3> _positionStack;
        private Stack<Vector3> _rotationStack;
        private Vector3 _currentPosition;
        private Vector3 _currentRotation;

        public float ForwardStep;
        public float RotationStep;
        
        public TurtlePen(GeometryRenderSystem renderSystem)
        {
            _positionStack = new Stack<Vector3>();
            _rotationStack = new Stack<Vector3>();
            _renderSystem = renderSystem;
        }

        public void Draw(Vector3 startingPosition, string commandString)
        {
            _currentPosition = startingPosition;
            _currentRotation = new Vector3(0, 0, 0);

            foreach (var command in commandString)
            {
                switch (command)
                {
                    case 'F':
                        MoveForward();
                        break;
                }
            }
        }

        private void MoveForward()
        {
            var lastPosition = _currentPosition;
            var rotatedX = ForwardStep * Math.Sin(_currentRotation.x);
            var rotatedY = ForwardStep * Math.Cos(_currentRotation.y);
            _currentPosition = new Vector3((float)(_currentPosition.x + rotatedX), (float)(_currentPosition.y + rotatedY), _currentPosition.z);
            _renderSystem.DrawCylinder(lastPosition, _currentPosition, 0.1);
        }
    }
}
