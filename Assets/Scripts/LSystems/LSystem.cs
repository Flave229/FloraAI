using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts.LSystems
{
    public class LSystem
    {
        private string _currentString;
        private readonly TurtlePen _turtlePen;
        private RuleSet _rules;

        public LSystem(TurtlePen turtlePen, RuleSet rules, string axiom)
        {
            _currentString = axiom;
            _turtlePen = turtlePen;
            _rules = rules;
        }

        public void Update()
        {
            var newString = "";

            for (int i = 0, length = _currentString.Length; i < length; ++i)
                newString += _rules.GetRule(_currentString[i].ToString());

            _currentString = newString;
        }

        public void Draw(Vector3 startingPosition)
        {
            _turtlePen.Draw(startingPosition, _currentString);
        }
    }
}
