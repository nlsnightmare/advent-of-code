void Swap(ref List<int> update, (int, int) indexes)
{
    (int left, int right) = indexes;
    int leftvalue = update[left];
    int rightValue = update[right];

    update[left] = rightValue;
    update[right] = leftvalue;
}

var rules = new List<Rule>();
var updates = new List<List<int>>();

var lines = File.ReadLines("./inputs/input.txt");
foreach (var line in lines)
{
    if (line.Contains('|'))
    {
        var split = line.Split("|").Select(int.Parse);
        rules.Add(new Rule(split.First(), split.Last()));
    }
    else if (line.Contains(','))
    {
        var split = line.Split(",").Select(int.Parse);
        updates.Add(split.ToList());
    }
}

Console.WriteLine($"Found {rules.Count()} rules, {updates.Count()} updates");
var valid =
    from update in updates
    where rules.All(r => r.Matches(update) == null)
    select update[update.Count() / 2];

Console.WriteLine(string.Join(",", valid));

var invalid = (
    from update in updates
    where rules.All(r => r.Matches(update) == null) == false
    select update
).ToList();

Console.WriteLine($"Sum: {valid.Sum()}");

Console.WriteLine("------------------------");

for (int i = 0; i < invalid.Count(); i++)
{
    var update = invalid[i];
    bool hasFailed;
    do
    {
        hasFailed = false;
        foreach (var rule in rules)
        {
            var match = rule.Matches(update);
            if (match is null)
                continue;

            hasFailed = true;
            Swap(ref update, match.Value);
        }
    } while (hasFailed);
}

var mid = invalid.Select(list => list[list.Count() / 2]).Sum();
Console.WriteLine($"Sum 2: {mid}");

record class Rule(int left, int right)
{
    public (int, int)? Matches(List<int> update)
    {
        int leftIndex = update.IndexOf(left);
        int rightIndex = update.IndexOf(right);
        if (leftIndex == -1 || rightIndex == -1)
            return null;

        if (leftIndex < rightIndex)
            return null;

        return (leftIndex, rightIndex);
    }
}
