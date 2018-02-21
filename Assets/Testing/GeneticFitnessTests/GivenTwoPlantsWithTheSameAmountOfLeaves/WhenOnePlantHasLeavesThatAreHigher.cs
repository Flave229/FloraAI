using Assets.Scripts;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenTwoPlantsWithTheSameAmountOfLeaves
{
    class WhenOnePlantHasLeavesThatAreHigher
    {
        [Test]
        public void ThenThePlantWithHigherLeavesHasAHigherPositivePhototropismFitnessValue()
        {
            PlantFitness plantFitness = new PlantFitness();

            Mock<GeometryRenderSystem> geometryRenderMock = new Mock<GeometryRenderSystem>();
            TurtlePen turtlePen = new TurtlePen(geometryRenderMock.Object)
            {
                ForwardStep = 1
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("FO");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            Plant plant1 = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero);
            plant1.Generate();
            float plant1Fitness = plantFitness.EvaluatePositivePhototrophicFitness(plant1);

            Mock<ILSystem> lSystem2Mock = new Mock<ILSystem>();
            lSystem2Mock.Setup(x => x.GetCommandString()).Returns("FFO");
            PersistentPlantGeometryStorage geometryStorage2 = new PersistentPlantGeometryStorage();
            Plant plant2 = new Plant(lSystem2Mock.Object, turtlePen, geometryStorage2, Vector3.zero);
            plant2.Generate();
            float plant2Fitness = plantFitness.EvaluatePositivePhototrophicFitness(plant2);

            Debug.Log("Small Plant Fitness: " + plant1Fitness);
            Debug.Log("Larger Plant Fitness: " + plant2Fitness);
            Assert.That(plant2Fitness, Is.GreaterThan(plant1Fitness));
        }
    }
}
