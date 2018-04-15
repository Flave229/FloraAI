using System;
using System.Collections.Generic;

namespace Assets.Scripts.LSystems
{
    public class LSystemGenerator
    {
        private readonly Random _randomGenerator;
        private List<string> _avaliableRuleKeys;
        private List<string> _avaliableCharacters;

        public LSystemGenerator(Random randomGenerator)
        {
            _randomGenerator = randomGenerator;
            _avaliableRuleKeys = new List<string> { "L", "S", "A", "F" };
            _avaliableCharacters = new List<string> { "F", "L", "S", "A", "+", "-", "&", "^", "\\", "/", "!" };
        }

        public LSystem GenerateRandomLSystem()
        {
            RuleSet randomRuleSet = GenerateRandomRules();
            return new LSystem(randomRuleSet, "A");
        }

        private RuleSet GenerateRandomRules()
        {
            Dictionary<string, List<LSystemRule>> randomRules = new Dictionary<string, List<LSystemRule>>();

            foreach (var ruleKey in _avaliableRuleKeys)
            {
                List<string> additionalCharacters = new List<string>();
                List<string> removableCharacters = new List<string>();
                if (ruleKey == "L")
                {
                    additionalCharacters.AddRange(new List<string> { "O", "O", "O" });
                    removableCharacters.AddRange(new List<string> { "S", "A", "F", "L" });
                }

                randomRules.Add(ruleKey, new List<LSystemRule>
                {
                    new LSystemRule
                    {
                        Rule = GenerateLSystemRule(additionalCharacters, removableCharacters),
                        Probability = 1
                    }
                });
            }

            return new RuleSet(randomRules);
        }
    

        private string GenerateLSystemRule(List<string> additionalCharacters, List<string> removableCharacters)
        {
            // Random amount of character between 2 and 20
            // Brackets count as 1 character
            string randomRule = "";
            int amountOfCharacters = _randomGenerator.Next(2, 8);
            int totalBracketCount = amountOfCharacters / 5;
            int bracketCount = 0;
            int characterCount = 0;

            for (int i = 0; i < amountOfCharacters; ++i)
            {
                int randomCharacterIndex;
                if (bracketCount >= totalBracketCount)
                    randomCharacterIndex = _randomGenerator.Next(0, _avaliableCharacters.Count);
                else
                    randomCharacterIndex = _randomGenerator.Next(0, _avaliableCharacters.Count + 1);

                if (randomCharacterIndex == _avaliableCharacters.Count)
                {
                    ++bracketCount;
                    randomRule += GenerateRandomBracket(additionalCharacters, removableCharacters);
                }
                else
                    randomRule += PickRuleFromAdjustedCharacterSet(randomRule, additionalCharacters, removableCharacters);
            }

            return randomRule;
        }

        private string GenerateRandomBracket(List<string> additionalCharacters, List<string> removableCharacters)
        {
            // Inside brackets contains between 1 and 4 characters
            string randomBracket = "[";
            int amountOfCharacters = _randomGenerator.Next(1, 5);

            for (int i = 0; i < amountOfCharacters; i++)
            {
                randomBracket += PickRuleFromAdjustedCharacterSet(randomBracket, additionalCharacters, removableCharacters);
            }

            return randomBracket + "]";
        }

        private string PickRuleFromAdjustedCharacterSet(string currentRule, List<string> additionalCharacters, List<string> removableCharacters)
        {
            List<string> adjustedCharacters = new List<string>(_avaliableCharacters);
            adjustedCharacters.AddRange(additionalCharacters);

            foreach (var character in removableCharacters)
            {
                adjustedCharacters.Remove(character);
            }

            if (currentRule.Length > 0)
            {
                switch (currentRule[currentRule.Length - 1])
                {
                    case '+':
                        adjustedCharacters.Remove("-");
                        break;
                    case '-':
                        adjustedCharacters.Remove("+");
                        break;
                    case '&':
                        adjustedCharacters.Remove("^");
                        break;
                    case '^':
                        adjustedCharacters.Remove("&");
                        break;
                    case '\\':
                        adjustedCharacters.Remove("/");
                        break;
                    case '/':
                        adjustedCharacters.Remove("\\");
                        break;
                }
            }

            int randomIndex = _randomGenerator.Next(0, adjustedCharacters.Count);
            return adjustedCharacters[randomIndex];
        }
    }
}