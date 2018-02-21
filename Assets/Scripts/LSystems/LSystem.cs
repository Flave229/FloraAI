namespace Assets.Scripts.LSystems
{
    public interface ILSystem
    {
        void Iterate();
        string GetCommandString();
    }

    public class LSystem : ILSystem
    {
        private string _currentString;
        private readonly RuleSet _rules;

        public LSystem(RuleSet rules, string axiom)
        {
            _currentString = axiom;
            _rules = rules;
        }

        public void Iterate()
        {
            var newString = "";

            for (int i = 0, length = _currentString.Length; i < length; ++i)
                newString += _rules.GetRule(_currentString[i].ToString());

            _currentString = newString;
        }

        public string GetCommandString()
        {
            return _currentString;
        }
    }
}