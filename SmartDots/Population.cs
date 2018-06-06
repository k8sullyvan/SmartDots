using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Dots_OnForm
{
    class Population
    {
        public Point startPoint = new Point(500, 200);
        public Point targetPoint = new Point(200, 200);

        public int LifeSpan { get; private set; }

        public Dots[] Citizens;

        public Population(int populationSize, int LifeSpan)
        {
            this.LifeSpan = LifeSpan;
            Citizens = new Dots[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                Citizens[i] = new Dots(startPoint, LifeSpan);
            }
        }

        /// <summary>
        /// Checks if all the citizens are dead. 
        /// True if all the citizens are dead, false if any citizens are still alive. 
        /// </summary>
        /// <returns></returns>
        public bool AreAllDead()
        {
            return Citizens.All(d => !d.IsAlive);
        }

        /// <summary>
        /// Checks if all the citizens are dead. 
        /// True if at least one Citizen is dead, false if any citizens are still alive.
        /// </summary>
        /// <returns></returns>
        public bool ReadchedTarget()
        {
            return Citizens.Any(d => d.ReadchedTarget());
        }

        /// <summary>
        /// Advance the position of the citizen and save the value. 
        /// Returns the new position of the citizen and the citizens colour, as a brush.
        /// </summary>
        /// <param name="time"></param>
        /// <returns>Key is the Colour of the citizen, Value is the position of the citizen</returns>
        public KeyValuePair<Brush, Point>[] GetSnapshotAt(int time)
        {
            KeyValuePair<Brush, Point>[] renderInformation = new KeyValuePair<Brush, Point>[Citizens.Length];

            for (int i = 0; i < Citizens.Length; i++)
            {
                if (Citizens[i].IsAlive)
                {
                    Citizens[i].StepToTarget(time, targetPoint);
                }
                renderInformation[i] = new KeyValuePair<Brush, Point>(Citizens[i].GetColour(), Citizens[i].Position);
            }

            return renderInformation;
        }

        /// <summary>
        /// Generates the next generation by taking some of the fittest citizens in the previous generation and creating mutated children from them.
        /// </summary>
        public void GetNextGeneration()
        {
            // Calculate fitness of every citizen, and keep track of the fittest citizen
            double populationFitness = 0.0;
            Dots fittestDot = Citizens[0];
            foreach (var individual in Citizens)
            {
                individual.CalculateFitness(targetPoint);
                populationFitness += individual.fitness;
                fittestDot = individual.fitness > fittestDot.fitness ? individual : fittestDot;
            }

            // Keep an exact copy of the fittest citizen to prevent devolution
            Dots[] nextGen = new Dots[Citizens.Length];
            nextGen[0] = fittestDot.Child(startPoint);
            //nextGen[0].Colour = Color.FromArgb(255, 255, 0, 255);

            // Generate a new population and mutate it
            for (int i = 1; i < Citizens.Length; i++)
            {
                nextGen[i] = RandomChild(populationFitness);
                nextGen[i].Mutate();
                //nextGen[i].Colour = Color.FromArgb(150, 0, 255, 255);
            }
            Citizens = nextGen;
        }

        /// <summary>
        /// Returns a child from the previous population.
        /// This is not strictly random as fitter citizens are more heavily weighted, and will be more likely to produce offspring.
        /// 
        /// Child is an exact clone of the parent.
        /// </summary>
        /// <param name="populationFitness"></param>
        /// <returns>A child that is an exact clone of the parent</returns>
        private Dots RandomChild(double populationFitness)
        {
            double threashold = Dots.random.NextDouble() * populationFitness;
            double runningTotal = 0.0;
            foreach (var individual in Citizens)
            {
                runningTotal += individual.fitness;
                if (runningTotal > threashold)
                {
                    return individual.Child(startPoint);
                }
            }
            return null;
        }
    }
}
