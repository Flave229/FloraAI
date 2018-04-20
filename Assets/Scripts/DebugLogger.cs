using System.Linq;
using Assets.Scripts.Export;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class DebugLogger : MonoBehaviour
    {
        private PlantSpawner[] _plantSpawners;
        private Text _debugOutput;
        private int _currentIndex;
        private Text _sunOutput;
        private Image _sunPanel;
        private Material _dirtColour;
        private Material _selectedColour;
        private Text _redInput;
        private Text _greenInput;
        private Text _blueInput;
        private float _previousRed;
        private float _previousGreen;
        private float _previousBlue;
        private Light _light;

        void Start()
        {
            _plantSpawners = FindObjectsOfType<PlantSpawner>().OrderBy(x => x.transform.GetSiblingIndex()).ToArray();
            _debugOutput = GameObject.Find("DebugOutput").GetComponent<Text>();
            _light = FindObjectOfType<Light>();
            Vector3 color = new Vector3(_light.color.r, _light.color.g, _light.color.b);
            _currentIndex = 0;
            _sunPanel = GameObject.Find("SunPanel").GetComponent<Image>();
            _sunPanel.color = _light.color;

            _dirtColour = Resources.Load<Material>("Material/Dirt");
            _selectedColour = Resources.Load<Material>("Material/SelectedDirt");
            _redInput = GameObject.Find("RedInput").transform.Find("Text").GetComponent<Text>();
            _previousRed = color.x;
            _greenInput = GameObject.Find("GreenInput").transform.Find("Text").GetComponent<Text>();
            _previousGreen = color.y;
            _blueInput = GameObject.Find("BlueInput").transform.Find("Text").GetComponent<Text>();
            _previousBlue = color.z;
        }

        void Update()
        {
            if (_redInput.text != "" && float.Parse(_redInput.text) != _previousRed)
            {
                _previousRed = float.Parse(_redInput.text);
                _light.color = new Color(_previousRed, _previousGreen, _previousBlue);
                _sunPanel.color = _light.color;

                foreach (var plantSpawner in _plantSpawners)
                    plantSpawner.UpdateSunColour();
            }
            if (_greenInput.text != "" && float.Parse(_greenInput.text) != _previousGreen)
            {
                _previousGreen = float.Parse(_greenInput.text);
                _light.color = new Color(_previousRed, _previousGreen, _previousBlue);
                _sunPanel.color = _light.color;

                foreach (var plantSpawner in _plantSpawners)
                    plantSpawner.UpdateSunColour();
            }
            if (_blueInput.text != "" && float.Parse(_blueInput.text) != _previousBlue)
            {
                _previousBlue = float.Parse(_blueInput.text);
                _light.color = new Color(_previousRed, _previousGreen, _previousBlue);
                _sunPanel.color = _light.color;

                foreach (var plantSpawner in _plantSpawners)
                    plantSpawner.UpdateSunColour();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();

            if (Input.GetKeyDown(KeyCode.U))
            {
                var canvas = GameObject.Find("Canvas");
                foreach (Transform child in canvas.transform)
                {
                    if (canvas.transform == child)
                        continue;

                    child.gameObject.SetActive(!child.gameObject.activeSelf);
                }
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                string fileLocation = ObjExporter.ExportGameObject(_plantSpawners[_currentIndex].GetFittestPlant().RenderedGeometry);
                _debugOutput.text = "Saved plant to: " + fileLocation;
            }

            bool changed = false;
            int previousIndex = _currentIndex;
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_currentIndex <= 0)
                    _currentIndex = _plantSpawners.Length - 1;
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

            foreach (Transform dirtObject in _plantSpawners[_currentIndex].transform)
            {
                if (dirtObject == _plantSpawners[_currentIndex].transform)
                    continue;

                dirtObject.GetComponent<Renderer>().material = _selectedColour;
            }

            foreach (Transform dirtObject in _plantSpawners[previousIndex].transform)
            {
                if (dirtObject == _plantSpawners[previousIndex].transform)
                    continue;

                dirtObject.GetComponent<Renderer>().material = _dirtColour;
            }

            Plant plantToShowInfoOn = _plantSpawners[_currentIndex].GetFittestPlant();
            _debugOutput.text = "Looking at Plant " + _currentIndex + "\n";
            foreach (var rule in plantToShowInfoOn.LindenMayerSystem.GetRuleSet().Rules)
            {
                _debugOutput.text += "Rule " + rule.Key + ": " + rule.Value[0].Rule + "\n";
            }
            Color sunColour = FindObjectOfType<Light>().color;
            //_debugOutput.text += "Total Command: " + fittestPlant.LindenMayerSystem.GetCommandString() + "\n";
            _debugOutput.text += "Total Leaf Energy: " + plantToShowInfoOn.Fitness.LeafEnergy + "\n";
            _debugOutput.text += "Total Branch Cost: " + plantToShowInfoOn.Fitness.BranchCost + "\n";
            _debugOutput.text += "Total Branch Amount: " + plantToShowInfoOn.Fitness.BranchCount + "\n";
            _debugOutput.text += "Total Leaf Amount: " + plantToShowInfoOn.Fitness.LeafCount + "\n";
            _debugOutput.text += "Leaf Colour: " + plantToShowInfoOn.Fitness.LeafColour + "\n";
            var sunEnergyWeightings = new Vector3(Mathf.Pow(sunColour.r / (670 / 437.5f) * 4.1f, 2), Mathf.Pow(sunColour.g / (532.5f / 437.5f) * 3, 2), Mathf.Pow(sunColour.b * 2.9f, 2)).normalized;
            _debugOutput.text += "Sun Energy Weightings: " + sunEnergyWeightings + "\n";
            _debugOutput.text += "Leaf Energy Weightings: " +
                                 plantToShowInfoOn.Fitness.LeafEnergyWeightings(sunEnergyWeightings) + "\n";
            _debugOutput.text += "Colour Energy: " +
                                 plantToShowInfoOn.Fitness.CalculateColourEnergyFactor(sunEnergyWeightings) + "\n";
            _debugOutput.text += "Total Energy Loss: " + plantToShowInfoOn.Fitness.EnergyLoss + "\n";
            _debugOutput.text += "Total branches that were too thin: " + plantToShowInfoOn.Fitness.BranchesTooThin +
                                 "\n";
            _debugOutput.text += "Total Fitness: " + plantToShowInfoOn.Fitness.TotalFitness(
                                     sunEnergyWeightings);
        }
    }
}
