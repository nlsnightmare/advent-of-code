var tiles = File.ReadLines("./inputs/input.txt")
    // var tiles = File.ReadLines("./inputs/example.txt")
    .Select(line =>
        line.Select(c =>
            c switch
            {
                '#' => Tile.Wall,
                '.' => Tile.Empty,
                'S' => Tile.Start,
                'E' => Tile.End,
                _ => Tile.None,
            }
        )
    );

var map = new Map(tiles);

var start = map.Positions().First(p => map.Get(p) == Tile.Start);
var end = map.Positions().First(p => map.Get(p) == Tile.End);

// Find a single optimal solution
var optimalSolution = new Pathfinder().Search(
    new Node { pos = start },
    new Node { pos = end },
    map
);
if (optimalSolution is null)
{
    Console.WriteLine("Unable to find any optimal solution");
    return;
}

Console.WriteLine("Calculated Optimal Solution");

int found = 1;
HashSet<Vec2> visited = new();
for (int index = 1; index < optimalSolution.Nodes.Count(); index++)
{
    var current = optimalSolution.Nodes[index];
    var nextStep = optimalSolution.Nodes[index - 1];
    visited.Add(current.pos);

    var maxCost = optimalSolution.Cost - current.cost;
    Console.Clear();
    Console.WriteLine( $"Checking index {index}/{optimalSolution.Nodes.Count()}, maxCost: {maxCost}");

    var banned = new List<Vec2> { nextStep.pos };
    Path? otherSolution = null;

    while (true)
    {
        otherSolution = new Pathfinder().Search(
            new Node
            {
                pos = current.pos,
                parent = current.parent,
                cost = 0,
            },
            new Node { pos = end },
            map,
            banned: banned,
            maxCost: maxCost
        );

        if (otherSolution is null)
            break;

        found++;
        var changed = otherSolution.Nodes[2];
        otherSolution.Nodes.ForEach(n => visited.Add(n.pos));
        banned.Add(changed.pos);
    }
}

Console.WriteLine($"found {found} solutions! Total tiles: {visited.Count()} (expected 45)");

// map.Print(null, visited.ToList());
/* new Pathfinder().CountPaths(new Node { pos = start }, new Node { pos = end }, map); */
