using System.Collections.Generic;
using Assets.Scripts.LSystems;
using NUnit.Framework;

namespace Assets.Testing.LSystemTests.GivenTwoRuleSets
{
    class WhenARuleModifiesTheAxiomWithARuleThatIsThenAlteredByTheSecondRule
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
                    "A", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "BAB"
                        }
                    }
                },
                {
                    "B", new List<LSystemRule>
                    {
                        new LSystemRule
                        {
                            Probability = 1,
                            Rule = "C"
                        }
                    }
                }
            };
        }

        [Test]
        public void ThenOnTheFirstIterationTheCommandStringEqualsTheRuleForTheAxiomCharacter()
        {
            var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo("BAB"));
        }

        [Test]
        public void ThenOnTheSecondIterationTheCommandStringEqualsTheRulesReplaceAllCharactersFromTheFirstIteration()
        {
            var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
            subject.Iterate();
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo("CBABC"));
        }

        [Test]
        public void ThenOnTheThirdIterationTheCommandStringEqualsTheRulesReplaceAllCharactersFromTheSecondIteration()
        {
            var subject = new LSystem(new RuleSet(_ruleSet), _axiom);
            subject.Iterate();
            subject.Iterate();
            subject.Iterate();
            Assert.That(subject.GetCommandString(), Is.EqualTo("CCBABCC"));
        }
    }
}