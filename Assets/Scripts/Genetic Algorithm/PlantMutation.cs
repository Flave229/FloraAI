using System;
using System.Collections.Generic;
using Assets.Scripts.LSystems;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantMutation
    {
        private readonly Random _randomGenerator;
        private readonly float _mutationChance;
        private List<string> _mutableCharacters;
        private List<string> _mutableSymbols;

        public PlantMutation(Random randomGenerator, float mutationChance)
        {
            _randomGenerator = randomGenerator;
            _mutationChance = mutationChance;
            _mutableCharacters = new List<string> {"F", "L", "S", "A" };
            _mutableSymbols = new List<string> { "", "+", "-", "&", "^", "\\", "/" , "!"};
        }


        public RuleSet Mutate(RuleSet ruleSet, ref Color leafColour)
        {
            foreach (var rule in ruleSet.Rules)
            {
                foreach (var lSystemRule in rule.Value)
                {
                    lSystemRule.Rule = SymbolMutation(lSystemRule.Rule);
                    lSystemRule.Rule = BlockMutation(lSystemRule.Rule);
                    lSystemRule.Rule = BlockInjectionAndExtraction(lSystemRule.Rule);
                }

                if (rule.Key == "L")
                {
                    rule.Value[0].Rule = rule.Value[0].Rule.Replace('F', 'O').Replace('L', 'O').Replace('S', 'O')
                        .Replace('A', 'O');
                }
                else if (rule.Value[0].Rule.Contains("O"))
                {
                    int randomCharacter = _randomGenerator.Next(0, _mutableCharacters.Count);
                    rule.Value[0].Rule = rule.Value[0].Rule.Replace("O", _mutableCharacters[randomCharacter]);
                }
            }

            leafColour = MutateLeafColour(leafColour);
            return ruleSet;
        }

        private Color MutateLeafColour(Color leafColour)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance)
                return leafColour;

            int channelToMutate = _randomGenerator.Next(0, 6);
            float colourChange = (float)_randomGenerator.NextDouble() / 3;
            if (channelToMutate == 0)
                leafColour.r += colourChange;
            else if (channelToMutate == 1)
                leafColour.r -= colourChange;
            else if (channelToMutate == 2)
                leafColour.g += colourChange;
            else if (channelToMutate == 3)
                leafColour.g -= colourChange;
            else if (channelToMutate == 4)
                leafColour.b += colourChange;
            else if (channelToMutate == 5)
                leafColour.b -= colourChange;

            if (leafColour.r > 1)
                leafColour.r = 1.0f;
            else if (leafColour.r < 0)
                leafColour.r = 0;
            if (leafColour.g > 1)
                leafColour.g = 1.0f;
            else if (leafColour.g < 0)
                leafColour.g = 0;
            if (leafColour.b > 1)
                leafColour.b = 1.0f;
            else if (leafColour.b < 0)
                leafColour.b = 0;

            return leafColour;
        }

        private string BlockMutation(string ruleString)
        {
            int characterIndex = 0;
            int endBracketIndex = -1;

            if (ruleString.IndexOf('[') == -1)
                return ruleString;
            string newRuleString = "";
            while (characterIndex != -1)
            {
                int oldIndex = characterIndex;
                characterIndex = ruleString.IndexOf('[', characterIndex);
                if (characterIndex == -1)
                {
                    characterIndex = oldIndex;
                    break;
                }
                newRuleString += ruleString.Substring(endBracketIndex + 1, characterIndex - (endBracketIndex + 1));

                endBracketIndex = ruleString.IndexOf(']', characterIndex);
                int nextStartBracket = ruleString.IndexOf('[', characterIndex + 1);
                while (nextStartBracket < endBracketIndex && nextStartBracket != -1)
                {
                    newRuleString += ruleString.Substring(characterIndex, nextStartBracket - characterIndex);
                    characterIndex = nextStartBracket;
                    endBracketIndex = ruleString.IndexOf(']', characterIndex);
                    nextStartBracket = ruleString.IndexOf('[', characterIndex + 1);
                }

                double randomChance = _randomGenerator.NextDouble();

                var currentBlock = ruleString.Substring(characterIndex, endBracketIndex - characterIndex + 1);
                if (randomChance >= (_mutationChance / 4)) // Lowering chance of block mutation
                {
                    newRuleString += currentBlock;
                    characterIndex = endBracketIndex + 1;
                    if (characterIndex >= ruleString.Length)
                        break;
                    continue;
                }

                string newBlock = currentBlock;
                while (newBlock == currentBlock)
                {
                    newBlock = BuildNewGeneticBlock();
                }
                
                newRuleString += newBlock;
                characterIndex = endBracketIndex + 1;
                if (characterIndex >= ruleString.Length)
                    break;
            }
            newRuleString += ruleString.Substring(characterIndex);

            return newRuleString;
        }

        private string BuildNewGeneticBlock()
        {
            int randomAmount = _randomGenerator.Next(1, 5);
            string newBlock = "[";

            for (int i = 0; i < randomAmount; ++i)
            {
                int randomCharacter = _randomGenerator.Next(0, _mutableCharacters.Count);
                int randomSymbol = _randomGenerator.Next(0, _mutableSymbols.Count);
                newBlock += _mutableSymbols[randomSymbol] + _mutableCharacters[randomCharacter];
            }

            newBlock += "]";
            return newBlock;
        }

        private string SymbolMutation(string rule)
        {
            string newRule = "";
            try
            {
                if (rule.Length > 20)
                {
                    // Remove Symbol
                    int randomIndex = _randomGenerator.Next(0, rule.Length);
                    while (rule[randomIndex] == '[' || rule[randomIndex] == ']')
                    {
                        ++randomIndex;
                        if (randomIndex >= rule.Length)
                            randomIndex = 0;
                    }

                    rule.Remove(randomIndex);
                    return rule;
                }

                for (int i = 0; i < rule.Length; ++i)
                {
                    char character = rule[i];

                    switch (character)
                    {
                        case '+':
                        case '-':
                        case '&':
                        case '^':
                        case '\\':
                        case '/':
                        case '!':
                            if (i + 1 < rule.Length && rule[i + 1] != 'F')
                            {
                                newRule += MutateSymbol(character.ToString(), false);
                                continue;
                            }

                            ++i;
                            newRule += MutateSymbol(character.ToString(), true);
                            newRule += MutateCharacter("F");
                            continue;
                        case 'F':
                        case 'A':
                        case 'S':
                            newRule += MutateCharacter(character.ToString());
                            break;
                        case 'L':
                            newRule += MutateLeafCharacter(character.ToString());
                            break;
                        default:
                            newRule += rule[i];
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Exception Occured In Plant Sumbol Mutation: " + e);
                Debug.Log("Old rule: " + rule);
                Debug.Log("New rule: " + newRule);
                throw;
            }
            return newRule;
        }

        private string MutateSymbol(string symbol, bool includeEmptyCharacter)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance)
                return symbol;

            string newMutatedCharacter = symbol;
            while (newMutatedCharacter == symbol && (includeEmptyCharacter || newMutatedCharacter != ""))
            {
                int randomSymbol = _randomGenerator.Next(0, _mutableSymbols.Count);
                newMutatedCharacter = _mutableSymbols[randomSymbol];
            }
            return newMutatedCharacter;
        }

        private string MutateCharacter(string character)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance)
                return character;
            
            int randomCharacter = _randomGenerator.Next(0, _mutableCharacters.Count);
            return _mutableCharacters[randomCharacter];
        }

        private string MutateLeafCharacter(string character)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance)
                return character;

            List<string> mutableCharacters = new List<string>
            {
                "O",
                ""
            };
            int randomCharacter = _randomGenerator.Next(0, mutableCharacters.Count);
            return mutableCharacters[randomCharacter];
        }

        private string BlockInjectionAndExtraction(string rule)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance / 4)
                return rule;

            int randomNumber = _randomGenerator.Next(0, 2);
            if (rule.Length > 20)
                randomNumber = 1;

            if (randomNumber == 0) // injection
            {
                int randomRuleIndex = _randomGenerator.Next(0, rule.Length);
                rule = rule.Insert(randomRuleIndex, BuildNewGeneticBlock());
            } 
            else // extraction
            {
                List<KeyValuePair<int, int>> startAndEndIndexesOfBrackets = new List<KeyValuePair<int, int>>();

                int characterIndex = 0;
                int endBracketIndex = -1;

                if (rule.IndexOf('[') == -1)
                    return rule;
                while (characterIndex != -1)
                {
                    int oldIndex = characterIndex;
                    characterIndex = rule.IndexOf('[', characterIndex);
                    if (characterIndex == -1)
                    {
                        characterIndex = oldIndex;
                        break;
                    }
                    endBracketIndex = rule.IndexOf(']', characterIndex);
                    int nextStartBracket = rule.IndexOf('[', characterIndex + 1);
                    while (nextStartBracket < endBracketIndex && nextStartBracket != -1)
                    {
                        characterIndex = nextStartBracket;
                        endBracketIndex = rule.IndexOf(']', characterIndex);
                        nextStartBracket = rule.IndexOf('[', characterIndex + 1);
                    }

                    startAndEndIndexesOfBrackets.Add(new KeyValuePair<int, int>(characterIndex, endBracketIndex));

                    characterIndex = endBracketIndex + 1;
                    if (characterIndex >= rule.Length)
                        break;
                }

                int randomBlockToExtract = _randomGenerator.Next(0, startAndEndIndexesOfBrackets.Count);
                KeyValuePair<int, int> indexesToExtract = startAndEndIndexesOfBrackets[randomBlockToExtract];
                rule = rule.Remove(indexesToExtract.Key, indexesToExtract.Value - indexesToExtract.Key + 1);
            }

            return rule;
        }
    }
}