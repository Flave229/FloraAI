using System;
using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Genetic_Algorithm;
using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Testing.GeneticFitnessTests.GivenAPlant
{
    class WhenThePlantHasOneBranchWhichCanOnlySupportOneLeafButItHasTwoLeavesAttached
    {
        [Test]
        public void ThenOnlyOneLeafsEnergyIsTransferedThroughTheBranch()
        {
            Mock<ILeafFitness> leafFitnessMock = new Mock<ILeafFitness>();
            leafFitnessMock.Setup(x => x.EvaluatePhotosyntheticRate(It.IsAny<Leaf>()))
                .Returns(1);
            PlantFitness plantFitness = new PlantFitness(leafFitnessMock.Object);

            TurtlePen turtlePen = new TurtlePen(new GeometryRenderSystem())
            {
                ForwardStep = 1,
                RotationStep = 90.0f,
                BranchDiameter = 0.01f
            };

            Mock<ILSystem> lSystem1Mock = new Mock<ILSystem>();
            lSystem1Mock.Setup(x => x.GetCommandString()).Returns("F+O--O");
            PersistentPlantGeometryStorage geometryStorage1 = new PersistentPlantGeometryStorage();
            Plant plant1 = new Plant(lSystem1Mock.Object, turtlePen, geometryStorage1, Vector3.zero);
            plant1.Generate();
            
            Fitness plantFitnessObject = plantFitness.EvaluatePhloemTransportationFitness(plant1);
            float plantFitnessValue = plantFitnessObject.LeafEnergy - plantFitnessObject.BranchCost;

            Debug.Log("Plant 1 Fitness: " + plantFitnessValue);
            Assert.That(Math.Abs(plantFitnessValue - (1 - 0.000314f)), Is.LessThan(0.0001f));
        }
    }
}
