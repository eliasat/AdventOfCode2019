using System;
using System.IO;
using System.Linq;
namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            dayOne();
        }
        private static void dayOne()
        {

            var moduleMasses = File.ReadAllLines(@"C:\Users\Administrator\source\repos\AdventOfCode\AdventOfCode\Input\input").Select(x => int.Parse(x));
            var sum = 0;
            foreach (var item in moduleMasses)
            {
                sum += item / 3 - 2;
                sum += RecFuel(0, item / 3 - 2);
            }
            Console.WriteLine(sum);
        }
        private static int RecFuel(int sum, int mass)
        {
            if (mass / 3 - 2 < 1)
            {
                return sum;
            }
            sum += mass/3-2;
            return RecFuel(sum, mass/3 - 2);
        }
    }
}
