using System;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.LSystems
{
    public interface ILSystem
    {
        void Iterate();
        string GetCommandString();
        RuleSet GetRuleSet();
        void ClearCommandString();
        Color GetLeafColor();
        void SetLeafColour(Color leafColour);
    }

    public class LSystem : ILSystem
    {
        private string _currentString;
        private readonly RuleSet _rules;
        public Color LeafColour;

        public LSystem(RuleSet rules, string axiom)
        {
            _currentString = axiom;
            _rules = rules;
        }

        public void Iterate()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0, length = _currentString.Length; i < length; ++i)
                    stringBuilder.Append(_rules.GetRule(_currentString[i].ToString()));

                _currentString = stringBuilder.ToString();
            }
            catch (Exception e)
            {
                Exception newException = new Exception("Error occured in LSystem Iteration", e);
                throw newException;
            }
        }

        public string GetCommandString()
        {
            return _currentString;
        }

        public RuleSet GetRuleSet()
        {
            return _rules;
        }

        public void ClearCommandString()
        {
            _currentString = "";
        }

        public Color GetLeafColor()
        {
            return LeafColour;
        }

        public void SetLeafColour(Color leafColour)
        {
            LeafColour = leafColour;
        }
    }
}