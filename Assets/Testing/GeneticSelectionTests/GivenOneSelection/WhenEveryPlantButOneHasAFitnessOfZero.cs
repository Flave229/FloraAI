using System;
using System.Collections.Generic;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.GeneticSelectionTests.GivenOneSelection
{
    class WhenEveryPlantButOneHasAFitnessOfZero
    {
        [Test]
        public void ThenTheFirstPlantIsGuaranteedToBeSelectedAsOneOfTheParents()
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

            RuleSet secondRuleSet = new RuleSet(new Dictionary<string, List<LSystemRule>>
            {
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

            var firstLSystem = new LSystem(firstRuleSet, "F");
            Dictionary<ILSystem, float> plantsAndFitness = new Dictionary<ILSystem, float>
            {
                { firstLSystem, 1 },
                { new LSystem(secondRuleSet, "A"), 0 },
                { new LSystem(secondRuleSet, "B"), 0 },
                { new LSystem(secondRuleSet, "C"), 0 },
                { new LSystem(secondRuleSet, "D"), 0 },
                { new LSystem(secondRuleSet, "E"), 0 },
                { new LSystem(secondRuleSet, "F"), 0 },
                { new LSystem(secondRuleSet, "G"), 0 },
                { new LSystem(secondRuleSet, "H"), 0 },
                { new LSystem(secondRuleSet, "I"), 0 },
                { new LSystem(secondRuleSet, "J"), 0 },
                { new LSystem(secondRuleSet, "K"), 0 },
                { new LSystem(secondRuleSet, "L"), 0 },
                { new LSystem(secondRuleSet, "M"), 0 },
                { new LSystem(secondRuleSet, "N"), 0 },
                { new LSystem(secondRuleSet, "O"), 0 },
                { new LSystem(secondRuleSet, "P"), 0 },
                { new LSystem(secondRuleSet, "Q"), 0 },
                { new LSystem(secondRuleSet, "R"), 0 },
                { new LSystem(secondRuleSet, "S"), 0 },
                { new LSystem(secondRuleSet, "T"), 0 },
                { new LSystem(secondRuleSet, "U"), 0 },
                { new LSystem(secondRuleSet, "V"), 0 }
            };

            List<ILSystem> chosenParents = selection.ChooseParents(plantsAndFitness);

            Assert.That(chosenParents, Contains.Item(firstLSystem));
        }
    }
}
