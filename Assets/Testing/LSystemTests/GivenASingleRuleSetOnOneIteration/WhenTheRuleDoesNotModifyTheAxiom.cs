using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenASingleRuleSet
{
    class WhenASingleRuleSetDoesNotModifyTheAxiom
    {
        [Test]
        public void ThenTheCommandStringRemainsUnchanged()
        {
            var axiom = "A";
            var ruleSet = new Dictionary<string, List<LSystemRule>>
            {
                {
                    "B", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "TestString"
                        }
                    }
                }
            };

            var subject = new LSystem(new RuleSet(ruleSet), axiom);
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo("A"));
        }
    }
}