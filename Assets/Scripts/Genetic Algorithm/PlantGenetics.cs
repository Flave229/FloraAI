﻿using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LSystems;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantGenetics
    {
        private Random _randomGenerator;

        public PlantGenetics()
        {
            _randomGenerator = new Random();
        }

        public RuleSet CrossOver(RuleSet leftParentRuleSets, RuleSet rightParentRuleSets)
        {
            List<string> avaliableRules = new List<string>
            {
                leftParentRuleSets.Rules["F"][0].Rule,
                rightParentRuleSets.Rules["F"][0].Rule
            };

            int randomIndex = _randomGenerator.Next(0, 2);
            List<string> rulesToPotentiallyReplace = GetLowestBracketedRules(avaliableRules[randomIndex]);
            List<string> rulesToReplaceWith = GetLowestBracketedRules(avaliableRules[randomIndex == 1 ? 0 : 1]);

            string crossOverRule = CrossOverLowestBracketHierarchy(avaliableRules[randomIndex], rulesToPotentiallyReplace, rulesToReplaceWith);

            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>
            {
                {
                    "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = crossOverRule
                        }
                    }
                }
            };

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