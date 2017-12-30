using System;
using System.Collections.Generic;
using Assets.Scripts.Render;
using UnityEngine;
using Assets.Scripts.Common.MathHelper;

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
            _renderSystem.ClearObjects();
            _currentPosition = startingPosition;
            _currentRotation = new Vector3(0, 0, 0);

            foreach (var command in commandString)
            {
                switch (command)
                {
                    case 'F':
                        MoveForward();
                        break;
                    case '+':
                        RotateRight();
                        break;
                    case '-':
                        RotateLeft();
                        break;
                    case '[':
                        PushTransformation();
                        break;
                    case ']':
                        PopTransformation();
                        break;
                }
            }
        }

        private void MoveForward()
        {
            var lastPosition = _currentPosition;
            _currentPosition += Matrix.RotateVector(new Vector3(0, ForwardStep, 0), _currentRotation);
            _renderSystem.DrawCylinder(lastPosition + ((_currentPosition - lastPosition) / 2), _currentRotation);
            _renderSystem.DrawSphere(_currentPosition);
            _renderSystem.DrawSphere(lastPosition);
        }

        private void RotateRight()
        {
            _currentRotation.x += RotationStep;
        }

        private void RotateLeft()
        {
            _currentRotation.x -= RotationStep;
        }

        private void PushTransformation()
        {
            _positionStack.Push(_currentPosition);
            _rotationStack.Push(_currentRotation);
        }

        private void PopTransformation()
        {
            _currentPosition = _positionStack.Pop();
            _currentRotation = _rotationStack.Pop();
        }
    }
}
