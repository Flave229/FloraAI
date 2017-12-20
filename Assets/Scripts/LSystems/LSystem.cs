using Assets.Scripts.TurtleGeometry;
using UnityEngine;

namespace Assets.Scripts.LSystems
{
    public class LSystem
    {
        private readonly TurtlePen _turtlePen;

        public LSystem(TurtlePen turtlePen)
        {
            _turtlePen = turtlePen;
        }

        public void Update()
        {
            // Generate new command string here
        }

        public void Draw(Vector3 startingPosition)
        {
            _turtlePen.Draw(startingPosition, "FF+F");
        }
    }
}
