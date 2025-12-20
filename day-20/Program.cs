var tiles = File.ReadLines("./inputs/input.txt")
    .Select(line =>
        line.ToCharArray()
            .Select(c =>
                c switch
                {
                    '.' => Tile.Empty,
                    '#' => Tile.Wall,
                    'S' => Tile.Start,
                    'E' => Tile.End,
                    _ => Tile.None,
                }
            )
    );
var map = new Map(tiles);
var start = map.Positions().Where(p => map.Get(p) == Tile.Start).First();
var end = map.Positions().Where(p => map.Get(p) == Tile.End).First();
var path = new Pathfinder().AStar(start, end, map)?.Nodes;

if (path is null)
{
    Console.WriteLine("Unable to find path!");
    return;
}

const int MIN_TIME_SAVED = 100;
const int MAX_SKIP = 20;
Dictionary<int, int> Skips = new();
for (int i = 0; i < path.Count() - 1; i++)
{
    var first = path[i];
    for (int j = i + MIN_TIME_SAVED; j < path.Count(); j++)
    {
        var destination = path[j];
        var distance = destination.Distance(first);
        var timeGained = (j - i) - distance;

        if (distance > MAX_SKIP)
            continue;


        var totalSkips = Skips.GetValueOrDefault(timeGained);
        Skips[timeGained] = totalSkips + 1;
    }
}

foreach (var skip in Skips.Where(p => p.Key >= MIN_TIME_SAVED))
{
    Console.WriteLine($"There are {skip.Value} skips saving {skip.Key} picoseconds");
}

var solution = Skips.Where(p => p.Key >= MIN_TIME_SAVED)
    .Select(p => p.Value)
    .Sum();
    Console.WriteLine($"Solution: {solution}");
    
// Console.WriteLine($"There are {totalSavingAtLeast100} skips saving >= 100picoseconds");
