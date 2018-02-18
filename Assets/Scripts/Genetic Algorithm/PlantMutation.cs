using System;
using System.Collections.Generic;
using Assets.Scripts.LSystems;

namespace Assets.Scripts.Genetic_Algorithm
{
    public class PlantMutation
    {
        private readonly Random _randomGenerator;
        private readonly float _mutationChance;
        private readonly List<string> _mutableList;

        public PlantMutation(Random randomGenerator, float mutationChance)
        {
            _randomGenerator = randomGenerator;
            _mutationChance = mutationChance;
            _mutableList = new List<string> { "F", "+F", "-F", "&F", "^F", "\\F", "/F" };
        }

        public RuleSet Mutate(RuleSet ruleSet)
        {
            foreach (var rule in ruleSet.Rules)
            {
                foreach (var lSystemRule in rule.Value)
                {
                    lSystemRule.Rule = BlockMutation(lSystemRule.Rule);
                    lSystemRule.Rule = SymbolMutation(lSystemRule.Rule);
                }
            }
            return ruleSet;
        }

        private string BlockMutation(string ruleString)
        {
            int characterIndex = 0;
            string newRuleString = ruleString.Substring(0, ruleString.IndexOf('['));
            while (characterIndex != -1)
            {
                int oldIndex = characterIndex;
                characterIndex = ruleString.IndexOf('[', characterIndex);
                if (characterIndex == -1)
                {
                    characterIndex = oldIndex;
                    break;
                }

                int endBracket = ruleString.IndexOf(']', characterIndex);
                int nextStartBracket = ruleString.IndexOf('[', characterIndex + 1);
                while (nextStartBracket < endBracket && nextStartBracket != -1)
                {
                    newRuleString += ruleString.Substring(characterIndex, nextStartBracket - characterIndex);
                    characterIndex = nextStartBracket;
                    endBracket = ruleString.IndexOf(']', characterIndex);
                    nextStartBracket = ruleString.IndexOf('[', characterIndex + 1);
                }

                double randomChance = _randomGenerator.NextDouble();

                var currentBlock = ruleString.Substring(characterIndex, endBracket - characterIndex + 1);
                if (randomChance >= _mutationChance)
                {
                    newRuleString += currentBlock;
                    characterIndex = endBracket + 1;
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
                characterIndex = endBracket + 1;
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
                int randomCharacter = _randomGenerator.Next(0, _mutableList.Count);
                newBlock += _mutableList[randomCharacter];
            }

            newBlock += "]";
            return newBlock;
        }

        private string SymbolMutation(string rule)
        {
            string newRule = "";
            for (int i = 0; i < rule.Length; ++i)
            {
                char character = rule[i];

                switch (character)
                {
                    case '+':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("+F");
                        continue;
                    case '-':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("-F");
                        break;
                    case '&':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("&F");
                        break;
                    case '^':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("^F");
                        break;
                    case '\\':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("\\F");
                        break;
                    case '/':
                        if (rule[i + 1] != 'F')
                        {
                            newRule += character;
                            continue;
                        }

                        ++i;
                        newRule += MutateCharacter("/F");
                        break;
                    case 'F':
                        ++i;
                        newRule += MutateCharacter("F");
                        break;
                    default:
                        newRule += rule[i];
                        break;
                }
            }
            return newRule;
        }


        private string MutateCharacter(string character)
        {
            double randomChance = _randomGenerator.NextDouble();
            if (randomChance >= _mutationChance)
                return character;

            string newMutatedCharacter = character;
            while (newMutatedCharacter == character)
            {
                int randomMutableIndex = _randomGenerator.Next(0, _mutableList.Count);
                newMutatedCharacter = _mutableList[randomMutableIndex];
            }
            return newMutatedCharacter;
        }
    }
}