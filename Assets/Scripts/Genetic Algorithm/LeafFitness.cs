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
            Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);
            float summerDot = Vector3.Dot(normalisedLeafNormal, summerToSun);
            float winterDot = Vector3.Dot(normalisedLeafNormal, winterToSun);
            return Mathf.Max((summerDot + winterDot) / 2, 0);

            //// Area of tolerance when the height is (above 10?) 
            //Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            //Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            //Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);

            //float angleTolerance = Mathf.Max(leaf.Position.y / 20, 1);
            //float summerDot = Vector3.Dot(normalisedLeafNormal, summerToSun);
            //float summerAngle = Mathf.Acos(summerDot / (Vector3.Magnitude(normalisedLeafNormal) * Vector3.Magnitude(summerToSun)));
            //if (summerAngle < angleTolerance)
            //    summerDot = 1;

            //float winterDot = Vector3.Dot(normalisedLeafNormal, winterToSun);
            //float winterAngle = Mathf.Acos(winterDot / (Vector3.Magnitude(normalisedLeafNormal) * Vector3.Magnitude(winterToSun)));
            //if (winterAngle < angleTolerance)
            //    winterDot = 1;

            //return Mathf.Max((summerDot + winterDot) / 2, 0);




            //// Area of tolerance when the height is (above 10?) 
            //Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            //Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            //Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);
            //float summerDot = Vector3.Dot(normalisedLeafNormal, summerToSun);
            //float winterDot = Vector3.Dot(normalisedLeafNormal, winterToSun);

            //float heightBonus = Mathf.Min((leaf.Position.y / 20), 5);


            //return Mathf.Max(Mathf.Min(((summerDot + winterDot + heightBonus) / 7), 1), 0);
        }
    }
}
