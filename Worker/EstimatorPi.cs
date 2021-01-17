using System;
using System.Linq;
using System.Threading.Tasks;

namespace Worker
{
    public class EstimatorPi
    {
        private static readonly int NumberOfCores = Environment.ProcessorCount;

        public double Estimate(int numberOfSteps)
        {
            int[] localCounters = new int[numberOfSteps];
            Task[] tasks = new Task[numberOfSteps];

            for (int i = 0; i < numberOfSteps; i++)
            {
                int procIndex = i; //closure capture 
                tasks[procIndex] = Task.Factory.StartNew(() =>
                {
                    int localCounterInside = 0;
                    Random random = new Random();

                    for (int j = 0; j < numberOfSteps / NumberOfCores; ++j)
                    {
                        double x = random.NextDouble();
                        double y = random.NextDouble();
                        double z = Math.Pow(x, 2) + Math.Pow(y, 2);
                        if (z <= 1.0) localCounterInside++;
                    }
                    localCounters[procIndex] = localCounterInside;
                });
            }
            Task.WaitAll(tasks);
            long count = localCounters.Sum();

            double pi = 4 * (count / (double)numberOfSteps);

            return pi;
        }
    }
}
