#if false
var lines = File.ReadLines("./input.txt").Select(line => line.Trim());

Console.WriteLine("### ATTEMPTING TO SOLVE PART 1 ###");
var totalScore = 0;
foreach (var line in lines)
{
    Console.WriteLine($"# Handling line {line}");

    var result = Part1(line);
    if (result is null)
    {
        Console.WriteLine($"Unable to solve line {line}");
        return;
    }

    int score = Utils.Complexity(result, line);
    totalScore += score;
    Console.WriteLine($"{line} -> {result.Length}");
}

Console.WriteLine($"Total score: {totalScore}");
#else

Console.WriteLine("### DEBUG SECTION ###");

var inputs = File.ReadLines("./example.txt")
.Select(line =>
        {
        var parts = line.Trim().Split(':');
        var actual = Part1(parts[0]);
        return new { input = parts[0], expected = parts[1].Trim(), actual = actual };
    });

var scores = inputs.Select(line => Utils.Complexity(line.actual, line.input));
var totalScore = scores.Sum();

var err = inputs.Where((a) => a.actual.Length != a.expected.Length);

foreach (var input in inputs) {
    Solver.DebugPrint(input.actual);
}
foreach (var e in err)
{
    Console.WriteLine($"Failed to correctly calculate score of {e.input}");

    var expected = e.expected;
    var actual = e.actual;
    for (int level = 0; level < 4; level++)
    {
        Console.WriteLine($"Testing in level {level}");

        string newExpected = Solver.Debug(expected, level);
        Console.WriteLine($"expected: {newExpected}");

        string newActual = Solver.Debug(actual, level);
        Console.WriteLine($"actual:   {newActual}");

        if (newExpected.Length != newActual.Length)
            Console.WriteLine("Discrepency detected!\n");
    }

} 
#endif

string Part1(string code) =>
new Keypad[] { Utils.Numpad, Utils.Arrows, Utils.Arrows }.Aggregate(code, (acc, keys) => Solver.Type(acc, keys).Value);
