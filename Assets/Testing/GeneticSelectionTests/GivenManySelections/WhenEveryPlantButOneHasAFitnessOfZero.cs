using System;
using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.GeneticSelectionTests.GivenManySelections
{
    class WhenEveryPlantButOneHasAFitnessOfZero
    {
        [Test]
        public void ThenTheFirstPlantIsGuaranteedToBeSelectedForAllPairings()
        {
            PlantSelection selection = new PlantSelection(new Random());

            RuleSet firstRuleSet = new RuleSet(new Dictionary<string, List<LSystemRule>>
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

            var firstLSystem = new LSystem(firstRuleSet, "F");
            Dictionary<LSystem, float> plantsAndFitness = new Dictionary<LSystem, float>
            {
                { firstLSystem, 1 },
                { new LSystem(firstRuleSet, "A"), 0 },
                { new LSystem(firstRuleSet, "B"), 0 },
                { new LSystem(firstRuleSet, "C"), 0 },
                { new LSystem(firstRuleSet, "D"), 0 },
                { new LSystem(firstRuleSet, "E"), 0 },
                { new LSystem(firstRuleSet, "F"), 0 },
                { new LSystem(firstRuleSet, "G"), 0 },
                { new LSystem(firstRuleSet, "H"), 0 },
                { new LSystem(firstRuleSet, "I"), 0 },
                { new LSystem(firstRuleSet, "J"), 0 },
                { new LSystem(firstRuleSet, "K"), 0 },
                { new LSystem(firstRuleSet, "L"), 0 },
                { new LSystem(firstRuleSet, "M"), 0 },
                { new LSystem(firstRuleSet, "N"), 0 },
                { new LSystem(firstRuleSet, "O"), 0 },
                { new LSystem(firstRuleSet, "P"), 0 },
                { new LSystem(firstRuleSet, "Q"), 0 },
                { new LSystem(firstRuleSet, "R"), 0 },
                { new LSystem(firstRuleSet, "S"), 0 },
                { new LSystem(firstRuleSet, "T"), 0 },
                { new LSystem(firstRuleSet, "U"), 0 },
                { new LSystem(firstRuleSet, "V"), 0 }
            };

            List<List<LSystem>> chosenParents = selection.SelectParentPairs(plantsAndFitness, 5);

            Assert.That(chosenParents[0], Contains.Item(firstLSystem));
            Assert.That(chosenParents[1], Contains.Item(firstLSystem));
            Assert.That(chosenParents[2], Contains.Item(firstLSystem));
            Assert.That(chosenParents[3], Contains.Item(firstLSystem));
            Assert.That(chosenParents[4], Contains.Item(firstLSystem));
        }
    }
}
