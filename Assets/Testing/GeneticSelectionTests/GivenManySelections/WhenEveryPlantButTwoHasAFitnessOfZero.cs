using System;
using System.Collections.Generic;
using Assets.Scripts.Common;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.GeneticSelectionTests.GivenManySelections
{
    class WhenEveryPlantButTwoHasAFitnessOfZero
    {
        [Test]
        public void ThenTheFirstPlantAndSecondPlantAreGuaranteedToBeChosenAsParents()
        {
            PlantSelection selection = new PlantSelection(new Random());

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

            var firstLSystem = new LSystem(ruleSet, "A");
            var secondLSystem = new LSystem(ruleSet, "B");
            List<Tuple<ILSystem, float>> plantsAndFitness = new List<Tuple<ILSystem, float>>
            {
                new Tuple<ILSystem, float>(firstLSystem, 5),
                new Tuple<ILSystem, float>(secondLSystem, 5),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "B"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "C"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "D"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "E"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "F"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "G"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "H"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "I"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "J"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "K"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "L"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "M"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "N"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "O"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "P"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "Q"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "R"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "S"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "T"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "U"), 0),
                new Tuple<ILSystem, float>(new LSystem(ruleSet, "V"), 0)
            };

            List<List<ILSystem>> chosenParents = selection.SelectParentPairs(plantsAndFitness, 5);

            Assert.That(chosenParents[0], Contains.Item(firstLSystem));
            Assert.That(chosenParents[0], Contains.Item(secondLSystem));
            Assert.That(chosenParents[1], Contains.Item(firstLSystem));
            Assert.That(chosenParents[1], Contains.Item(secondLSystem));
            Assert.That(chosenParents[2], Contains.Item(firstLSystem));
            Assert.That(chosenParents[2], Contains.Item(secondLSystem));
            Assert.That(chosenParents[3], Contains.Item(firstLSystem));
            Assert.That(chosenParents[3], Contains.Item(secondLSystem));
            Assert.That(chosenParents[4], Contains.Item(firstLSystem));
            Assert.That(chosenParents[4], Contains.Item(secondLSystem));
        }
    }
}
