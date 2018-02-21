using System;
using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.GeneticSelectionTests.GivenOneSelection
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
            Dictionary<LSystem, float> plantsAndFitness = new Dictionary<LSystem, float>
            {
                { firstLSystem, 5 },
                { secondLSystem, 5 },
                { new LSystem(ruleSet, "B"), 0 },
                { new LSystem(ruleSet, "C"), 0 },
                { new LSystem(ruleSet, "D"), 0 },
                { new LSystem(ruleSet, "E"), 0 },
                { new LSystem(ruleSet, "F"), 0 },
                { new LSystem(ruleSet, "G"), 0 },
                { new LSystem(ruleSet, "H"), 0 },
                { new LSystem(ruleSet, "I"), 0 },
                { new LSystem(ruleSet, "J"), 0 },
                { new LSystem(ruleSet, "K"), 0 },
                { new LSystem(ruleSet, "L"), 0 },
                { new LSystem(ruleSet, "M"), 0 },
                { new LSystem(ruleSet, "N"), 0 },
                { new LSystem(ruleSet, "O"), 0 },
                { new LSystem(ruleSet, "P"), 0 },
                { new LSystem(ruleSet, "Q"), 0 },
                { new LSystem(ruleSet, "R"), 0 },
                { new LSystem(ruleSet, "S"), 0 },
                { new LSystem(ruleSet, "T"), 0 },
                { new LSystem(ruleSet, "U"), 0 },
                { new LSystem(ruleSet, "V"), 0 }
            };

            List<LSystem> chosenParents = selection.ChooseParents(plantsAndFitness);

            Assert.That(chosenParents, Contains.Item(firstLSystem));
            Assert.That(chosenParents, Contains.Item(secondLSystem));
        }
    }
}
