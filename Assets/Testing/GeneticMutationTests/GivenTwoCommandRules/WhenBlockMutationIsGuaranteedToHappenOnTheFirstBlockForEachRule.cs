using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticMutationTests.GivenTwoCommandRules
{
    class WhenBlockMutationIsGuaranteedToHappenOnTheFirstBlockForEachRule
    {
        private static int _calls;

        [Test]
        public void ThenTheFirstBlockIsMutated()
        {
            var actualRandom = new System.Random();
            var randomMock = new Mock<System.Random>();
            randomMock.Setup(x => x.NextDouble())
                .Returns(() =>
                {
                    ++_calls;
                    return _calls == 7 || _calls == 15 ? -1 : 0;
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
                },
                { "A", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "+A[+A+A]"
                        }
                    }
                }
            });

            Debug.Log("Original F Rule: " + ruleSet.Rules["F"][0].Rule);
            Debug.Log("Original A Rule: " + ruleSet.Rules["A"][0].Rule);

            Color color = Color.black;
            RuleSet mutatedRuleSet = mutation.Mutate(ruleSet, ref color);
            string fRule = mutatedRuleSet.Rules["F"][0].Rule;
            string aRule = mutatedRuleSet.Rules["A"][0].Rule;

            Debug.Log("Entire F Rule: " + fRule);
            Debug.Log("Entire A Rule: " + aRule);
            Debug.Log("Mutated F Block: " + fRule.Substring(2, fRule.Length - 2));
            Debug.Log("Mutated A Block: " + aRule.Substring(2, aRule.Length - 2));
            Assert.That(fRule.Substring(0, 2), Is.EqualTo("+F"));
            Assert.That(fRule.Substring(2, fRule.Length - 2).Length, Is.AtLeast(3));
            Assert.That(fRule.Substring(2, fRule.Length - 2), Is.Not.EqualTo("[+F+F]"));

            Assert.That(aRule.Substring(0, 2), Is.EqualTo("+A"));
            Assert.That(aRule.Substring(2, aRule.Length - 2).Length, Is.AtLeast(3));
            Assert.That(aRule.Substring(2, aRule.Length - 2), Is.Not.EqualTo("[+A+A]"));
        }
    }
}