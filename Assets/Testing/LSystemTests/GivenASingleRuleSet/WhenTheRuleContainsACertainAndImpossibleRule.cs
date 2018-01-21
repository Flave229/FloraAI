using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenASingleRuleSet
{
    class WhenTheRuleContainsACertainAndImpossibleRule
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
                    _axiom, new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 0,
                            Rule = "Impossible"
                        },
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
        public void ThenOnTheFirstIterationTheCertainRuleIsAlwaysChosen()
        {
            // Loops 5 times because this has randomness
            for(int i = 0; i < 5; ++i)
            { 
                var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
                subject.Iterate();
                Assert.That(subject.GetCommandString(), Is.EqualTo("BAB"));
            }
        }

        [Test]
        public void ThenOnTheSecondIterationTheCertainRuleIsAlwaysChosen()
        {
            // Loops 5 times because this has randomness
            for (int i = 0; i < 5; ++i)
            {
                var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
                subject.Iterate();
                subject.Iterate();
                Assert.That(subject.GetCommandString(), Is.EqualTo("BBABB"));
            }
        }
    }
}