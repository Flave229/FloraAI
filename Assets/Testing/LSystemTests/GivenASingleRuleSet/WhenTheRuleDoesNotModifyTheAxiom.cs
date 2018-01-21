using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenASingleRuleSet
{
    class WhenASingleRuleSetDoesNotModifyTheAxiom
    {
        private string _axiom;
        private Dictionary<string, List<LSystemRule>> _ruleSet;

        [SetUp]
        public void SetUp()
        {
            _axiom = "A";
            _ruleSet = new Dictionary<string, List<LSystemRule>>
            {
                {
                    "B", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "BAB"
                        }
                    }
                }
            };
        }

        [Test]
        public void ThenOnTheFirstIterationTheCommandStringRemainsUnchanged()
        {
            var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo(_axiom));
        }

        [Test]
        public void ThenOnTheSecondIterationTheCommandStringRemainsUnchanged()
        {
            var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
            subject.Iterate();
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo(_axiom));
        }
    }
}