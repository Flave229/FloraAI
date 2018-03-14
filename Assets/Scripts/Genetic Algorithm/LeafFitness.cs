using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    public interface ILeafFitness
    {
        float EvaluatePhotosyntheticRate(Leaf leaf);
    }

    public class LeafFitness : ILeafFitness
    {
        private readonly SunInformation _sunInformation;

        public LeafFitness(SunInformation sunInformation)
        {
            _sunInformation = sunInformation;
        }

        public float EvaluatePhotosyntheticRate(Leaf leaf)
        {
            Vector3 summerToSun = Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0);
            Vector3 winterToSun = Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0);

            return Mathf.Max((Vector3.Dot(Vector3.Normalize(leaf.Normal), summerToSun) + Vector3.Dot(Vector3.Normalize(leaf.Normal), winterToSun)) / 2, 0);
        }
    }
}
