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
                        //IncreaseVerticalAxis();
                        IncreaseRollAxis();
                        break;
                    case '-':
                        //DecreaseVerticalAxis();
                        DecreaseRollAxis();
                        break;
                    case '&':
                        IncreaseLateralAxis();
                        break;
                    case '^':
                        DecreaseLateralAxis();
                        break;
                    case '\\':
                        IncreaseVerticalAxis();
                        //IncreaseRollAxis();
                        break;
                    case '/':
                        DecreaseVerticalAxis();
                        //DecreaseRollAxis();
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
            _renderSystem.DrawCylinder(lastPosition + ((_currentPosition - lastPosition) / 2), _currentRotation, ForwardStep / 2);
            _renderSystem.DrawSphere(_currentPosition);
            _renderSystem.DrawSphere(lastPosition);
        }

        private void IncreaseLateralAxis()
        {
            _currentRotation.x += RotationStep;
        }

        private void DecreaseLateralAxis()
        {
            _currentRotation.x -= RotationStep;
        }

        private void IncreaseVerticalAxis()
        {
            _currentRotation.y += RotationStep;
        }

        private void DecreaseVerticalAxis()
        {
            _currentRotation.y -= RotationStep;
        }

        private void IncreaseRollAxis()
        {
            _currentRotation.z += RotationStep;
        }

        private void DecreaseRollAxis()
        {
            _currentRotation.z -= RotationStep;
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
