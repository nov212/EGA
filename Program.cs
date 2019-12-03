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
            //Matrix m = new Matrix("C:/Users/Ильяс/source/repos/EGA/EGA/m1.txt");
            //m.PrintMatrix();
            //System.Console.WriteLine("{0}", m[0, 1]);
            FitnessFunction ff = new FitnessFunction(new Matrix("C:/Users/Ильяс/source/repos/EGA/EGA/m1.txt"));
            GeneticAlgorithm ga = new GeneticAlgorithm(7, 10, 10, FirstGenerationGenerator.NearestNeighbor, CrossoverMethod.OX, MutationMethod.Inversion, SelectionMethod.Tourney,ff);
            ga.Run();
            System.Console.ReadKey();
        }
    }
}
