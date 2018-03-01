using System;
using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticCrossoverTests.GivenTwoDifferentRuleSets
{
    class WhereOnlyTheRightHandParentContainsARuleThatTheLeftDoesnt
    {
        [Test]
        public void ThenTheChildContainsTheMissingRule()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new System.Random());
            RuleSet leftParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "+F[+F+F]"
                        }
                    }
                }
            });

            RuleSet rightParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "-F[-F-F]"
                        }
                    }
                },
                { "A", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "-A[-A-A]"
                        }
                    }
                },
            });

            RuleSet result = crossOver.CrossOverV2(leftParentRuleSets, rightParentRuleSets);
            Assert.That(result.Rules["A"], Is.Not.Null);
        }
    }
}
