using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenASingleRuleSetOnOneIteration
{
    class WhenTheRuleContainsACertainAndImpossibleRule
    {
        [Test]
        public void ThenTheCertainRuleIsAlwaysChosen()
        {
            var axiom = "A";
            var ruleSet = new Dictionary<string, List<LSystemRule>>
            {
                {
                    axiom, new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 0,
                            Rule = "Impossible"
                        },
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "Certain"
                        }
                    }
                }
            };

            // Loops 5 times because this has randomness
            for(int i = 0; i < 5; ++i)
            { 
                var subject = new LSystem(new RuleSet(ruleSet), axiom);
                subject.Iterate();
                Assert.That(subject.GetCommandString(), Is.EqualTo("Certain"));
            }
        }
    }
}
