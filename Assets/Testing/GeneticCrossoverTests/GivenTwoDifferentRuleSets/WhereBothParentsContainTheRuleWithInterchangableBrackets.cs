using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticCrossoverTests.GivenTwoDifferentRuleSets
{
    class WhereBothParentsContainTheRuleWithInterchangableBrackets
    {
        [Test]
        public void ThenTheChildContainsAMixtureOfTheGeneticCode()
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
                },
                { "A", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "+A[+A+A]"
                        }
                    }
                },
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
                }
            });

            RuleSet result = crossOver.CrossOverV2(leftParentRuleSets, rightParentRuleSets);
            string fRule = result.Rules["F"][0].Rule;
            string aRule = result.Rules["A"][0].Rule;

            Debug.Log(fRule);
            Debug.Log(aRule);
            Assert.That(fRule, Is.EqualTo("+F[+F+F]").Or.EqualTo("+F[-F-F]").Or.EqualTo("-F[+F+F]").Or.EqualTo("-F[-F-F]"));
            Assert.That(aRule, Is.EqualTo("+A[+A+A]").Or.EqualTo("+A[-A-A]").Or.EqualTo("-A[+A+A]").Or.EqualTo("-A[-A-A]"));
        }
    }
}
