using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGA
{
    public enum FirstGenerationGenerator
    {
        Rand, NearestNeighbor
    };

    public enum CrossoverMethod
    {
        OX, PMX
    };

    public enum MutationMethod
    {
       Saltation, Inversion
    };

    public enum SelectionMethod
    {
        Tourney, RouletteWheel
    };
    public class GeneticAlgorithm
    {

        private int genomLength;                    //длина генома
        private int genomsInGeneration;             //количество геномов в поколении
        private FirstGenerationGenerator fgg;       //генератор первого поколения
        private CrossoverMethod cm;                 //способ кроссовера
        private SelectionMethod sm;                 //способ селекции
        private MutationMethod mm;                  //способ мутации
        public FitnessFunction FitFunc;
        private double mutationProbability;
        private int generationCount;
        private Path BestPath;
        public List<Path> Generation;
        public List<Path> NextGeneration;

        public GeneticAlgorithm(int genomsInGeneration, int generationCount, double mutProb, FirstGenerationGenerator _fgg, CrossoverMethod _cm,
            MutationMethod _mm, SelectionMethod _sm, FitnessFunction ff)
        {
            this.genomLength = ff.GetArity();
            this.genomsInGeneration = genomsInGeneration;
            this.fgg = _fgg;
            this.mutationProbability = mutProb;
            this.generationCount = generationCount;
            Generation = new List<Path>();
            NextGeneration = new List<Path>();
            FitFunc = ff;
            this.cm = _cm;
            this.mm = _mm;
            this.sm = _sm;
        }

        public void CreateFirstGeneration()
        {
            switch (fgg)
            {
                case FirstGenerationGenerator.NearestNeighbor:
                    {
                        Random rnd = new Random();
                        for (int i = 0; i < genomsInGeneration; i++)
                        {
                            int r = rnd.Next(genomLength - 1);
                            Generation.Add(FitFunc.NearestNeighbor(r));
                        }
                        break;
                    }
                case FirstGenerationGenerator.Rand:
                    {
                        Random rnd = new Random();
                        int RGT;
                        for (int k = 0; k < genomsInGeneration; k++)
                        {
                            RGT = 0;
                            Path path = new Path(genomLength);
                            for (int i = 0; i < genomLength; i++)
                                path[i] = i;
                            for (int i = 0; i < genomLength; i++)
                            {
                                int r = rnd.Next(RGT, genomLength - 1);
                                int temp = path[RGT];
                                path[RGT] = path[r];
                                path[r] = temp;
                                RGT++;
                            }
                            Generation.Add(path);
                        }
                        break;
                    }
            }
        }

        public void Reproduction()
        {
            Random rnd = new Random();
            for (int i = 0; i < genomsInGeneration / 2; i++)
            {
                int numOfFirstParent = rnd.Next(genomsInGeneration - 1);
                int numOfSecondParent = rnd.Next(genomsInGeneration - 1);
                while (numOfFirstParent == numOfSecondParent)
                    numOfSecondParent = rnd.Next(genomsInGeneration - 1);
                Crossing(Generation[numOfFirstParent], Generation[numOfSecondParent]);
            }
        }

        public void Crossing(Path firstParent, Path secondParent)
        {
            switch (cm)
            {
                case CrossoverMethod.OX:
                    {
                        OX(firstParent, secondParent);
                        break;
                    }
                case CrossoverMethod.PMX:
                    {
                        PMX(firstParent, secondParent);
                        break;
                    }
            }
        }

        public void Mutation(Path p)
        {
            Random rnd = new Random();
            double r = rnd.NextDouble();
            if (r < mutationProbability)
                switch (mm)
                {
                    case MutationMethod.Inversion:
                        {
                            Inversion(p);
                            break;
                        }
                    case MutationMethod.Saltation:
                        {
                            Saltation(p);
                            break;
                        }
                }
        }

        public void Inversion(Path path)
        {
            Random rnd = new Random();
            int Right = rnd.Next(genomLength - 1);
            int Left = rnd.Next(genomLength - 1);
            while (Left == Right)
                Left = rnd.Next(genomLength - 1);
            if (Right < Left)
            {
                int tmp = Right;
                Right = Left;
                Left = tmp;
            }
            while (Left < Right)
            {
                int temp = path[Left];
                path[Left] = path[Right];
                path[Right] = temp;
                Left++;
                Right--;
            }
        }

        public void Saltation(Path path)
        {
            path.Show();
            Random rnd = new Random();
            int Right = rnd.Next(genomLength - 1);
            int Left = rnd.Next(genomLength - 1);
            System.Console.Write("{0} {1}", Left, Right);
            int temp = path[Left];
            path[Left] = path[Right];
            path[Right] = temp;
            path.Show();
        }

        public void OX(Path firstParent, Path secondParent)
        {
            Random rnd = new Random();
            int Right = rnd.Next(genomLength - 1);
            int Left = rnd.Next(genomLength - 1);
            bool[] cityInPath1 = new bool[genomLength];
            bool[] cityInPath2 = new bool[genomLength];
            while (Left == Right)
                Left = rnd.Next(genomLength - 1);
            if (Right < Left)
            {
                int tmp = Right;
                Right = Left;
                Left = tmp;
            }
            Path firstChild = new Path(genomLength);
            Path secondChild = new Path(genomLength);
            for (int i = Left; i <= Right; i++)
            {
                firstChild[i] = firstParent[i];
                secondChild[i] = secondParent[i];
                cityInPath1[firstParent[i]] = true;
                cityInPath2[secondParent[i]] = true;
            }
            int Iterator = (Right + 1) % genomLength;
            int Position = (Right + 1) % genomLength;
            while (Position != Left)
            {
                if (!cityInPath1[secondParent[Iterator]])
                {
                    firstChild[Position] = secondParent[Iterator];
                    cityInPath1[secondParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                Iterator = (Iterator + 1) % genomLength;
            }

            Iterator = (Right + 1) % genomLength;
            Position = (Right + 1) % genomLength;
            while (Position != Left)
            {
                if (!cityInPath2[firstParent[Iterator]])
                {
                    secondChild[Position] = firstParent[Iterator];
                    cityInPath2[firstParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                Iterator = (Iterator + 1) % genomLength;
            }
            Generation.Add(firstChild);
            Generation.Add(secondChild);
        }

        public void PMX(Path firstParent, Path secondParent)
        {
            Random rnd = new Random();
            int Right = rnd.Next(genomLength);
            int Left = rnd.Next(genomLength);
            bool[] cityInPath1 = new bool[genomLength];
            bool[] cityInPath2 = new bool[genomLength];
            while (Left == Right)
                Left = rnd.Next(genomLength - 1);
            if (Right < Left)
            {
                int tmp = Right;
                Right = Left;
                Left = tmp;
            }
            Path firstChild = new Path(genomLength);
            Path secondChild = new Path(genomLength);
            for (int i = Left; i <= Right; i++)
            {
                firstChild[i] = firstParent[i];
                secondChild[i] = secondParent[i];
                cityInPath1[firstParent[i]] = true;
                cityInPath2[secondParent[i]] = true;
            }
            int Iterator = (Right + 1) % genomLength;
            int Position = (Right + 1) % genomLength;
            while (Position != Left)
            {
                if (!cityInPath1[secondParent[Iterator]])
                {
                    firstChild[Position] = secondParent[Iterator];
                    cityInPath1[secondParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                else
                if (!cityInPath1[firstParent[Iterator]])
                {
                    firstChild[Position] = firstParent[Iterator];
                    cityInPath1[firstParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                Iterator = (Iterator + 1) % genomLength;
            }
            Iterator = (Right + 1) % genomLength;
            Position = (Right + 1) % genomLength;
            while (Position != Left)
            {
                if (!cityInPath2[firstParent[Iterator]])
                {
                    secondChild[Position] = firstParent[Iterator];
                    cityInPath2[firstParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                else
                if (!cityInPath2[secondParent[Iterator]])
                {
                    secondChild[Position] = secondParent[Iterator];
                    cityInPath2[secondParent[Iterator]] = true;
                    Position = (Position + 1) % genomLength;
                }
                Iterator = (Iterator + 1) % genomLength;
            }
            Generation.Add(firstChild);
            Generation.Add(secondChild);
        }

        public void Run()
        {
            int CurrentGeneration = 1;
            CreateFirstGeneration();
            ShowInfo(Generation);
            while (CurrentGeneration <= generationCount)
            {
                Reproduction();
                for (int i = genomsInGeneration; i < Generation.Count(); i++)
                    Mutation(Generation[i]);
                Selection();
                Merge();
                System.Console.WriteLine("Generation {0}", CurrentGeneration);
                ShowInfo(Generation);
                CurrentGeneration++;
            }
        }

        private void ShowInfo(List<Path> path)
        {
            foreach (Path p in path)
            {
                p.Show();
                System.Console.WriteLine("\t{0}", FitFunc.PathLength(p));
            }
        }

        private Path BestSolution(List<Path> path)
        {
            Path solve = path[0];
            double min = FitFunc.PathLength(path[0]);
            foreach(Path p in path)
                if (FitFunc.PathLength(p)<min)
                {
                    min = FitFunc.PathLength(p);
                    solve = p;
                }
            return solve;
        }

        private void Selection()
        {
            switch(sm)
            {
                case SelectionMethod.Tourney:
                    {
                        Tourney();
                        break;
                    }
                case SelectionMethod.RouletteWheel:
                    {
                        RouletteWheel();
                        break;
                    }
            }
        }

        private void Tourney()
        {
            Random rnd = new Random();
            for (int i = 0; i < Generation.Count; i++)
            {
                int Ind1 = rnd.Next(genomsInGeneration - 1);
                int Ind2 = rnd.Next(genomsInGeneration - 1);
                while (Ind2 == Ind1)
                    Ind2 = rnd.Next(genomsInGeneration);
                double len1 = FitFunc.PathLength(Generation[Ind1]);
                double len2 = FitFunc.PathLength(Generation[Ind2]);
                if (len1 < len2)
                    NextGeneration.Add(Generation[Ind1]);
                else
                    NextGeneration.Add(Generation[Ind2]);
            }
        }

        private void RouletteWheel()
        {
            Random rnd = new Random();
            double[] fitness = new double[genomsInGeneration];
            fitness[0] = FitFunc.PathLength(Generation[genomsInGeneration]);
            for (int i = genomsInGeneration + 1; i < Generation.Count(); i++)
                fitness[i] = fitness[i - 1] + FitFunc.PathLength(Generation[i]);
            double summary = fitness[Generation.Count() - 1];
            for (int i=0;i<genomsInGeneration;i++)
            {
                double index = rnd.NextDouble() * summary;
                int start = genomsInGeneration;
                int end = 2 * genomsInGeneration - 1;
                int center = 0;
                while(start<end)
                {
                    center = (start + end) / 2;
                    if (index <= fitness[center])
                        end = center;
                    else
                        start = center + 1;
                }
                NextGeneration[i] = Generation[end];
            }
        }

        private void Merge()
        {
            Generation.Clear();
            foreach(Path p in NextGeneration )
            {
                Generation.Add(new Path(p));
            }
            NextGeneration.Clear();
        }
    }
}
