using System;
using System.IO;
using System.Linq;
namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //DayOne();
            DayTwo();
        }
        private static void DayOne()
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
        private static void DayTwo()
        {
            var intCode = "1,12,2,3,1,1,2,3,1,3,4,3,1,5,0,3,2,1,10,19,2,9,19,23,1,9,23,27,2," +
                "27,9,31,1,31,5,35,2,35,9,39,1,39,10,43,2,43,13,47,1,47,6,51,2,51,10,55,1,9," +
                "55,59,2,6,59,63,1,63,6,67,1,67,10,71,1,71,10,75,2,9,75,79,1,5,79,83,2,9,83,87" +
                ",1,87,9,91,2,91,13,95,1,95,9,99,1,99,6,103,2,103,6,107,1,107,5,111,1,13,111,115" +
                ",2,115,6,119,1,119,5,123,1,2,123,127,1,6,127,0,99,2,14,0,0";
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    int[] opArray = intCode.Trim().Split(',').Select(x => int.Parse(x)).ToArray();
                    opArray[1] = noun;
                    opArray[2] = verb;
                    var finished = false;
                    var pc = 0;
                    while (!finished)
                    {
                        switch (opArray[pc])
                        {
                            case 1:
                                var addend1Address = opArray[pc + 1];
                                var addend2Address = opArray[pc + 2];
                                var sumAddress = opArray[pc + 3];
                                opArray[sumAddress] = opArray[addend1Address] + opArray[addend2Address];
                                pc += 4;
                                break;
                            case 2:
                                var factor1Address = opArray[pc + 1];
                                var factor2Address = opArray[pc + 2];
                                var productAddress = opArray[pc + 3];
                                opArray[productAddress] = opArray[factor1Address] * opArray[factor2Address];
                                pc += 4;
                                break;
                            case 99:
                                finished = true;
                                break;
                            default:
                                throw new Exception("Unknown opcode");
                        }
                    }
                    if(opArray[0] == 19690720)
                    {
                        printComputerOutput(opArray);
                        return;
                    }
                }
            }
        }
        private static void printComputerOutput(int[] opArray)
        {
            foreach (var op in opArray)
            {
                Console.Write($"{op}, ");
            }
            Console.ReadLine();
            return;
        }
    }
}
