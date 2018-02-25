using Assets.Scripts;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenAPlant
{
    class WhenThePlantHasNoLeaves
    {
        private PlantFitness _plantFitness;
        private Plant _plant;

        [SetUp]
        public void SetUp()
        {
            _plantFitness = new PlantFitness(Vector3.zero);

            Mock<GeometryRenderSystem> geometryRenderMock = new Mock<GeometryRenderSystem>();
            TurtlePen turtlePen = new TurtlePen(geometryRenderMock.Object)
            {
                ForwardStep = 1
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("FF");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            _plant = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero);
            _plant.Generate();
        }

        [Test]
        public void ThenThePlantHasZeroUpwardsPhototrophicFitness()
        {
            float plant1Fitness = _plantFitness.EvaluateUpwardsPhototrophicFitness(_plant);

            Debug.Log("Small Plant Fitness: " + plant1Fitness);
            Assert.That(plant1Fitness, Is.EqualTo(0));
        }

        [Test]
        public void ThenThePlantHasZeroDynamicPhototrophicFitness()
        {
            float plant1Fitness = _plantFitness.EvaluateDynamicPhototrophicFitness(_plant);

            Debug.Log("Small Plant Fitness: " + plant1Fitness);
            Assert.That(plant1Fitness, Is.EqualTo(0));
        }
    }
}
