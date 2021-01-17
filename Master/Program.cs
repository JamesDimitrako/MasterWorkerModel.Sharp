using System;
using System.Linq;
using System.Threading.Tasks;
using Worker;

namespace Master
{
    class Program
    {
        private static readonly int NumberOfCores = Environment.ProcessorCount;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World from Master!");
            Console.WriteLine("Please enter number of steps");

            double[] localPiEstimations = new double[NumberOfCores];
            int numberOfSteps;
            var input = Console.ReadLine();

            bool validInput = Int32.TryParse(input, out numberOfSteps);
            if (!validInput)
                throw new ArgumentException("Not a valid number;");

            Task[] tasks = new Task[NumberOfCores];

            for (int i = 0; i < NumberOfCores; i++)
            {
                int procIndex = i;
                double localPiEstimation;
                tasks[procIndex] = Task.Factory.StartNew(() =>
                {
                    var estimator = new EstimatorPi();
                    localPiEstimation = estimator.Estimate(numberOfSteps/NumberOfCores);
                    localPiEstimations[procIndex] = localPiEstimation;
                });
            }

            Task.WaitAll(tasks);
            double count = localPiEstimations.Sum();
            double pi = 4 * (count / (double)numberOfSteps);
            Console.WriteLine($"The pi estimation is: {pi}");

        }
    }
}
