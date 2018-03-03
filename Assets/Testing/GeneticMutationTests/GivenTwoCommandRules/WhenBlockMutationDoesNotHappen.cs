using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticMutationTests.GivenTwoCommandRules
{
    class WhenBlockMutationDoesNotHappen
    {
        [Test]
        public void ThenTheRuleDoesNotChange()
        {
            var randomMock = new Mock<System.Random>();
            randomMock.Setup(x => x.NextDouble())
                .Returns(0);

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

            RuleSet mutatedRuleSet = mutation.Mutate(ruleSet);
            string fRule = mutatedRuleSet.Rules["F"][0].Rule;
            string aRule = mutatedRuleSet.Rules["A"][0].Rule;

            Debug.Log("After Mutation F Rule: " + fRule);
            Debug.Log("After Mutation A Rule: " + aRule);
            Assert.That(fRule, Is.EqualTo("+F[+F+F]"));
            Assert.That(aRule, Is.EqualTo("+A[+A+A]"));
        }
    }
}