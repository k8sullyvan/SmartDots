using System;
using System.Drawing;

namespace Dots_OnForm
{
    public class Dots
    {
        public static readonly Random random = new Random();

        private const double MULTIPLIER = 1000.0;
        private const double MAX_VELOCITY = 10.0;

        public Point Position;
        public bool IsAlive = false;

        private int lifeSpan;
        private Brain brain;
        public Color Colour;

        private int stepsToTarget = -1;
        public double fitness = 0.0;

        /// <summary>
        /// Creates a new Dot with a given startpoint, brain and colour
        /// </summary>
        /// <param name="startPoint">Point for the dot to spawn from</param>
        /// <param name="brain">The instructions for the dot</param>
        /// <param name="color">Colour of the dot</param>
        private Dots(Point startPoint, Brain brain, Color color)
        {
            IsAlive = true;

            Position = startPoint;

            this.brain = brain.Clone();
            this.Colour = color;
        }

        /// <summary>
        /// Creates a Dot with a given starting point and lifespan
        /// </summary>
        /// <param name="startPoint">Point for the dot to spawn from</param>
        /// <param name="lifeSpan">Maximum 'age' of the dot, the number of </param>
        public Dots(Point startPoint, int lifeSpan)
        {
            IsAlive = true;

            Position = startPoint;
            this.lifeSpan = lifeSpan;

            brain = new Brain(lifeSpan);
            Colour = Color.FromArgb(255, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="time">Current time</param>
        /// <param name="target">Target the dot is aiming for</param>
        /// <returns></returns>
        public Point StepToTarget(int time, Point target)
        {
            if (time < lifeSpan)
            {
                if (IsAlive)
                {
                    Position.X = (int)Math.Round(brain.GetDirectionAtTime(time).Key * MAX_VELOCITY) + Position.X;
                    Position.Y = (int)Math.Round(brain.GetDirectionAtTime(time).Value * MAX_VELOCITY) + Position.Y;

                    // Check the dot is within the bounds of the window
                    IsAlive = Position.X < 800 && Position.X > 0 && Position.Y < 450 && Position.Y > 0;

                    if (DistanceTo(target) <= 10.0)
                    {
                        IsAlive = false;
                        stepsToTarget = time;
                    }
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Dot has died of old age");
                IsAlive = false;
            }
            return Position;
        }

        public Brush GetColour()
        {
            return new SolidBrush(this.Colour);
        }

        public void CalculateFitness(Point target)
        {
            double dist = DistanceTo(target);
            fitness = MULTIPLIER / (dist*dist);
            if (stepsToTarget > 0)
            {
                fitness += MULTIPLIER * ((lifeSpan - stepsToTarget) * (lifeSpan - stepsToTarget));
            }
        }

        public double DistanceTo(Point target)
        {
            int side1 = (Position.X - target.X) * (Position.X - target.X);
            int side2 = (Position.Y - target.Y) * (Position.Y - target.Y);
            return Math.Sqrt(side1 + side2);
        }

        public Dots Child(Point startPoint)
        {
            return new Dots(startPoint, brain, Colour);
        }

        public void Mutate()
        {
            this.brain = brain.Mutate();

            // mutate the colour, but only a little, to show its in the same family
            Colour = Color.FromArgb( Colour.A,
                Math.Max(Math.Min(Colour.R + random.Next(-50, 50), 254), 0),
                Math.Max(Math.Min(Colour.G + random.Next(-50, 50), 254), 0),
                Math.Max(Math.Min(Colour.B + random.Next(-50, 50), 254), 0));
        } 

        public bool ReadchedTarget()
        {
            return stepsToTarget > 0;
        }
    }
}
