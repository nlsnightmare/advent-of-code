class Map
{
    public List<List<char>> Tiles;

    public Map(string fileName)
    {
        Tiles = File.ReadLines(fileName).Select(line => line.ToCharArray().ToList()).ToList();
    }

    public List<Plot> Plots()
    {
        var plots = new List<Plot>();

        foreach (var position in Positions())
        {
            // Only there to fix stupid null error
            char c = Get(position) ?? '/';

            var existingPlot = plots.Where(p => p.Tiles.Contains(position)).Count();
            if (existingPlot > 0)
                continue;

            var plot = new Plot { Char = c };
            plot.Tiles.Add(position);

            var neighbours = new List<Vec2> { position };
            do
            {
                neighbours = neighbours
                    .SelectMany(pos => pos.Neighbours())
                    .Distinct()
                    .Where(n => Get(n) == plot.Char)
                    .Where(n => plot.Tiles.Contains(n) == false)
                    .ToList();

                foreach (var n in neighbours)
                    plot.Tiles.Add(n);
            } while (neighbours.Count() > 0);

            plots.Add(plot);
        }

        return plots;
    }

    private char? Get(Vec2 position) =>
        Tiles.ElementAtOrDefault((int)position.y)?.ElementAtOrDefault((int)position.x);

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
