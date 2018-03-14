using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenTwoPlantsWithTheSameAmountOfLeaves
{
    class WhenBothPlantsHaveLeavesPointingInTheSameDirection
    {
        [Test]
        public void ThenTheTwoPlantsWillHaveTheSameDynamicPhototropismFitnessValue()
        {
            PlantFitness plantFitness = new PlantFitness(new LeafFitness(new SunInformation
            {
                SummerAltitude = 90,
                WinterAltitude = 90,
                Azimuth = 0
            }));

            Mock<GeometryRenderSystem> geometryRenderMock = new Mock<GeometryRenderSystem>();
            TurtlePen turtlePen = new TurtlePen(geometryRenderMock.Object)
            {
                ForwardStep = 1
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("FFO");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            Plant plant1 = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero);
            plant1.Generate();
            float plant1Fitness = plantFitness.EvaluateDynamicPhototrophicFitness(plant1);

            Mock<ILSystem> lSystem2Mock = new Mock<ILSystem>();
            lSystem2Mock.Setup(x => x.GetCommandString()).Returns("FFO");
            PersistentPlantGeometryStorage geometryStorage2 = new PersistentPlantGeometryStorage();
            Plant plant2 = new Plant(lSystem2Mock.Object, turtlePen, geometryStorage2, Vector3.zero);
            plant2.Generate();
            float plant2Fitness = plantFitness.EvaluateDynamicPhototrophicFitness(plant2);

            Debug.Log("Plant 1 Fitness: " + plant1Fitness);
            Debug.Log("Plant 2 Fitness: " + plant2Fitness);
            Assert.That(plant2Fitness, Is.EqualTo(plant1Fitness));
        }
    }
}
