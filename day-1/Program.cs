(List<int>, List<int>) ReadFile()
{
    var list1 = new List<int>();
    var list2 = new List<int>();

    foreach (var line in File.ReadLines("./input.txt"))
    {
        var parts = line.Split(' ');
        list1.Add(int.Parse(parts.First()));
        list2.Add(int.Parse(parts.Last()));
    }
    return (list1, list2);
}

var (list1, list2) = ReadFile();
list1.Sort();
list2.Sort();

var distance = list1.Zip(list2).Select((values) => Math.Abs(values.First - values.Second)).Sum();
Console.WriteLine($"Distance is {distance}");

var group = list2.GroupBy(value => value).ToDictionary(g => g.Key, g => g.Count());

var score = list1.Select(value => value * group.GetValueOrDefault(value, 0)).Sum();
Console.WriteLine($"Similarity Score: {score}");

