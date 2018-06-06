using System;
using System.Threading;
using System.Windows.Forms;

namespace Dots_OnForm
{
    static class Program
    {
        private const int POPULATION_SIZE = 40;
        private const int CITIZEN_LIEFSPAN = 400;

        public static Form1 myForm;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread myThread;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new Form1();
            myThread = new Thread(new ThreadStart(Evolution));
            myThread.Start();
            Application.Run(myForm);
        }

        private static void Evolution()
        {
            Thread.Sleep(100);

            int generationNumber = 0;
            int minimumSteps = CITIZEN_LIEFSPAN;
            Population dotPopulation = new Population(POPULATION_SIZE, CITIZEN_LIEFSPAN);

            for (int i = 0; i <= dotPopulation.LifeSpan; i++)
            {
                try
                {
                    myForm.Invoke(new MethodInvoker(delegate
                    {
                        myForm.ResetCanvas();
                        myForm.WriteToCanvas("Generation: " + generationNumber, new System.Drawing.Point(0, 0));
                        myForm.WriteToCanvas("Min Steps: " + minimumSteps, new System.Drawing.Point(0, 30));
                        myForm.DrawTarget(dotPopulation.targetPoint);
                        myForm.RenderPopulation(dotPopulation, i);
                        myForm.Update();
                    }));
                }
                catch (Exception)
                {
                    System.Diagnostics.Debug.WriteLine("Shutting down");
                    break;
                }

                if (i == dotPopulation.LifeSpan || dotPopulation.AreAllDead())
                {
                    while (!dotPopulation.AreAllDead()) ;

                    generationNumber++;
                    dotPopulation.GetNextGeneration();
                    i = 0;
                }

                if (dotPopulation.ReadchedTarget())
                {
                    minimumSteps = Math.Min(minimumSteps, i);
                }

                Thread.Sleep(50);
            }
        }
    }
}

