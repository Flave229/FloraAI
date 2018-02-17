using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.LSystems
{
    public class RuleSet
    {
        public readonly Dictionary<string, List<LSystemRule>> Rules;

        public RuleSet(Dictionary<string, List<LSystemRule>> rules)
        {
            Rules = rules;
        }

        public string GetRule(string key)
        {
            var randomGenerator = new Random();
            var randomNumber = randomGenerator.NextDouble();
            double probabilityTotal = 0;

            if (Rules.ContainsKey(key) == false)
                return key;

            for (int i = 0; i < Rules[key].Count; ++i)
            {
                var ruleProbability = Rules[key][i].Probability;
                if (randomNumber < ruleProbability + probabilityTotal)
                    return Rules[key][i].Rule;

                probabilityTotal += randomNumber;
            }

            Debug.LogError("Critical: Failed to get rule for LSystem. Please check probabilities are correctly initialised");
            return "Error";
        }
    }
}
