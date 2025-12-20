// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

var buttonRegex = new Regex(@"\d+");
var minMoves = File.ReadLines("./input/input.txt")
    .Where(line => string.IsNullOrEmpty(line) == false)
    .Chunk(3)
    .Select(lines =>
    {
        var matches = buttonRegex
            .Matches(lines[0])
            .Select(m => decimal.Parse(m.ToString()))
            .ToList();
        var buttonA = new Button(3, new Vec2(matches[0], matches[1]));

        matches = buttonRegex.Matches(lines[1]).Select(m => decimal.Parse(m.ToString())).ToList();
        var buttonB = new Button(1, new Vec2(matches[0], matches[1]));

        matches = buttonRegex.Matches(lines[2]).Select(m => decimal.Parse(m.ToString())).ToList();

        var machine = new ClawMachine { Prize = new(matches[0], matches[1]) };

        return machine.Solve(buttonA, buttonB);
    })
    .Where(moves => moves is not null)
    .Sum();

Console.WriteLine("Min Cost: " + minMoves);

var error = new Vec2(10000000000000m, 10000000000000m);
var minMoves2 = File.ReadLines("./input/input.txt")
    .Where(line => string.IsNullOrEmpty(line) == false)
    .Chunk(3)
    .Select(lines =>
    {
        var matches = buttonRegex
            .Matches(lines[0])
            .Select(m => decimal.Parse(m.ToString()))
            .ToList();
        var buttonA = new Button(3, new Vec2(matches[0], matches[1]));

        matches = buttonRegex.Matches(lines[1]).Select(m => decimal.Parse(m.ToString())).ToList();
        var buttonB = new Button(1, new Vec2(matches[0], matches[1]));

        matches = buttonRegex.Matches(lines[2]).Select(m => decimal.Parse(m.ToString())).ToList();

        var machine = new ClawMachine { Prize = new Vec2(matches[0], matches[1]) + error };

        return machine.Solve(buttonA, buttonB);
    })
    .Where(moves => moves is not null)
    .Sum();

Console.WriteLine("Updated Min Cost: " + minMoves2);
