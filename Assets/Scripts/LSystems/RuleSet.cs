using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.LSystems
{
    public class RuleSet
    {
        private readonly Dictionary<string, List<LSystemRule>> _rules;

        public RuleSet(Dictionary<string, List<LSystemRule>> rules)
        {
            _rules = rules;
        }

        public string GetRule(string key)
        {
            var randomGenerator = new Random();
            var randomNumber = randomGenerator.NextDouble();
            double probabilityTotal = 0;

            if (_rules.ContainsKey(key) == false)
                return key;

            for (int i = 0; i < _rules[key].Count; ++i)
            {
                var ruleProbability = _rules[key][i].Probability;
                if (randomNumber < ruleProbability + probabilityTotal)
                    return _rules[key][i].Rule;

                probabilityTotal += randomNumber;
            }

            Debug.LogError("Critical: Failed to get rule for LSystem. Please check probabilities are correctly initialised");
            return "Error";
        }
    }
}
