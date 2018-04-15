using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using UnityEngine;

namespace Assets.Testing.GeneticMutationTests.GivenACommandRule
{
    class WhenSymbolMutationIsGuaranteedToHappenOnTheFirstMutableStringCombination
    {
        private static int _calls;

        [Test]
        public void ThenTheFirstSymbolIsMutated()
        {
            var actualRandom = new System.Random();
            var randomMock = new Mock<System.Random>();
            randomMock.Setup(x => x.NextDouble())
                .Returns(() =>
                {
                    ++_calls;
                    return _calls == 1 ? -1 : 0;
                }); // Forces an always successful mutation on first run

            randomMock.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int lowerBound, int upperBound) => actualRandom.Next(lowerBound, upperBound));

            PlantMutation mutation = new PlantMutation(randomMock.Object, 0);
            
            RuleSet ruleSet = new RuleSet(new Dictionary<string, List<LSystemRule>>
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

            Debug.Log("Original Rule: " + ruleSet.Rules["F"][0].Rule);

            Color color = Color.black;
            RuleSet mutatedRuleSet = mutation.Mutate(ruleSet, ref color);
            string fRule = mutatedRuleSet.Rules["F"][0].Rule;

            Debug.Log("Entire Rule: " + fRule);
            Assert.That(fRule.Substring(0, 2), Is.Not.EqualTo("+F"));
            Assert.That(fRule.Substring(fRule.Length - 6, 6), Is.EqualTo("[+F+F]"));
        }
    }
}