using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.GeneticCrossoverTests.GivenTwoParentsWithOneRuleOnTheLowestLayer
{
    class WhenTheParentsContainNoBrackets
    {
        [Test]
        public void ThenTheChildContainsEitherTheParentsDna()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new System.Random());
            RuleSet leftParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "F", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "+F+F"
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
                            Rule = "-F-F"
                        }
                    }
                }
            });

            RuleSet result = crossOver.CrossOver(leftParentRuleSets, rightParentRuleSets);
            string fRule = result.Rules["F"][0].Rule;

            Assert.That(fRule, Is.EqualTo(leftParentRuleSets.Rules["F"][0].Rule).Or.EqualTo(rightParentRuleSets.Rules["F"][0].Rule));
        }
    }
}
