using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticCrossoverTests.GivenTwoDifferentRuleSets
{
    class WhenOneRuleHasABracketAroundTheEntireRuleAndTheOtherHasATwoLayerBracketHierarchy
    {
        [Test]
        public void ThenTheEntireCrossOverRuleIsWrappedInBrackets()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new System.Random());
            RuleSet leftParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "L", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "['''^^O]"
                        }
                    }
                }
            });

            RuleSet rightParentRuleSets = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
                { "L", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "[[-FA\\L^S]'''^^O]"
                        }
                    }
                }
            });

            RuleSet result = crossOver.CrossOverV2(leftParentRuleSets, rightParentRuleSets);
            string lRule = result.Rules["L"][0].Rule;

            Debug.Log(lRule);
            Assert.That(lRule[0], Is.EqualTo('['));
            Assert.That(lRule[lRule.Length - 1], Is.EqualTo(']'));
        }
    }
}
