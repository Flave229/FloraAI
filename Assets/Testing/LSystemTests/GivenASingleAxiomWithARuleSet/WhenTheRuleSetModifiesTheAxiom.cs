using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenASingleAxiomWithARuleSet
{
    public class WhenTheRuleSetModifiesTheAxiom
    {
        [Test]
        public void ThenTheCommandStringIsEqualToTheRuleSet()
        {
            var axiom = "A";
            var ruleSet = new Dictionary<string, List<LSystemRule>>
            {
                {
                    axiom, new List<LSystemRule>
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
            Assert.That(subject.GetCommandString(), Is.EqualTo("TestString"));
        }
    }
}