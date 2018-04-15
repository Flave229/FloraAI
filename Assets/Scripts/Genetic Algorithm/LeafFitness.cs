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
            //Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            //Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            //Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);
            //float summerDot = Vector3.Dot(normalisedLeafNormal, summerToSun);
            //float winterDot = Vector3.Dot(normalisedLeafNormal, winterToSun);

            //return Mathf.Max((summerDot + winterDot) / 2, 0);



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


            // Area of tolerance when the height is (above 10?) 
            Vector3 summerToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.SummerAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));
            Vector3 winterToSun = Vector3.Normalize(Quaternion.Euler((float)_sunInformation.WinterAltitude, (float)_sunInformation.Azimuth, 0) * new Vector3(0, 1, 0));

            Vector3 normalisedLeafNormal = Vector3.Normalize(leaf.Normal);
            float summerDot = Mathf.Max(Vector3.Dot(normalisedLeafNormal, summerToSun), 0);
            float winterDot = Mathf.Max(Vector3.Dot(normalisedLeafNormal, winterToSun), 0);
            //float logarithmicHeightModifier = Mathf.Min(Mathf.Log(leaf.Position.y + 1, 21), 1);

            //return Mathf.Min((((summerDot + winterDot) * logarithmicHeightModifier) / 2), 1);
            //return ((summerDot + winterDot) / 2) * Mathf.Pow(leaf.Position.y + 1, 2) + leaf.Position.y;
            //return ((summerDot + winterDot) / 2) + Mathf.Pow(leaf.Position.y + 1, 2);
            if (leaf.Position.y < 1)
                return 0; // I do not want leaves going into the ground to perform well at all

            //return ((summerDot + winterDot)) + (Mathf.Log(leaf.Position.y + 1, 11));
            return (((summerDot + winterDot)) + Mathf.Pow(Mathf.Min(leaf.Position.y / 3, 2), 2)) / 6;
            //return ((summerDot + winterDot)) + (Mathf.Pow((leaf.Position.y - 1) / 5, (float) 1 / 3) + 0.2f);
        }

        public SunInformation GetSunInformation()
        {
            return _sunInformation;
        }
    }
}
