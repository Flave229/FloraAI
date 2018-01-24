using System.Collections.Generic;
using Assets.Scripts.Render;
using UnityEngine;
using Assets.Scripts.Common.MathHelper;

namespace Assets.Scripts.TurtleGeometry
{
    public class TurtlePen
    {
        private Vector3 _rightVector;

        private readonly GeometryRenderSystem _renderSystem;
        private readonly Stack<Vector3> _positionStack;
        private readonly Stack<Vector3> _directionStack;
        private Vector3 _currentPosition;
        private Vector3 _currentDirection;

        public float ForwardStep;
        public float RotationStep;
        
        public TurtlePen(GeometryRenderSystem renderSystem)
        {
            _positionStack = new Stack<Vector3>();
            _directionStack = new Stack<Vector3>();
            _renderSystem = renderSystem;
        }

        public void Draw(Vector3 startingPosition, string commandString)
        {
            _renderSystem.ClearObjects();
            _currentPosition = startingPosition;
            _currentDirection = Vector3.up;
            _rightVector = Vector3.right;

            foreach (var command in commandString)
            {
                switch (command)
                {
                    case 'F':
                        MoveForward();
                        break;
                    case 'L':
                        DrawLeaf();
                        break;
                    case '+':
                        TurnLeft();
                        //IncreaseVerticalAxis();
                        //IncreaseRollAxis();
                        //IncreaseLateralAxis();
                        break;
                    case '-':
                        TurnRight();
                        //DecreaseVerticalAxis();
                        //DecreaseRollAxis();
                        //DecreaseLateralAxis();
                        break;
                    case '&':
                        PitchDown();
                        //IncreaseVerticalAxis();
                        //IncreaseRollAxis();
                        //IncreaseLateralAxis();
                        break;
                    case '^':
                        PitchUp();
                        //DecreaseVerticalAxis();
                        //DecreaseRollAxis();
                        //DecreaseLateralAxis();
                        break;
                    case '\\':
                        RollRight();
                        //IncreaseVerticalAxis();
                        //IncreaseRollAxis();
                        //IncreaseLateralAxis();
                        break;
                    case '/':
                        RollLeft();
                        //DecreaseVerticalAxis();
                        //DecreaseRollAxis();
                        //DecreaseLateralAxis();
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
            _currentPosition += ForwardStep * _currentDirection;
            _renderSystem.DrawCylinder(lastPosition, _currentPosition, 0.01f);
            //_renderSystem.DrawSphere(_currentPosition, 0.01f);
            //_renderSystem.DrawSphere(lastPosition, 0.01f);
        }

        private void DrawLeaf()
        {
            _renderSystem.DrawSphere(_currentPosition, 0.1f);
        }

        private void TurnRight()
        {
            Vector3 axis = Vector3.Cross(_currentDirection, _rightVector);
            var angleAxis = Quaternion.AngleAxis(RotationStep, axis);
            _currentDirection = angleAxis * _currentDirection;
            _rightVector = angleAxis * _rightVector;

            _currentDirection.Normalize();
            _rightVector.Normalize();
        }

        private void TurnLeft()
        {
            Vector3 axis = Vector3.Cross(_currentDirection, _rightVector);
            var angleAxis = Quaternion.AngleAxis(-RotationStep, axis);
            _currentDirection = angleAxis * _currentDirection;
            _rightVector = angleAxis * _rightVector;

            _currentDirection.Normalize();
            _rightVector.Normalize();
        }

        private void PitchUp()
        {
            _currentDirection = Quaternion.AngleAxis(RotationStep, _rightVector) * _currentDirection;
            _currentDirection.Normalize();
        }

        private void PitchDown()
        {
            _currentDirection = Quaternion.AngleAxis(-RotationStep, _rightVector) * _currentDirection;
            _currentDirection.Normalize();
        }

        private void RollRight()
        {
            _rightVector = Quaternion.AngleAxis(RotationStep, _currentDirection) * _rightVector;
            _rightVector.Normalize();
        }

        private void RollLeft()
        {
            _rightVector = Quaternion.AngleAxis(-RotationStep, _currentDirection) * _rightVector;
            _rightVector.Normalize();
        }

        private void PushTransformation()
        {
            _positionStack.Push(_currentPosition);
            _directionStack.Push(_currentDirection);
        }

        private void PopTransformation()
        {
            _currentPosition = _positionStack.Pop();
            _currentDirection = _directionStack.Pop();
        }
    }
}
