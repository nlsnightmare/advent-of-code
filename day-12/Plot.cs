class Plot
{
    public required char Char { get; init; }
    public HashSet<Vec2> Tiles { get; init; } = new();

    public double Perimeter =>
        Tiles
            .Select(t => t.Neighbours().Where(n => Tiles.Contains(n)).Count())
            .Select(num => 4 - num)
            .Sum();

    public double Cost => Perimeter * Area;
    public double Area => Tiles.Count();
    public double UpdatedCost => Sides * Area;

    public double Sides => Tiles
            .SelectMany(p => p.Vertices())
            .GroupBy(p => p)
            .Select(group =>
            {
                var point = group.First();

                if (group.Count() % 2 == 1)
                    return 1;

                if (group.Count() != 2)
                    return 0;

                var tiles = point.Vertices().Where(v => Tiles.Contains(v)).ToList();
                return Vec2.AreNeighbours(tiles) ? 0 : 2;
            })
        .Sum();

    public void PrintPoints()
    {
        var points = Tiles
            .SelectMany(p => p.Vertices())
            .GroupBy(p => p)
            .Select(group =>
            {
                var point = group.First();

                if (group.Count() % 2 == 1)
                    return 1;

                if (group.Count() != 2)
                    return 0;

                var tiles = point.Vertices().Where(v => Tiles.Contains(v)).ToList();
                return Vec2.AreNeighbours(tiles) ? 0 : 2;
            })
        .Sum();
    }

    public void Print(Vec2? Cursor = null)
    {
        var maxX = Tiles.Max(t => t.x) + 4;
        var maxY = Tiles.Max(t => t.y) + 3;
        for (int y = -2; y < maxY; y++)
        {
            for (int x = -2; x < maxX; x++)
            {
                var pos = new Vec2(x, y);
                if (pos == Cursor)
                    Console.Write('X');
                else if (Tiles.Contains(pos))
                    Console.Write('*');
                else
                    Console.Write('.');
            }

            Console.WriteLine();
        }
    }

    public IEnumerable<Vec2> SideTiles =>
        Tiles.Where(t => t.Neighbours().Where(n => Tiles.Contains(n)).Count() != 4);
}
