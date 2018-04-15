using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class DebugLogger : MonoBehaviour
    {
        private PlantSpawner[] _plantSpawners;
        private Text _debugOutput;
        private int _currentIndex;

        void Start()
        {
            _plantSpawners = FindObjectsOfType<PlantSpawner>();
            _debugOutput = GameObject.Find("DebugOutput").GetComponent<Text>();
            _currentIndex = 0;
        }

        void Update()
        {
            bool changed = false;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_currentIndex <= 0)
                    _currentIndex = _plantSpawners.Length;
                else
                    --_currentIndex;

                changed = true;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_currentIndex >= _plantSpawners.Length - 1)
                    _currentIndex = 0;
                else
                    ++_currentIndex;

                changed = true;
            }

            if (changed == false)
                return;

            Plant plantToShowInfoOn = _plantSpawners[_currentIndex].GetFittestPlant();
            _debugOutput.text = "Looking at Plant " + _currentIndex + "\n";
            foreach (var rule in plantToShowInfoOn.LindenMayerSystem.GetRuleSet().Rules)
            {
                _debugOutput.text += "Rule " + rule.Key + ": " + rule.Value[0].Rule + "\n";
            }
            //_debugOutput.text += "Total Command: " + fittestPlant.LindenMayerSystem.GetCommandString() + "\n";
            _debugOutput.text += "Total Leaf Energy: " + plantToShowInfoOn.Fitness.LeafEnergy + "\n";
            _debugOutput.text += "Total Branch Cost: " + plantToShowInfoOn.Fitness.BranchCost + "\n";
            _debugOutput.text += "Total Branch Amount: " + plantToShowInfoOn.Fitness.BranchCount + "\n";
            _debugOutput.text += "Total Leaf Amount: " + plantToShowInfoOn.Fitness.LeafCount + "\n";
            _debugOutput.text += "Total Energy Loss: " + plantToShowInfoOn.Fitness.EnergyLoss + "\n";
            _debugOutput.text += "Total branches that were too thin: " + plantToShowInfoOn.Fitness.BranchesTooThin +
                                 "\n";
            Color sunColour = FindObjectOfType<Light>().color;
            _debugOutput.text += "Total Fitness: " + plantToShowInfoOn.Fitness.TotalFitness(
                                     new Vector3(sunColour.r / (670 / 437.5f), sunColour.g / (532.5f / 437.5f),
                                         sunColour.b));
        }
    }
}
