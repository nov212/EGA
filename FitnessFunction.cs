using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGA
{
    public class FitnessFunction
    {
        private int Arity;
        private Matrix Distances;
        public FitnessFunction(Matrix dist)
        {
            Distances = dist;
            Arity = dist.GetRange();
        }

        public double PathLength(Path path)
        {
            int i;
            double length = 0;
            for (i = 0; i < path.CitiesCount - 1; i++)
                length += Distances[path[i], path[i + 1]];
            length += Distances[path[i], path[0]];
            return length;
        }

        public Path NearestNeighbor(int start)
        {
            int RGT = 0;
            Path path = new Path(Arity);
            for (int j = 0; j < Arity; j++)
                path[j] = j;
            int temp = path[start];
            path[start] = path[0];
            path[0] = temp;
            RGT++;
            while (RGT < Arity)
            {
                double min = Distances[path[RGT - 1], path[RGT]];
                int choise = RGT;
                for (int j = RGT + 1; j < Arity; j++)
                    if (Distances[path[RGT - 1], path[j]] < min)
                    {
                        min = Distances[path[RGT - 1], path[j]];
                        choise = j;
                    }
                temp = path[RGT];
                path[RGT] = path[choise];
                path[choise] = temp;
                RGT++;
            }
            return path;
        }

        public int GetArity()
        {
            return Arity;
        }

    }
}
