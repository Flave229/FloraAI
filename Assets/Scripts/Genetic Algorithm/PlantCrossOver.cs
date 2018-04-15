using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.LSystems;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantCrossOver
    {
        private readonly Random _randomGenerator;

        public PlantCrossOver(Random randomGenerator)
        {
            _randomGenerator = randomGenerator;
        }

        public RuleSet CrossOverV2(RuleSet leftParentRuleSet, RuleSet rightParentRuleSet)
        {
            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>();

            foreach (var leftParentRule in leftParentRuleSet.Rules)
            {
                if (rightParentRuleSet.Rules.ContainsKey(leftParentRule.Key) == false)
                {
                    rules.Add(leftParentRule.Key, new List<LSystemRule>(leftParentRule.Value));
                    continue;
                }

                string leftRuleString = leftParentRule.Value[0].Rule;
                string rightRuleString = rightParentRuleSet.Rules[leftParentRule.Key][0].Rule;
                Dictionary<int, List<string>> allHierarchicalRules = SeperateBracketHierarchy(leftRuleString); // Only accepts one possibility
                Dictionary<int, List<string>> hierarchicalBracketedRightRules = SeperateBracketHierarchy(rightRuleString); // Only accepts one possibility

                foreach (var rightRule in hierarchicalBracketedRightRules)
                {
                    if (allHierarchicalRules.ContainsKey(rightRule.Key))
                        allHierarchicalRules[rightRule.Key].AddRange(rightRule.Value);
                    else
                        allHierarchicalRules.Add(rightRule.Key, rightRule.Value);
                }

                string crossOverRule = CrossOverCompleteBracketHierarchy(allHierarchicalRules);
                crossOverRule = RemoveGeneticRedundancies(crossOverRule);

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

        public Color CrossOverLeafColour(Color leftParentColour, Color rightParentColour)
        {
            float randomWeighting = (float)_randomGenerator.NextDouble();
            return new Color(Mathf.Lerp(leftParentColour.r, rightParentColour.r, randomWeighting), Mathf.Lerp(leftParentColour.g, rightParentColour.g, randomWeighting), Mathf.Lerp(leftParentColour.b, rightParentColour.b, randomWeighting));
        }

        public string RemoveGeneticRedundancies(string rule)
        {
            for (int i = 0; i < rule.Length - 1; ++i)
            {
                switch (rule[i])
                {
                    case '[':
                        if (rule[i + 1] == ']')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '+':
                        if (rule[i + 1] == '-')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '-':
                        if (rule[i + 1] == '+')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '&':
                        if (rule[i + 1] == '^')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '^':
                        if (rule[i + 1] == '&')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '\\':
                        if (rule[i + 1] == '/')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                    case '/':
                        if (rule[i + 1] == '\\')
                        {
                            rule = rule.Remove(i, 2);
                            --i;
                        }
                        break;
                }
            }
            return rule;
        }

        private Dictionary<int, List<string>> SeperateBracketHierarchy(string rule)
        {
            int stringIndex = 0;
            int bracketLayer = 0;
            return SeperateBracketHierarchy(rule, ref stringIndex, ref bracketLayer);
        }

        private Dictionary<int, List<string>> SeperateBracketHierarchy(string rule, ref int stringIndex, ref int bracketsDeep)
        {
            Dictionary<int, List<string>> bracketHierarchy = new Dictionary<int, List<string>>();

            string ruleOnThisLayer = "";
            int deepestBracketInside = 0;
            for (; stringIndex < rule.Length; ++stringIndex)
            {
                char character = rule[stringIndex];
                switch (character)
                {
                    case '[':
                        ++stringIndex;
                        ruleOnThisLayer += "[%]";
                        int bracketsInside = 0;
                        Dictionary<int, List<string>> lowerLevelBracketHierarchy = SeperateBracketHierarchy(rule, ref stringIndex, ref bracketsInside);
                        
                        foreach (var subRule in lowerLevelBracketHierarchy)
                        {
                            if (bracketHierarchy.ContainsKey(subRule.Key))
                                bracketHierarchy[subRule.Key].AddRange(subRule.Value);
                            else
                                bracketHierarchy.Add(subRule.Key, subRule.Value);
                        }

                        if (bracketsInside > deepestBracketInside)
                            deepestBracketInside = bracketsInside;

                        break;
                    case ']':
                        if (bracketHierarchy.ContainsKey(deepestBracketInside))
                            bracketHierarchy[deepestBracketInside].Add(ruleOnThisLayer);
                        else
                            bracketHierarchy.Add(deepestBracketInside, new List<string> { ruleOnThisLayer });

                        bracketsDeep = deepestBracketInside + 1;
                        return bracketHierarchy;
                    default:
                        ruleOnThisLayer += character;
                        break;
                }
            }
            
            if (bracketHierarchy.ContainsKey(deepestBracketInside))
                bracketHierarchy[deepestBracketInside].Add(ruleOnThisLayer);
            else
                bracketHierarchy.Add(deepestBracketInside, new List<string> { ruleOnThisLayer });

            return bracketHierarchy;
        }

        private string CrossOverCompleteBracketHierarchy(Dictionary<int, List<string>> allHierarchicalRules)
        {
            string childRule = "%";
            for (int i = allHierarchicalRules.Count - 1; i >= 0; --i)
            {
                List<string> rulePossibilities = allHierarchicalRules[i];
                
                int wildcardIndex = childRule.IndexOf("%");
                while (wildcardIndex != -1)
                {
                    int randomRuleIndex = _randomGenerator.Next(0, rulePossibilities.Count);
                    string randomRule = rulePossibilities[randomRuleIndex];

                    if (wildcardIndex != childRule.Length - 1)
                        childRule = childRule.Substring(0, wildcardIndex) + randomRule + childRule.Substring(wildcardIndex + 1, childRule.Length - (wildcardIndex + 1));
                    else
                        childRule = childRule.Substring(0, wildcardIndex) + randomRule;

                    wildcardIndex = childRule.IndexOf("%", wildcardIndex + randomRule.Length);
                }
            }

            return childRule;
        }

        public RuleSet CrossOver(RuleSet leftParentRuleSet, RuleSet rightParentRuleSet)
        {
            Dictionary<string, List<LSystemRule>> rules = new Dictionary<string, List<LSystemRule>>();

            foreach (var leftParentRule in leftParentRuleSet.Rules)
            {
                if (rightParentRuleSet.Rules.ContainsKey(leftParentRule.Key) == false)
                {
                    rules.Add(leftParentRule.Key, new List<LSystemRule>(leftParentRule.Value));
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