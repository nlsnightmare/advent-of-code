class Map
{
    public List<List<int>> Tiles;

    public Map(string fileName)
    {
        Tiles = File.ReadLines(fileName)
            .Select(line => line.ToCharArray().AsEnumerable().Select((char c) => c - '0').ToList())
            .ToList();
    }

    public int PossiblePaths(bool distinct = false)
    {
        var total = 0;
        var starts = Positions().Where(pos => Get(pos) == 0);

        foreach (var pos in starts)
        {
            var next = 1;
            var positions = new List<Vec2> { pos };
            while (next < 10)
            {
                var iter = positions.SelectMany(p => p.Neighbours()).Where(n => Get(n) == next);
                if (distinct == false) {
                    iter = iter.Distinct();
                }

                positions = iter.ToList();
                next++;
            }

            /* Console.WriteLine($"For start {pos}"); */
            /* Console.WriteLine(string.Join(" ", positions.ToHashSet())); */

            total += positions.Count();
        }

        return total;
    }

    private int? Get(Vec2 position) =>
        Tiles.ElementAtOrDefault(position.y)?.ElementAtOrDefault(position.x);

    public IEnumerable<Vec2> Positions()
    {
        for (int y = 0; y < Tiles.Count(); y++)
        {
            for (int x = 0; x < Tiles[y].Count(); x++)
            {
                yield return new Vec2(x, y);
            }
        }
    }
}
