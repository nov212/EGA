using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGA
{
    class Program
    {
        static void Main(string[] args)
        {
            FirstGenerationGenerator FGG = new FirstGenerationGenerator();
            CrossoverMethod CM = new CrossoverMethod();
            MutationMethod MM = new MutationMethod();
            SelectionMethod SM = new SelectionMethod();
            double mutationProbability = 0;
            int genomsInGeneration = 0;
            int generationCount = 0;
            int option = 0;
            System.Console.WriteLine("Enter count of genoms");
            genomsInGeneration = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Enter count of generations");
            generationCount = Convert.ToInt32(Console.ReadLine());
            System.Console.WriteLine("Enter mutation probability");
            mutationProbability = Convert.ToDouble(Console.ReadLine());
            System.Console.WriteLine("Choose first generatioh  generator: 1-Random  2-Nearest neighbor");
            option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
                FGG = FirstGenerationGenerator.Rand;
            else
                if (option == 2)
                FGG = FirstGenerationGenerator.NearestNeighbor;
            System.Console.WriteLine("Choose crossover method: 1-OX  2-PMX");
            option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
                CM = CrossoverMethod.OX;
            else
                if (option == 2)
                CM = CrossoverMethod.PMX;
            System.Console.WriteLine("Choose mutation method: 1-Inversion  2-Saltation");
            option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
                MM = MutationMethod.Inversion;
            else
                if (option == 2)
                MM = MutationMethod.Saltation;
            System.Console.WriteLine("Choose selection method: 1-Tourney  2-Roulette wheel");
            option = Convert.ToInt32(Console.ReadLine());
            if (option == 1)
                SM = SelectionMethod.Tourney;
            else
                if (option == 2)
                SM = SelectionMethod.RouletteWheel;
            FitnessFunction ff = new FitnessFunction(new Matrix("C:/Users/Ильяс/source/repos/EGA/EGA/m1.txt"));
            GeneticAlgorithm ga = new GeneticAlgorithm(genomsInGeneration, generationCount, mutationProbability, FGG, CM, MM, SM, ff);
            ga.Run();
            System.Console.ReadKey();
        }
    }
}
