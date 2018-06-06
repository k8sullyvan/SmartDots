using System;
using System.Collections.Generic;

namespace Dots_OnForm
{
    public class Brain
    {
        private int size;

        public KeyValuePair<double, double>[] Directions { get; set; }

        public Brain(int size)
        {
            this.size = size;
            Directions = new KeyValuePair<double, double>[size];
            SetRandomDirections();
        }

        private Brain(KeyValuePair<double, double>[] directions)
        {
            size = directions.Length;
            Directions = directions;
        }

        /// <summary>
        /// Returns an exact copy of a given brain
        /// </summary>
        /// <returns></returns>
        public Brain Clone()
        {
            return new Brain(Directions);
        }

        /// <summary>
        /// Sets all the directions in the brain to new, random, directions. 
        /// </summary>
        private void SetRandomDirections()
        {
            for (int i = 0; i < size; i++)
            {
                double xPart = RandomDirection();
                double yPart = RandomDirection();

                Directions[i] = new KeyValuePair<double, double>(xPart, yPart);
            }
        }

        /// <summary>
        /// Returns the direction in the brain at a given time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public KeyValuePair<double, double> GetDirectionAtTime(int time)
        {
            return Directions[time];
        }

        /// <summary>
        /// Returns a random direction between -1.0 and 1.0
        /// </summary>
        /// <returns></returns>
        private static double RandomDirection()
        {
            return Dots.random.NextDouble() + Dots.random.NextDouble() - 1.0;
        }

        /// <summary>
        /// Takes the current direction and alters it up to 0.4 in any direction. Returns a value direction between -1.0 and 1.0.
        /// </summary>
        /// <param name="currentDirection"></param>
        /// <returns></returns>
        private static double MutateDirection(double currentDirection)
        {
            double tweakedDirection = (RandomDirection() * 0.4) + currentDirection;
            double boundedDirection = Math.Min(1.0, Math.Max(-1.0, tweakedDirection));
            return boundedDirection;
        }

        /// <summary>
        /// Returns a slightly mutated version of a given Brain
        /// </summary>
        /// <returns></returns>
        public Brain Mutate()
        {
            KeyValuePair<double, double>[] Directions = new KeyValuePair<double, double>[size];
            double mutationThreashold = 0.01; // 1% chance of mutation
            for (int i = 0; i < size; i++)
            {
                if (Dots.random.NextDouble() < mutationThreashold)
                {
                    Directions[i] = new KeyValuePair<double, double>
                        (MutateDirection(Directions[i].Key), MutateDirection(Directions[i].Value));
                }
                else
                {
                    Directions[i] = this.Directions[i];
                }
            }
            return new Brain(Directions);
        }
    }
}
