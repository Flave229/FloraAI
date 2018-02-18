using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LSystems;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantCrossOver
    {
        private readonly Random _randomGenerator;

        public PlantCrossOver(Random randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public RuleSet CrossOver(RuleSet leftParentRuleSet, RuleSet rightParentRuleSet)
        {
            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>();

            foreach (var leftParentRule in leftParentRuleSet.Rules)
            {
                if (rightParentRuleSet.Rules.ContainsKey(leftParentRule.Key) == false)
                {
                    rules.Add(leftParentRule.Key, leftParentRule.Value);
                    continue;
                }

                List<string> avaliableRules = new List<string>
                {
                    leftParentRuleSet.Rules[leftParentRule.Key][0].Rule,
                    rightParentRuleSet.Rules[leftParentRule.Key][0].Rule
                };

                int randomIndex = _randomGenerator.Next(0, 2);
                List<string> rulesToPotentiallyReplace = GetLowestBracketedRules(avaliableRules[randomIndex]);
                List<string> rulesToReplaceWith = GetLowestBracketedRules(avaliableRules[randomIndex == 1 ? 0 : 1]);

                string crossOverRule = CrossOverLowestBracketHierarchy(avaliableRules[randomIndex], rulesToPotentiallyReplace, rulesToReplaceWith);

                rules.Add(leftParentRule.Key, new List<LSystemRule>
                {
                    new LSystemRule
                    {
                        Probability = 1,
                        Rule = crossOverRule
                    }
                });
            }

            Dictionary<string, List<LSystemRule>> rulesNotIncludedOnLeftSide = rightParentRuleSet.Rules
                .Where(rightRule => leftParentRuleSet.Rules.ContainsKey(rightRule.Key) == false)
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var rightParentRule in rulesNotIncludedOnLeftSide)
            {
                rules.Add(rightParentRule.Key, rightParentRule.Value);
            }

            return new RuleSet(rules);
        }

        private List<string> GetLowestBracketedRules(string rule)
        {
            int startIndex = 0;
            return GetLowestBracketedRules(rule, ref startIndex);
        }

        private List<string> GetLowestBracketedRules(string rule, ref int currentIndex)
        {
            bool isLowestLevel = true;
            List<string> lowestLevelSubstrings = new List<string>();
            string currentString = "";

            for (; currentIndex < rule.Length; ++currentIndex)
            {
                if (rule[currentIndex] == '[')
                {
                    if (isLowestLevel)
                    {
                        isLowestLevel = false;
                        lowestLevelSubstrings = new List<string>();
                    }

                    ++currentIndex;
                    lowestLevelSubstrings.AddRange(GetLowestBracketedRules(rule, ref currentIndex));
                }
                else if (rule[currentIndex] == ']')
                {
                    if (isLowestLevel)
                    {
                        lowestLevelSubstrings.Add(currentString);
                    }
                    break;
                }
                else
                {
                    currentString += rule[currentIndex];
                }
            }

            return lowestLevelSubstrings;
        }

        private string CrossOverLowestBracketHierarchy(string rule, List<string> rulesToPotentiallyReplace, List<string> rulesToReplaceWith)
        {
            if (rulesToPotentiallyReplace.Count == 0 || rulesToReplaceWith.Count == 0)
                return rule;

            int amountOfSwaps = _randomGenerator.Next(1, rulesToPotentiallyReplace.Count);

            for (int i = 0; i < amountOfSwaps; ++i)
            {
                var indexOfRuleToReplaceWith = _randomGenerator.Next(0, rulesToReplaceWith.Count);
                var indexOfRuleToReplace = _randomGenerator.Next(0, rulesToPotentiallyReplace.Count);

                string randomReplacementRule = rulesToReplaceWith[indexOfRuleToReplaceWith];
                string randomRuleToReplace = rulesToPotentiallyReplace[indexOfRuleToReplace];
                rule = rule.Replace("[" + randomRuleToReplace + "]", "[" + randomReplacementRule + "]");

                rulesToReplaceWith.RemoveAt(indexOfRuleToReplaceWith);
                rulesToPotentiallyReplace.RemoveAt(indexOfRuleToReplace);

                if (rulesToPotentiallyReplace.Count <= 0)
                    break;
            }

            return rule;
        }
    }
}