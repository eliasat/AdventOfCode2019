using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //DayOne();
            //DayTwo();
            //DayThree();
            //DayFour()
            DayFive();
        }

        enum ParameterMode
        {
            Position,
            Immediate
        }
        enum Instruction
        {
            Add = 1,
            Mul = 2,
            Store = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            LessThan = 7,
            Equals = 8,
            Exit = 99
        }
        private static void DayFive()
        {
            var intCode = File.ReadAllText(@"C:\Users\Administrator\source\repos\AdventOfCode\AdventOfCode\Input\dayfive_input");
            int[] opArray = intCode.Trim().Split(',').Select(x => int.Parse(x)).ToArray();
            var finished = false;
            var pc = 0;
            while (!finished)
            {
                var opLength = opArray[pc].ToString().Length;
                int operation;
                var parameter1Mode = (int)ParameterMode.Position;
                var parameter2Mode = (int)ParameterMode.Position;
                var parameter3Mode = (int)ParameterMode.Position;
                if (opLength <= 2)
                {
                    operation = opArray[pc];
                }
                else if (opLength == 3)
                {
                    operation = int.Parse(opArray[pc].ToString().Substring(opLength - 2));
                    parameter1Mode = (int)ParameterMode.Immediate;
                }
                else if (opLength == 4)
                {
                    operation = int.Parse(opArray[pc].ToString().Substring(opLength - 2));
                    parameter1Mode = int.Parse(opArray[pc].ToString().Substring(opLength - 3, 1));
                    parameter2Mode = (int)ParameterMode.Immediate;
                }
                else
                {
                    operation = int.Parse(opArray[pc].ToString().Substring(opLength - 2));
                    parameter1Mode = int.Parse(opArray[pc].ToString().Substring(opLength - 3, 1));
                    parameter2Mode = int.Parse(opArray[pc].ToString().Substring(opLength - 4, 1));
                    parameter3Mode = (int)ParameterMode.Immediate;
                }
                switch (operation)
                {
                    case (int)Instruction.Add:
                        var addend1 = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var addend2 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        var sumAddress = opArray[pc + 3];
                        opArray[sumAddress] = addend1 + addend2;
                        pc += 4;
                        break;
                    case (int)Instruction.Mul:
                        var factor1 = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var factor2 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        var productAddress = opArray[pc + 3];
                        opArray[productAddress] = factor1 * factor2;
                        pc += 4;
                        break;
                    case (int)Instruction.Store:
                        Console.WriteLine("Input ID of the system:");
                        var input = int.Parse(Console.ReadLine().Trim());
                        var storeAddress = opArray[pc + 1];
                        opArray[storeAddress] = input;
                        pc += 2;
                        break;
                    case (int)Instruction.Output:
                        var output = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        Console.WriteLine("Output: " + output);
                        pc += 2;
                        break;
                    case (int)Instruction.JumpIfTrue:
                        var val = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var val1 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        pc = val != 0 ? val1 : pc + 3;
                        break;
                    case (int)Instruction.JumpIfFalse:
                        var val2 = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var val3 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        pc = val2 == 0 ? val3 : pc + 3;
                        break;
                    case (int)Instruction.LessThan:
                        var term1 = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var term2 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        var boolAddress = opArray[pc + 3];
                        opArray[boolAddress] = term1 < term2 ? 1 : 0;
                        pc += 4;
                        break;
                    case (int)Instruction.Equals:
                        var eq1 = parameter1Mode == (int)ParameterMode.Immediate ? opArray[pc + 1] : opArray[opArray[pc + 1]];
                        var eq2 = parameter2Mode == (int)ParameterMode.Immediate ? opArray[pc + 2] : opArray[opArray[pc + 2]];
                        var eqAddress = opArray[pc + 3];
                        opArray[eqAddress] = eq1 == eq2 ? 1 : 0;
                        pc += 4;
                        break;
                    case (int)Instruction.Exit:
                        finished = true;
                        break;
                    default:
                        throw new Exception("Unknown instruction");
                }
            }
        }
        private static void DayFour()
        {
            // 10^5
            // never decrease
            var ctr = 0;
            for (int i = 171309; i <= 643603; i++)
            {
                if (containsAdjacentDigit(i) && noDecreasingDigit(i))
                {
                    ctr++;
                }
            }
            Console.WriteLine(ctr);
        }
        private static bool noDecreasingDigit(int number)
        {
            var cArray = number.ToString().ToCharArray();
            var prevC = 10;
            foreach (var c in cArray)
            {
                if (c < prevC)
                {
                    return false;
                }
                prevC = c;
            }
            return true;
        }
        private static bool containsAdjacentDigit(int number)
        {
            var cArray = number.ToString().ToCharArray();
            foreach (var c in cArray)
            {
                var indeces = getAllIndexes(cArray, c);
                foreach (var index in indeces)
                {
                    if ((indeces.Contains(index - 1) && !indeces.Contains(index - 2) && !indeces.Contains(index + 1))
                        || (indeces.Contains(index + 1) && !indeces.Contains(index + 2) && !indeces.Contains(index - 1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static List<int> getAllIndexes(char[] arr, char c)
        {
            var result = new List<int>();
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i] == c)
                {
                    result.Add(i);
                }
            }
            return result;
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
            sum += mass / 3 - 2;
            return RecFuel(sum, mass / 3 - 2);
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
                    if (opArray[0] == 19690720)
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
        private static void DayThree()
        {
            List<Line> GetWirePath(string[] moves)
            {
                var prevCoor = new Coordinate(); ;
                var coord = new Coordinate();
                var wireCoordinates = new List<Line>();
                foreach (var move in moves)
                {
                    var moveLength = int.Parse(move.Substring(1));
                    switch (move[0])
                    {
                        case 'R':
                            coord.X += moveLength;
                            break;
                        case 'L':
                            coord.X -= moveLength;
                            break;
                        case 'U':
                            coord.Y += moveLength;
                            break;
                        case 'D':
                            coord.Y -= moveLength;
                            break;
                        default:
                            throw new Exception("Unknown movetyp");
                    }
                    var line = new Line(prevCoor, coord);
                    wireCoordinates.Add(line);
                    prevCoor.X = coord.X;
                    prevCoor.Y = coord.Y;
                }
                return wireCoordinates;
            }
            var input = File.ReadAllLines(@"C:\Users\Administrator\source\repos\AdventOfCode\AdventOfCode\Input\daythree_input");
            var wirePathA = GetWirePath(input[0].Split(','));
            var wirePathB = GetWirePath(input[1].Split(','));
            List<(int x, int y, int steps)> intersections = new List<(int x, int y, int steps)>();
            var lineAStepSum = 0;
            foreach (var lineA in wirePathA)
            {
                var lineBStepSum = 0;
                foreach (var lineB in wirePathB)
                {
                    var xIntersections = Range(lineA.Start.X, lineA.End.X).Intersect(Range(lineB.Start.X, lineB.End.X));
                    var yIntersections = Range(lineA.Start.Y, lineA.End.Y).Intersect(Range(lineB.Start.Y, lineB.End.Y));
                    if (xIntersections.Any() && yIntersections.Any())
                    {
                        (int x, int y, int steps) intersect = (xIntersections.First(), yIntersections.First(), lineAStepSum + lineBStepSum);
                        if (lineA.Start.X == lineA.End.X)
                        {
                            intersect.steps += Math.Abs(yIntersections.First() - lineA.Start.Y);
                            intersect.steps += Math.Abs(xIntersections.First() - lineB.Start.X);
                        }
                        else
                        {
                            intersect.steps += Math.Abs(xIntersections.First() - lineA.Start.X);
                            intersect.steps += Math.Abs(yIntersections.First() - lineB.Start.Y);
                        }
                        intersections.Add(intersect);
                    }
                    lineBStepSum += Math.Abs(lineB.End.X - lineB.Start.X) + Math.Abs(lineB.End.Y - lineB.Start.Y);
                }
                lineAStepSum += Math.Abs(lineA.End.X - lineA.Start.X) + Math.Abs(lineA.End.Y - lineA.Start.Y);
            }
            intersections = intersections.OrderBy(coordinate => coordinate.steps).ToList();
            foreach (var inter in intersections)
            {
                Console.WriteLine($"({inter.x}, {inter.y}, {inter.steps})");
            }
        }
        private static IEnumerable<int> Range(int min, int max)
        {
            var dist = Math.Abs(max - min) + 1;
            if (max >= min)
            {
                var j = Enumerable.Range(min, dist);
                return j;
            }
            else
            {
                var a = Enumerable.Range(max, dist);
                return a;
            }
        }
        class Line
        {
            public Coordinate Start { get; set; } = new Coordinate();
            public Coordinate End { get; set; } = new Coordinate();
            public Line()
            {
                this.Start = new Coordinate();
                this.End = new Coordinate();
            }
            public Line(Coordinate start, Coordinate end)
            {
                this.Start = new Coordinate(start.X, start.Y);
                this.End = new Coordinate(end.X, end.Y);
            }
        }
    }
}
class Coordinate
{
    public int X { get; set; }
    public int Y { get; set; }
    public Coordinate()
    {
        this.X = 0;
        this.Y = 0;
    }
    public Coordinate(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
    public override string ToString()
    {
        return $"({this.X}, {this.Y})";
    }
}
