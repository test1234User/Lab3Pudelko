using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace PudelkoLibrary
{
    public enum UnitOfMeasure
    {
        milimeter,
        centimeter,
        meter
    }

    public sealed class Pudelko : IEquatable<Pudelko>, IFormattable, IEnumerable
    {
        private double a;
        private double b;
        private double c;
        public UnitOfMeasure Unit { get; private set; }
        public double Objetosc { get; private set; }
        public double Pole { get; private set; }
        public double A
        {
            get
            {
                return Math.Round(ConvertUnits(a, Unit), 3);
            }
        }
        public double B
        {
            get
            {
                return Math.Round(ConvertUnits(b, Unit), 3);
            }
        }
        public double C
        {
            get
            {
                return Math.Round(ConvertUnits(c, Unit), 3);
            }
        }

        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            if (a == null)
            {
                a = ConvertUnits(10, UnitOfMeasure.centimeter, unit);
            }
            if (b == null)
            {
                b = ConvertUnits(10, UnitOfMeasure.centimeter, unit);
            }
            if (c == null)
            {
                c = ConvertUnits(10, UnitOfMeasure.centimeter, unit);
            }

            if (unit == UnitOfMeasure.milimeter)
            {
                a = Math.Round((double)a, 0, MidpointRounding.ToZero);
                b = Math.Round((double)b, 0, MidpointRounding.ToZero);
                c = Math.Round((double)c, 0, MidpointRounding.ToZero);
            }
            else if (unit == UnitOfMeasure.centimeter)
            {
                a = Math.Round((double)a, 1, MidpointRounding.ToZero);
                b = Math.Round((double)b, 1, MidpointRounding.ToZero);
                c = Math.Round((double)c, 1, MidpointRounding.ToZero);
            }
            else
            {
                a = Math.Round((double)a, 3, MidpointRounding.ToZero);
                b = Math.Round((double)b, 3, MidpointRounding.ToZero);
                c = Math.Round((double)c, 3, MidpointRounding.ToZero);
            }

            double aMeters = ConvertUnits((double)a, unit);
            double bMeters = ConvertUnits((double)b, unit);
            double cMeters = ConvertUnits((double)c, unit);

            if (aMeters < 0.001 || bMeters < 0.001 || cMeters < 0.001 || aMeters > 10 || bMeters > 10 || cMeters > 10)
            {
                throw new ArgumentOutOfRangeException();
            }

            this.a = (double)a;
            this.b = (double)b;
            this.c = (double)c;
            Unit = unit;
            Objetosc = Math.Round(aMeters * bMeters * cMeters, 9);
            Pole = Math.Round((aMeters * bMeters * 2.0 + aMeters * cMeters * 2.0 + bMeters * cMeters * 2.0), 6);
        }

        public static double ConvertUnits(double x, UnitOfMeasure currentUnit, UnitOfMeasure newUnit = UnitOfMeasure.meter)
        {
            switch (currentUnit)
            {
                case UnitOfMeasure.milimeter:
                    if (newUnit == UnitOfMeasure.centimeter)
                    {
                        return x * 0.1;
                    }
                    else if (newUnit == UnitOfMeasure.meter)
                    {
                        return x * 0.001;
                    }
                    return x;
                case UnitOfMeasure.centimeter:
                    if (newUnit == UnitOfMeasure.milimeter)
                    {
                        return x * 10;
                    }
                    else if (newUnit == UnitOfMeasure.meter)
                    {
                        return x * 0.01;
                    }
                    return x;
                case UnitOfMeasure.meter:
                    if (newUnit == UnitOfMeasure.milimeter)
                    {
                        return x * 1000;
                    }
                    else if (newUnit == UnitOfMeasure.centimeter)
                    {
                        return x * 100;
                    }
                    return x;
                default:
                    return x;
            }
        }

        public string ToString(string? format = null, IFormatProvider formatProvider = null)
        {
            string doubleFormat;
            UnitOfMeasure newUnit;

            if (format == null)
            {
                format = "m";
            }
            
            if (format == "m")
            {
                newUnit = UnitOfMeasure.meter;
                doubleFormat = "F3";
            }
            else if (format == "cm")
            {
                newUnit = UnitOfMeasure.centimeter;
                doubleFormat = "F1";
            }
            else if(format == "mm")
            {
                newUnit = UnitOfMeasure.milimeter;
                doubleFormat = "F0";
            }
            else
            {
                throw new FormatException();
            }

            double convertedA = ConvertUnits(a, Unit, newUnit);
            double convertedB = ConvertUnits(b, Unit, newUnit);
            double convertedC = ConvertUnits(c, Unit, newUnit);

            return $"{convertedA.ToString(doubleFormat, formatProvider)} {format} × {convertedB.ToString(doubleFormat, formatProvider)} {format} × {convertedC.ToString(doubleFormat, formatProvider)} {format}";
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pudelko);
        }

        public bool Equals(Pudelko? obj)
        {
            if (obj is null)
            {
                return false;
            }

            bool equal;
            for (int i = 0; i < 3; i++)
            {
                equal = true;
                for (int j = 0; j < 3; j++)
                {
                    if (this[j] != obj[(j + i) % 3])
                    {
                        equal = false;
                        continue;
                    }
                }
                if(equal)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool operator ==(Pudelko lhs, Pudelko rhs)
        {
            if(lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }
                return false;
            }                      
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Pudelko lhs, Pudelko rhs)
        {
            return !(lhs == rhs);
        }
        public static Pudelko operator +(Pudelko lhs, Pudelko rhs)
        {
            double newA = ConvertUnits(lhs.A, lhs.Unit) + ConvertUnits(rhs.B, rhs.Unit);
            double newB = ConvertUnits(lhs.B, lhs.Unit) + ConvertUnits(rhs.B, rhs.Unit);
            double newC = ConvertUnits(lhs.C, lhs.Unit) + ConvertUnits(rhs.C, rhs.Unit);

            return new Pudelko(newA, newB, newC);
        }

        public static explicit operator double[](Pudelko pudelko)
        {
            return new double[] { pudelko.a, pudelko.b, pudelko.c };
        }

        public static implicit operator Pudelko(ValueTuple<int, int, int> values)
        {
            return new Pudelko(values.Item1, values.Item2, values.Item3, UnitOfMeasure.milimeter);
        }

        public double this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    case 2:
                        return c;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            yield return a;
            yield return b;
            yield return c;
        }        

        public static Pudelko Parse(string text)
        {
            string last2Chars = text.Substring(text.Length - 2, 2).TrimStart();
            UnitOfMeasure unit;

            if (last2Chars == "m")
            {
                unit = UnitOfMeasure.meter;
            }
            else if (last2Chars == "cm")
            {
                unit = UnitOfMeasure.centimeter;
            }
            else
            {
                unit = UnitOfMeasure.milimeter;
            }

            string[] lengths = text.Split($" {last2Chars} × ");
            double a = Convert.ToDouble(lengths[0]);
            double b = Convert.ToDouble(lengths[1]);
            double c = Convert.ToDouble(lengths[2]);

            return new Pudelko(a, b, c, unit);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(a, b, c, Unit);
        }
    }
}
