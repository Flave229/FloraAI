using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Render;
using UnityEngine;
using Assets.Scripts.Common.MathHelper;
using Random = System.Random;

namespace Assets.Scripts.TurtleGeometry
{
    public class TurtlePen
    {
        private Vector3 _rightVector;

        private readonly GeometryRenderSystem _renderSystem;
        private readonly Random _randomGenerator;
        private readonly Stack<Vector3> _positionStack;
        private readonly Stack<Vector3> _directionStack;
        private readonly Stack<Color> _colorStack;
        private readonly Stack<float> _branchDiameterStack;
        private Vector3 _currentPosition;
        private Vector3 _currentDirection;
        private float _currentBranchDiameter;
        private Color _currentColor;

        public float ForwardStep { get; set; }
        public float RotationStep { get; set; }
        public float BranchDiameter { get; set; }
        public MinMax<float> BranchReductionRate { get; set; }

        public TurtlePen(GeometryRenderSystem renderSystem)
        {
            _positionStack = new Stack<Vector3>();
            _directionStack = new Stack<Vector3>();
            _branchDiameterStack = new Stack<float>();
            _colorStack = new Stack<Color>();
            _renderSystem = renderSystem;
            _randomGenerator = new Random();
        }


        public void Draw(Vector3 startingPosition, string commandString)
        {
            _renderSystem.ClearObjects();
            _currentPosition = startingPosition;
            _currentBranchDiameter = BranchDiameter;
            _currentDirection = Vector3.up;
            _rightVector = Vector3.right;
            _currentColor = new Color(0.0f, 0.1f, 0.0f);

            foreach (var command in commandString)
            {
                switch (command)
                {
                    case 'F':
                        MoveForward();
                        break;
                    case 'O':
                        DrawLeaf();
                        break;
                    case '+':
                        TurnLeft();
                        break;
                    case '-':
                        TurnRight();
                        break;
                    case '&':
                        PitchDown();
                        break;
                    case '^':
                        PitchUp();
                        break;
                    case '\\':
                        RollRight();
                        break;
                    case '/':
                        RollLeft();
                        break;
                    case '!':
                        DecreaseBranchDiameter();
                        break;
                    case '\'':
                        IncreaseGreenValue();
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

        private void DecreaseBranchDiameter()
        {
            double randomNumber = _randomGenerator.Next((int) (BranchReductionRate.Min * 100), (int) (BranchReductionRate.Max * 100));
            _currentBranchDiameter *= (float)(randomNumber / 100);
        }

        private void MoveForward()
        {
            var lastPosition = _currentPosition;
            _currentPosition += ForwardStep * _currentDirection;
            _renderSystem.DrawCylinder(lastPosition, _currentPosition, _currentBranchDiameter);
        }

        private void DrawLeaf()
        {
            _renderSystem.DrawQuad(_currentPosition + ((ForwardStep) * _currentDirection), _currentDirection, _currentColor);
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

        private void IncreaseGreenValue()
        {
            if (_currentColor.g + 0.05f > 1.0f)
            {
                _currentColor.g = 1.0f;
                return;
            }

            _currentColor.g = _currentColor.g + 0.05f;
        }

        private void PushTransformation()
        {
            _positionStack.Push(_currentPosition);
            _directionStack.Push(_currentDirection);
            _branchDiameterStack.Push(_currentBranchDiameter);
            _colorStack.Push(_currentColor);
        }

        private void PopTransformation()
        {
            _currentPosition = _positionStack.Pop();
            _currentDirection = _directionStack.Pop();
            _currentBranchDiameter = _branchDiameterStack.Pop();
            _currentColor = _colorStack.Pop();
        }
    }
}
