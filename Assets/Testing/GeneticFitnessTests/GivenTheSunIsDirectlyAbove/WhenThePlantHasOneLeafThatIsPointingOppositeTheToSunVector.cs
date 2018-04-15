using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenTheSunIsDirectlyAbove
{
    class WhenThePlantHasOneLeafThatIsPointingOppositeTheToSunVector
    {
        [Test]
        public void ThenTheDynamicPhototrophicFitnessIsZero()
        {
            PlantFitness plantFitness = new PlantFitness(new LeafFitness(new SunInformation
            {
                SummerAltitude = 90,
                WinterAltitude = 90,
                Azimuth = 0
            }));

            TurtlePen turtlePen = new TurtlePen(new GeometryRenderSystem())
            {
                ForwardStep = 1,
                RotationStep = 90.0f,
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("F+O");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            Plant plant1 = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero, Color.black);
            plant1.Generate();
            float plantFitnessValue = plantFitness.EvaluateDynamicPhototrophicFitness(plant1);

            Debug.Log("Plant 1 Fitness: " + plantFitnessValue);
            Assert.That(plantFitnessValue, Is.EqualTo(0));
        }
    }
}