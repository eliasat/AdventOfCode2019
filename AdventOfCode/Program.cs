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
            DayThree();
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
                        (int x, int y, int steps) intersect = (xIntersections.First(), yIntersections.First(), lineAStepSum + lineBStepSum );
                        if(lineA.Start.X == lineA.End.X)
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
