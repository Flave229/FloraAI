﻿using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticMutationTests.GivenACommandRule
{
    class WhenBlockInjectionAndExtractionDoesNotHappen
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
                }
            });

            Debug.Log("Original Rule: " + ruleSet.Rules["F"][0].Rule);

            Color color = Color.black;
            RuleSet mutatedRuleSet = mutation.Mutate(ruleSet, ref color);
            string fRule = mutatedRuleSet.Rules["F"][0].Rule;

            Debug.Log("After Mutation Rule: " + fRule);
            Assert.That(fRule, Is.EqualTo("+F[+F+F]"));
        }
    }
}
