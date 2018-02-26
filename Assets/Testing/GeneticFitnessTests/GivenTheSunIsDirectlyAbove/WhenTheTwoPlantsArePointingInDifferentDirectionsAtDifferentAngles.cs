using Assets.Scripts;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenTheSunIsDirectlyAbove
{
    class WhenTheTwoPlantsArePointingInDifferentDirectionsAtDifferentAngles
    {
        [Test]
        public void ThenTheDynamicPhototrophicFitnessIsHigherForTheLeafPointingDirectlyAtTheSun()
        {
            PlantFitness plantFitness = new PlantFitness(new Vector3(0, 3, 0));
            
            Vector3 rightVector = new Vector3(0, 1, 0);
            TurtlePen turtlePen = new TurtlePen(new GeometryRenderSystem())
            {
                ForwardStep = 1,
                RotationStep = 90.0f,
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("F-O");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            Plant plant1 = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero);
            plant1.Generate();
            float plant1Fitness = plantFitness.EvaluateDynamicPhototrophicFitness(plant1);

            Mock<ILSystem> lSystem2Mock = new Mock<ILSystem>();
            lSystem2Mock.Setup(x => x.GetCommandString()).Returns("F+O");
            PersistentPlantGeometryStorage geometryStorage2 = new PersistentPlantGeometryStorage();
            Plant plant2 = new Plant(lSystem2Mock.Object, turtlePen, geometryStorage2, Vector3.zero);
            plant2.Generate();
            float plant2Fitness = plantFitness.EvaluateDynamicPhototrophicFitness(plant2);

            Debug.Log("Plant 1 Fitness: " + plant1Fitness);
            Debug.Log("Plant 2 Fitness: " + plant2Fitness);
            Assert.That(plant1Fitness, Is.GreaterThan(plant2Fitness));
        }
    }
}
