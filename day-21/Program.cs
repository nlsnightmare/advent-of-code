// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Keypad = System.Collections.Generic.Dictionary<char, Vec2>;

// Console.WriteLine($"Answer: {answer}");

Keypad numpad = new()
{
    { '7', new(0, 0) },
    { '8', new(1, 0) },
    { '9', new(2, 0) },
    { '4', new(0, 1) },
    { '5', new(1, 1) },
    { '6', new(2, 1) },
    { '1', new(0, 2) },
    { '2', new(1, 2) },
    { '3', new(2, 2) },
    { ' ', new(0, 3) },
    { '0', new(1, 3) },
    { 'A', new(2, 3) },
};

Keypad arrows = new()
{
    { ' ', new(0, 0) },
    { '^', new(1, 0) },
    { 'A', new(2, 0) },
    { '<', new(0, 1) },
    { 'v', new(1, 1) },
    { '>', new(2, 1) },
};

#if false
var lines = File.ReadLines("./input.txt").Select(line => line.Trim());
#else
var lines = File.ReadLines("./example.txt")
    .Select(line => line.Trim())
    .Where(t => t.StartsWith("379"));
#endif

var score = 0;
foreach (var line in lines)
{
    var result = Part1(line);
    if (result is null)
        continue;
    Console.WriteLine($"{line} -> {result.Count()} {result}");
    if (Debug(result, 3) != line)
    {
        Console.WriteLine("it broke");
    }
    Console.WriteLine(Debug(result, 0));
    Console.WriteLine(Debug(result, 1));
    Console.WriteLine(Debug(result, 2));
    Console.WriteLine(Debug(result, 3));

    score += Complexity(result, line);
}

Console.WriteLine($"Total score: {score}");

string Part1(string code, bool debug = false) =>
    new Keypad[] { numpad, arrows, arrows }.Aggregate(code, (acc, keys) => Type(acc, keys).Value);

int Complexity(string steps, string code) =>
    int.Parse(new Regex(@"(\d+)").Match(code).Value) * steps.Count();

string Debug(string steps, int levels)
{
    var keypads = new[] { arrows, arrows, numpad }.Take(levels);
    foreach (var keypad in keypads)
    {
        string value = "";
        var pos = keypad['A'];
        for (int i = 0; i < steps.Count(); i++)
        {
            var step = steps[i];
            var dir = step switch
            {
                '>' => new Vec2(1, 0),
                '<' => new Vec2(-1, 0),
                '^' => new Vec2(0, -1),
                'v' => new Vec2(0, 1),
                _ => null,
            };

            if (dir is not null)
            {
                pos += dir;
                if (keypad.First(p => p.Value == pos).Key == ' ')
                {
                    // Console.WriteLine("invalid key detected :(" + string.Join("", steps.Take(i + 1)));
                }
            }
            else
            {
                var key = keypad.First(p => p.Value == pos).Key;
                value += key;
            }
        }
        steps = value;
    }
    return steps;
}

TypeResult Type(string value, Keypad keys)
{
    var current = keys['A'];

    var steps = new List<char>();
    var empty = keys[' '];
    foreach (var character in value.ToCharArray())
    {
        var target = keys[character];
        if (target is null)
            throw new Exception($"invalid character '{character}' detected!");

        var direction = target - current;

        var chunk = new List<char>();
        chunk.AddRange(Enumerable.Repeat(direction.y > 0 ? 'v' : '^', Math.Abs(direction.y)));
        chunk.AddRange(Enumerable.Repeat(direction.x > 0 ? '>' : '<', Math.Abs(direction.x)));

        steps.AddRange(Optimize(chunk));
        steps.Add('A');

        current = target;
    }
    return new(steps, keys.First(p => p.Value == current).Key);
}

List<char> Optimize(List<char> steps)
{
    List<char> result = steps
        .OrderByDescending(v => v) // group all similar together
        .OrderBy(step => Vec2.Manhattan(arrows['A'], arrows[step]))
        .ToList();

    var stringResult = string.Join("", result);
    var stringInitial = string.Join("", steps);
    if (stringInitial != stringResult)
    {
        Console.WriteLine(stringResult + "\n" + stringInitial);
    }

    return result;
}

record TypeResult(List<char> steps, char position)
{
    public string Value => string.Join("", steps);
}
