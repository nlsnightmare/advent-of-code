var input = File.ReadLines("./input.txt");

var patterns = input.First().Split(',').Select(p => p.Trim()).ToList();
List<string> towels = input.Skip(2).ToList();

// Part 1
long totalPossible = towels.Where(t => IsPossible(t, patterns)).Count();
Console.WriteLine($"There are {totalPossible} combinations");

// Part 2
var cache = new Dictionary<string, long>();
totalPossible = towels.Select(t => PossibleCombinations(t, patterns, cache)).Sum();
Console.WriteLine($"There are {totalPossible} combinations");

bool IsPossible(string towel, IEnumerable<string> patterns)
{
    if (towel.Length == 0)
        return true;

    foreach (var pattern in patterns)
    {
        if (towel.StartsWith(pattern) && IsPossible(towel.Substring(pattern.Length), patterns))
        {
            return true;
        }
    }
    return false;
}

long PossibleCombinations(string towel, IEnumerable<string> patterns, Dictionary<string, long> cache)
{
    if (towel.Length == 0)
        return 1;

    var sum = 0L;

    if (cache.ContainsKey(towel))
    {
        return cache[towel];
    }

    foreach (var pattern in patterns)
    {
        if (towel.StartsWith(pattern))
        {
            var value = PossibleCombinations(towel.Substring(pattern.Length), patterns, cache);
            sum += value;
        }
    }

    cache.Add(towel, sum);
    return sum;
}
