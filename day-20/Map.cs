class Map
{
    private List<List<Tile>> _tiles = new();

    public Map(IEnumerable<IEnumerable<Tile>> tiles)
    {
        _tiles = tiles.Select(t => t.ToList()).ToList();
    }

    public void Print(params Vec2[] cursors)
    {
        for (int y = 0; y < _tiles.Count(); y++)
        {
            for (int x = 0; x < _tiles[y].Count(); x++)
            {
                var pos = new Vec2(x, y);
                if (cursors.Contains(pos))
                {
                    Console.Write("*");
                    continue;
                }

                var tile = Get(pos);
                Console.Write(
                    tile switch
                    {
                        Tile.Wall => '#',
                        Tile.Start => 'S',
                        Tile.Empty => '.',
                        Tile.End => 'E',
                        _ => '?',
                    }
                );
            }
            Console.WriteLine();
        }
    }

    public void Print(Vec2? cursor = null, IEnumerable<Vec2>? path = null)
    {
        for (int y = 0; y < _tiles.Count(); y++)
        {
            for (int x = 0; x < _tiles[y].Count(); x++)
            {
                var pos = new Vec2(x, y);
                if (cursor == pos)
                {
                    Console.Write("*");
                    continue;
                }
                else if (path?.Contains(pos) == true)
                {
                    Console.Write("O");
                    continue;
                }

                var tile = Get(pos);
                Console.Write(
                    tile switch
                    {
                        Tile.Wall => ' ',
                        Tile.Start => 'S',
                        Tile.Empty => '.',
                        Tile.End => 'E',
                        _ => '?',
                    }
                );
            }
            Console.WriteLine();
        }
    }

    public Tile? Get(Vec2 position)
    {
        if (position.x < 0 || position.y < 0)
            return Tile.Wall;
        if (position.x >= _tiles[0].Count() || position.y >= _tiles.Count())
            return Tile.Wall;

        return _tiles.ElementAtOrDefault(position.y)?.ElementAtOrDefault(position.x);
    }

    public void Set(Vec2 position, Tile value) => _tiles[position.y][position.x] = value;

    public IEnumerable<Vec2> Positions()
    {
        for (int y = 0; y < _tiles.Count(); y++)
        {
            for (int x = 0; x < _tiles[y].Count(); x++)
            {
                yield return new Vec2(x, y);
            }
        }
    }
}
