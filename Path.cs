using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGA
{
   public class Path: IEquatable<Path>
    {
        private int citiesCount;    //количество городов
        private int[] Cities;       //номера городов
        public Path(int count)
        {
            citiesCount = count;
            Cities = new int[citiesCount];
            for (int i = 0; i < citiesCount; i++)
                Cities[i] = -1;
        }

        public int this[int n]
        {
            get
            {
                return Cities[n];
            }
            set
            {
                Cities[n] = value;
            }
        }

        public int CitiesCount
        {
            get
            {
                return this.citiesCount;
            }
        }

        public void Show()
        {
            for (int i = 0; i < citiesCount; i++)
                System.Console.Write("{0} ", Cities[i]);
            System.Console.Write("{0}", Cities[0]);
        }

        public bool Equals(Path path)
        {
            for (int i = 0; i < CitiesCount; i++)
                if (this[i] != path[i])
                    return false;
            return true;
        }

        public Path(Path p)
        {
            this.citiesCount = p.citiesCount;
            Cities = new int[this.citiesCount];
            for (int i = 0; i < citiesCount; i++)
                this[i] = p[i];
        }
    }
}
