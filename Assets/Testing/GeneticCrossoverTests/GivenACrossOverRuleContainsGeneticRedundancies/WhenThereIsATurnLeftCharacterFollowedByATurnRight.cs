using System;
using Assets.Scripts.Genetic_Algorithm;
using NUnit.Framework;

namespace Assets.Testing.GeneticCrossoverTests.GivenACrossOverRuleContainsGeneticRedundancies
{
    class WhenThereIsATurnLeftCharacterFollowedByATurnRight
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("+-FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }

    class WhenThereIsATurnRightCharacterFollowedByATurnLeft
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("-+FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }

    class WhenThereIsAPitchUpCharacterFollowedByAPitchDown
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("&^FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }

    class WhenThereIsAPitchDownCharacterFollowedByAPitchUp
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("^&FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }

    class WhenThereIsARollLeftCharacterFollowedByARollRight
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("/\\FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }

    class WhenThereIsARollRightCharacterFollowedByARollLeft
    {
        [Test]
        public void ThenTheBracketIsRemovedFromTheRuleSet()
        {
            PlantCrossOver crossOver = new PlantCrossOver(new Random());
            string fixedRule = crossOver.RemoveGeneticRedundancies("\\/FF");

            Assert.That(fixedRule, Is.EqualTo("FF"));
        }
    }
}
