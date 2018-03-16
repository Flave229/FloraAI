using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Render;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.TurtleGeometry
{
    public class TurtlePen
    {
        private readonly IRenderSystem _renderSystem;
        private readonly Random _randomGenerator;
        private readonly Stack<Vector3> _positionStack;
        private readonly Stack<Quaternion> _rotationStack;
        private readonly Stack<Color> _colorStack;
        private readonly Stack<float> _branchDiameterStack;
        private Vector3 _currentPosition;
        private Quaternion _currentRotation;
        private float _currentBranchDiameter;
        private Color _currentColor;

        private Quaternion _increaseYawQuaternion;
        private Quaternion _decreaseYawQuaternion;
        private Quaternion _increaseRollQuaternion;
        private Quaternion _decreaseRollQuaternion;
        private Quaternion _increasePitchQuaternion;
        private Quaternion _decreasePitchQuaternion;
        private Vector3 _lastMovementDirection;
        private int _forwardStepMultiplication;

        public float ForwardStep { get; set; }
        public float RotationStep { get; set; }
        public float BranchDiameter { get; set; }
        public MinMax<float> BranchReductionRate { get; set; }

        public TurtlePen(IRenderSystem renderSystem)
        {
            _positionStack = new Stack<Vector3>();
            _rotationStack = new Stack<Quaternion>();
            _branchDiameterStack = new Stack<float>();
            _colorStack = new Stack<Color>();
            _renderSystem = renderSystem;
            _randomGenerator = new Random();
        }

        private void SetupQuaternions()
        {
            _increaseYawQuaternion = Quaternion.AngleAxis(RotationStep, new Vector3(0, 0, 1));
            _decreaseYawQuaternion = Quaternion.AngleAxis(-RotationStep, new Vector3(0, 0, 1));
            _increaseRollQuaternion = Quaternion.AngleAxis(RotationStep, new Vector3(0, 1, 0));
            _decreaseRollQuaternion = Quaternion.AngleAxis(-RotationStep, new Vector3(0, 1, 0));
            _increasePitchQuaternion = Quaternion.AngleAxis(RotationStep, new Vector3(1, 0, 0));
            _decreasePitchQuaternion = Quaternion.AngleAxis(-RotationStep, new Vector3(1, 0, 0));
        }
        
        public void Draw(PersistentPlantGeometryStorage geometryStorage, Vector3 startingPosition, string commandString)
        {
            SetupQuaternions();
            geometryStorage.StartPlant();
            _renderSystem.ClearObjects();
            _currentPosition = startingPosition;
            _currentRotation = Quaternion.identity;
            _currentBranchDiameter = BranchDiameter;
            _currentColor = new Color(0.0f, 0.1f, 0.0f);
            _lastMovementDirection = Vector3.zero;
            _forwardStepMultiplication = 1;

            foreach (var command in commandString)
            {
                switch (command)
                {
                    case 'F':
                        MoveForward(geometryStorage);
                        break;
                    case 'O':
                        DrawLeaf(geometryStorage);
                        break;
                    case '+':
                        TurnLeft();
                        break;
                    case '-':
                        TurnRight();
                        break;
                    case '&':
                        PitchUp();
                        break;
                    case '^':
                        PitchDown();
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
                        PushTransformation(geometryStorage);
                        break;
                    case ']':
                        PopTransformation(geometryStorage);
                        break;
                }
            }

            _renderSystem.FinalisePlant();
        }

        private void DecreaseBranchDiameter()
        {
            double randomNumber = _randomGenerator.Next((int) (BranchReductionRate.Min * 100), (int) (BranchReductionRate.Max * 100));
            _currentBranchDiameter *= (float)(randomNumber / 100);
        }

        private void MoveForward(PersistentPlantGeometryStorage geometryStorage)
        {
            Vector3 currentDirection = GetDirection();
            if (currentDirection == _lastMovementDirection)
            {
                ++_forwardStepMultiplication;
                return;
            }

            var lastPosition = _currentPosition;
            _currentPosition += (ForwardStep * _forwardStepMultiplication) * currentDirection;
            _renderSystem.DrawCylinder(lastPosition, _currentPosition, _currentBranchDiameter);
            geometryStorage.ExtendBranch(_currentBranchDiameter);
            
            _lastMovementDirection = currentDirection;
            _forwardStepMultiplication = 1;
        }

        private void DrawLeaf(PersistentPlantGeometryStorage geometryStorage)
        {
            Vector3 leafPosition = _currentPosition + ((ForwardStep) * GetDirection());
            Vector3 rightVector = Vector3.zero;
            _renderSystem.DrawQuad(leafPosition, GetDirection(), _currentColor, ref rightVector);
            geometryStorage.StoreLeaf(leafPosition, rightVector);
        }

        private void TurnRight()
        {
            _currentRotation *= _increaseYawQuaternion;
        }

        private void TurnLeft()
        {
            _currentRotation *= _decreaseYawQuaternion;
        }

        private void PitchUp()
        {
            _currentRotation *= _increasePitchQuaternion;
        }

        private void PitchDown()
        {
            _currentRotation *= _decreasePitchQuaternion;
        }

        private void RollRight()
        {
            _currentRotation *= _increaseRollQuaternion;
        }

        private void RollLeft()
        {
            _currentRotation *= _decreaseRollQuaternion;
        }

        private Vector3 GetDirection()
        {
            return _currentRotation * Vector3.up;
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

        private void PushTransformation(PersistentPlantGeometryStorage geometryStorage)
        {
            _positionStack.Push(_currentPosition);
            _rotationStack.Push(_currentRotation);
            _branchDiameterStack.Push(_currentBranchDiameter);
            _colorStack.Push(_currentColor);
            geometryStorage.StartNewBranch();
            
        }

        private void PopTransformation(PersistentPlantGeometryStorage geometryStorage)
        {
            _currentPosition = _positionStack.Pop();
            _currentRotation = _rotationStack.Pop();
            _currentBranchDiameter = _branchDiameterStack.Pop();
            _currentColor = _colorStack.Pop();
            geometryStorage.ReturnToLastBranch();
        }
    }
}