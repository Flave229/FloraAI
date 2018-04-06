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
    }

    public class LSystem : ILSystem
    {
        private string _currentString;
        private readonly RuleSet _rules;

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
                foreach (var rule in _rules.Rules)
                {
                    Debug.Log(rule.Value + ": " + rule.Value[0].Rule);
                }
                throw e;
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
    }
}