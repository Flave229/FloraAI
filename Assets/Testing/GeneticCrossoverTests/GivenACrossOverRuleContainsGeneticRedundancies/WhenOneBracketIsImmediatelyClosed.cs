using System;
using Assets.Scripts.Genetic_Algorithm;
using NUnit.Framework;

namespace Assets.Testing.GeneticCrossoverTests.GivenACrossOverRuleContainsGeneticRedundancies
{
    class WhenOneBracketIsImmediatelyClosed
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("[]FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }
}
