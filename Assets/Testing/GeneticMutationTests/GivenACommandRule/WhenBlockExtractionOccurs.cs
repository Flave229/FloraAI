using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticMutationTests.GivenACommandRule
{
    class WhenBlockExtractionOccurs
    {
        private static int _nextDoubleCalls;
        private static int _nextCalls;

        [Test]
        public void ThenOneExtraBracketAppearsInTheRule()
        {
            var actualRandom = new System.Random();
            var randomMock = new Mock<System.Random>();
            randomMock.Setup(x => x.NextDouble())
                .Returns(() =>
                {
                    ++_nextDoubleCalls;
                    return _nextDoubleCalls == 8 ? -1 : 0;
                });

            randomMock.Setup(x => x.Next(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int lowerBound, int upperBound) =>
                {
                    ++_nextCalls;
                    if (_nextCalls == 1)
                        return 1;
                    return actualRandom.Next(lowerBound, upperBound);
                });
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

            Debug.Log("After Mutation Rule: " + fRule);
            Assert.That(fRule, Is.EqualTo("+F"));
        }
    }
}
