using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticCrossoverTests.GivenTwoParentsWithTwoRulesOnTheLowestLayer
{
    class WhenTheParentsContainTwoHierarchicalBracketLayersWithOneBranchHavingTwoLowerLevels
    {
        [Test]
        public void ThenAtLeastOneOfTheRulesFromTheChosenParentAreSwappedWithTheOtherParent()
        {
            PlantGenetics genetics = new PlantGenetics();
            RuleSet leftParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "+F[+F[+F+F][+F+F+F]][+F+F+F+F]"
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
                            Rule = "-F[-F[-F-F][-F-F-F]][-F-F-F-F]"
                        }
                    }
                }
            });

            RuleSet result = genetics.CrossOver(leftParentRuleSets, rightParentRuleSets);
            string fRule = result.Rules["F"][0].Rule;

            Debug.Log(fRule);
            Assert.That(fRule, Is.EqualTo("+F[+F[+F+F][+F+F+F]][-F-F]").Or.EqualTo("+F[+F[+F+F][+F+F+F]][-F-F-F]").Or.EqualTo("+F[+F[+F+F][+F+F+F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[+F+F][-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[+F+F][-F-F]][-F-F-F]").Or.EqualTo("+F[+F[+F+F][-F-F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[+F+F][-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[+F+F][-F-F-F]][-F-F]").Or.EqualTo("+F[+F[+F+F][-F-F-F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[+F+F][-F-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[+F+F][-F-F-F-F]][-F-F]").Or.EqualTo("+F[+F[+F+F][-F-F-F-F]][-F-F-F]")

                                    .Or.EqualTo("+F[+F[-F-F][+F+F+F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F][+F+F+F]][-F-F-F]").Or.EqualTo("+F[+F[-F-F][+F+F+F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F][-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F][-F-F-F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F][-F-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F][-F-F-F-F]][-F-F-F]")

                                    .Or.EqualTo("+F[+F[-F-F-F][+F+F+F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F][+F+F+F]][-F-F]").Or.EqualTo("+F[+F[-F-F-F][+F+F+F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F-F][-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F][-F-F]][-F-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F-F][-F-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F][-F-F-F-F]][-F-F]")

                                    .Or.EqualTo("+F[+F[-F-F-F-F][+F+F+F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F-F][+F+F+F]][-F-F]").Or.EqualTo("+F[+F[-F-F-F-F][+F+F+F]][-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F-F-F][-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F-F][-F-F]][-F-F-F]")
                                    .Or.EqualTo("+F[+F[-F-F-F-F][-F-F-F]][+F+F+F+F]").Or.EqualTo("+F[+F[-F-F-F-F][-F-F-F]][-F-F-F-F]")

                                    .Or.EqualTo("-F[-F[-F-F][-F-F-F]][+F+F]").Or.EqualTo("-F[-F[-F-F][-F-F-F]][+F+F+F]").Or.EqualTo("-F[-F[-F-F][-F-F-F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[-F-F][+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[-F-F][+F+F]][+F+F+F]").Or.EqualTo("-F[-F[-F-F][+F+F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[-F-F][+F+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[-F-F][+F+F+F]][+F+F]").Or.EqualTo("-F[-F[-F-F][+F+F+F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[-F-F][+F+F+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[-F-F][+F+F+F+F]][+F+F]").Or.EqualTo("-F[-F[-F-F][+F+F+F+F]][+F+F+F]")

                                    .Or.EqualTo("-F[-F[+F+F][-F-F-F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F][-F-F-F]][+F+F+F]").Or.EqualTo("-F[-F[+F+F][-F-F-F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F][+F+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F][+F+F+F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F][+F+F+F+F]][-F-F-F]").Or.EqualTo("-F[-F[+F+F][+F+F+F+F]][+F+F+F]")

                                    .Or.EqualTo("-F[-F[+F+F+F][-F-F-F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F][-F-F-F]][+F+F]").Or.EqualTo("-F[-F[+F+F+F][-F-F-F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F+F][+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F][+F+F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F+F][+F+F+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F][+F+F+F+F]][+F+F]")

                                    .Or.EqualTo("-F[-F[+F+F+F+F][-F-F-F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F+F][-F-F-F]][+F+F]").Or.EqualTo("-F[-F[+F+F+F+F][-F-F-F]][+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F+F+F][+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F+F][+F+F]][+F+F+F+F]")
                                    .Or.EqualTo("-F[-F[+F+F+F+F][+F+F+F]][-F-F-F-F]").Or.EqualTo("-F[-F[+F+F+F+F][+F+F+F]][+F+F]"));

        }
    }
}
