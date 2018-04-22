using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Genetic_Algorithm
{
    public interface ILeafFitness
    {
        float EvaluatePhotosyntheticRate(Leaf leaf);
        SunInformation GetSunInformation();
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
            Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);
            float summerDot = Mathf.Max(Vector3.Dot(normalisedLeafNormal, summerToSun), 0);
            float winterDot = Mathf.Max(Vector3.Dot(normalisedLeafNormal, winterToSun), 0);

            if (leaf.Position.y < 0)
                return 0; // I do not want leaves going into the ground to perform well at all

            //return (((summerDot + winterDot)) + (Mathf.Log(leaf.Position.y, 11))) / 3;
            return (((summerDot + winterDot)) + Mathf.Pow(Mathf.Min(leaf.Position.y / 3, 2), 2)) / 6;
            //return (((summerDot + winterDot)) + Mathf.Min(leaf.Position.y / 3, 2)) / 4;
            //return ((summerDot + winterDot)) + (Mathf.Pow((leaf.Position.y - 1) / 5, (float) 1 / 3) + 0.2f);
        }

        public SunInformation GetSunInformation()
        {
            return _sunInformation;
        }
    }
}