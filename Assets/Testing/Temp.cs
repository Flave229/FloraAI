using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing
{
    class Temp
    {
        [Test]
        public void ThenTemp()
        {
            float logarithmicHeightModifier = Mathf.Max(Mathf.Min(Mathf.Log(1 + 1, 21), 1), 0);
            Debug.Log("Log base 20 at value 1: " + logarithmicHeightModifier);

            logarithmicHeightModifier = Mathf.Max(Mathf.Min(Mathf.Log(5 + 1, 21), 1), 0);
            Debug.Log("Log base 20 at value 5: " + logarithmicHeightModifier);

            logarithmicHeightModifier = Mathf.Max(Mathf.Min(Mathf.Log(10 + 1, 21), 1), 0);
            Debug.Log("Log base 20 at value 10: " + logarithmicHeightModifier);

            logarithmicHeightModifier = Mathf.Max(Mathf.Min(Mathf.Log(15 + 1, 21), 1), 0);
            Debug.Log("Log base 20 at value 15: " + logarithmicHeightModifier);

            logarithmicHeightModifier = Mathf.Max(Mathf.Min(Mathf.Log(20 + 1, 21), 1), 0);
            Debug.Log("Log base 20 at value 20: " + logarithmicHeightModifier);
        }
    }
}
