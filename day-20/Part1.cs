public class Part1
{
    public void Execute()
    {
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

        Dictionary<int, int> Skips = new();
        for (int i = 0; i < path.Count() - 1; i++)
        {
            var first = path[i];
            var next = path[i + 1];
            for (int j = i + 1; j < path.Count(); j++)
            {
                var destination = path[j];

                if (Vec2.IsSameLine(first, destination) == false)
                    continue;

                var distance = destination.Distance(first);

                if (distance != 2 && distance != 3)
                    continue;

                var direction = (destination - first).Direction();

                if (!OnlyWallsBetween(first, destination, direction, map))
                    continue;

                var timeGained = (j - i) - 2;

                var totalSkips = Skips.GetValueOrDefault(timeGained);
                Skips[timeGained] = totalSkips + 1;
            }
        }

        var totalSavingAtLeast100 = Skips.Where(p => p.Key >= 100).Select(p => p.Value).Sum();
        Console.WriteLine($"There are {totalSavingAtLeast100} skips saving >= 100picoseconds");

    }

        bool OnlyWallsBetween(Vec2 start, Vec2 end, Vec2 direction, Map map)
        {
            Vec2 pos = start + direction;

            while (pos != end)
            {
                var tile = map.Get(pos);

                if (tile != Tile.Wall)
                    return false;

                pos += direction;
            }

            return true;
        }
}
