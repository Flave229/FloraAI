using Assets.Scripts.LSystems;
using Assets.Scripts.Render;
using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts
{
    class PlantSpawner : MonoBehaviour
    {
        private Plant _plant;

        private void Start()
        {
            GeometryRenderSystem renderSystem = new GeometryRenderSystem();
            TurtlePen turtlePen = new TurtlePen(renderSystem)
            {
                ForwardStep = 1,
                RotationStep = 30
            };
            LSystem lindenMayerSystem = new LSystem(turtlePen);
            _plant = new Plant(lindenMayerSystem, new Vector3(transform.position.x + 1, transform.position.y + 0.5f, transform.position.z + 1));
            _plant.Generate();
        }
    }
}