Part1();

void Part1()
{
    const int WIDTH = 71;
    const int HEIGHT = 71;
    const int BYTE_COUNT = 1024;

    var tiles = new List<List<Tile>>();
    for (int i = 0; i < HEIGHT; i++)
    {
        tiles.Add(Enumerable.Range(0, WIDTH).Select(t => Tile.Empty).ToList());
    }

    var bytePositions = File.ReadLines("./inputs/input.txt")
        .Select(line => line.Split(',').Select(int.Parse).ToArray())
        .Take(BYTE_COUNT)
        // .Where(l => l.Count() == 2)
        .Select(val => new Vec2(val[0], val[1]));

    foreach (var position in bytePositions)
        tiles[position.y][position.x] = Tile.Wall;

    var map = new Map(tiles);

    var start = new Vec2(0, 0);
    var end = new Vec2(WIDTH - 1, HEIGHT - 1);

    var minSteps = new Pathfinder().Dijkstra(start, end, map);
    Console.WriteLine($"The minimum number of steps is {minSteps}");
    var minSteps2 = new Pathfinder().AStar(start, end, map);
    Console.WriteLine($"A* Result: {minSteps2!.Nodes.Count()}");

}

void Part2()
{
    const int SIZE = 71;

    var byteStream = File.ReadLines("./inputs/input.txt")
        .Select(line => line.Split(',').Select(int.Parse).ToArray())
        .Where(l => l.Count() == 2)
        .Select(val => new Vec2(val[0], val[1]));

    var total = byteStream.Count();

    var bytes = total / 2;

    var start = new Vec2(0, 0);
    var end = new Vec2(SIZE - 1, SIZE - 1);

    var high = total;
    var low = 0;
    Console.WriteLine($"total: {high}");

    while (true)
    {
        var count = low + (high - low) / 2;
        Console.WriteLine($"Attempting {count}");
        var map = InitializeMap(byteStream.Take(count), SIZE);
        var solution = new Pathfinder().AStar(start, end, map);

        if (solution is null)
        {
            Console.WriteLine("solution not found!");
            high = count - 1;
        }
        else
        {
            Console.WriteLine("found solution!");
            low = count + 1;

            map = InitializeMap(byteStream.Take(count + 1), SIZE);
            solution = new Pathfinder().AStar(start, end, map);
            if (solution is null) {
                Console.WriteLine("found it!" + byteStream.Take(count + 1).Last());
                break;
            }
        }
    }
}

Map InitializeMap(IEnumerable<Vec2> bytes, int size)
{
    var tiles = new List<List<Tile>>();
    for (int i = 0; i < size; i++)
    {
        tiles.Add(Enumerable.Range(0, size).Select(t => Tile.Empty).ToList());
    }

    foreach (var position in bytes)
    {
        tiles[position.y][position.x] = Tile.Wall;
    }

    return new Map(tiles);
}
