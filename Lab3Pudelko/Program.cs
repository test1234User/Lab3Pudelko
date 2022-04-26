using P = PudelkoLibrary.Pudelko;

namespace PudelkoLibrary
{
    public class Kompresor
    {
        public static P Kompresuj(P pudelko)
        {
            double a = P.ConvertUnits(pudelko.A, UnitOfMeasure.meter, pudelko.Unit);
            double b = P.ConvertUnits(pudelko.B, UnitOfMeasure.meter, pudelko.Unit);
            double c = P.ConvertUnits(pudelko.C, UnitOfMeasure.meter, pudelko.Unit);
            double d = Math.Cbrt(a * b * c);
            return new P(d, d, d, pudelko.Unit);
        }
    }

    public class Program
    {
        static int KryteriumPorwnawczePudelek(P lhs, P rhs)
        {
            if(lhs.Objetosc < rhs.Objetosc)
            {
                return -1;
            }
            else if(lhs.Objetosc > rhs.Objetosc)
            {
                return 1;
            }

            if(lhs.Pole < rhs.Pole)
            {
                return -1;
            }
            if (lhs.Pole > rhs.Pole)
            {
                return 1;
            }

            double sumaLhs = 0, sumaRhs = 0;
            for (int i = 0; i < 3; i++)
            {
                sumaLhs += lhs[i];
                sumaRhs += rhs[i];
            }
            if (sumaLhs < sumaRhs)
            {
                return -1;
            }
            if(sumaLhs > sumaRhs)
            {
                return 1;
            }
            return 0;
        }

        public static void Main()
        {
            Comparison<P> kryterium = new Comparison<P>(KryteriumPorwnawczePudelek);

            P przedKompresja = new P(c: 2500, unit: UnitOfMeasure.milimeter);
            P poKompresji = Kompresor.Kompresuj(przedKompresja);

            List<P> list = new List<P>();
            list.Add(new P(1, 2, 3));
            list.Add(new P());
            list.Add(new P(5, 4, unit: UnitOfMeasure.centimeter));           
            list.Add(przedKompresja);
            list.Add(poKompresji);
            
            list.Sort(kryterium);
            
            foreach(P p in list)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\nWymiary pudełka przed kompresja:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"{przedKompresja[i]} ");
            }
            Console.WriteLine("\nWymiary pudełka po kompresji:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"{poKompresji[i]} ");
            }
            Console.WriteLine();

            P p1 = new P(1, 2, 3);
            P p2 = new P(2, 3, 1); 
            P p3 = new P(1, 2, 3);
            P p4 = new P(1, 1, 1);

            if(p1.Equals(p2))
            {
                Console.WriteLine("\nNastępujące 2 pudełka są równe:");
                Console.WriteLine(p1.ToString("mm"));
                Console.WriteLine(p2.ToString("mm"));
            }            
            if (p1 == p3)
            {
                Console.WriteLine("\nNastępujące 2 pudełka są równe:");
                Console.WriteLine(p1.ToString("cm"));
                Console.WriteLine(p3.ToString("cm"));
            }
            if (p1 != p4)
            {
                Console.WriteLine("\nNastępujące 2 pudełka są różne:");
                Console.WriteLine(p1.ToString("m"));
                Console.WriteLine(p4.ToString("m"));
            }
        }
    }
}