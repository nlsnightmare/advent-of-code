class Map
{
    private List<List<Tile>> _tiles = new List<List<Tile>>();
    private Vec2 _robot;

    public Map(List<List<Tile>> tiles)
    {
        _tiles = tiles;
        _robot = Positions().First(p => Get(p) == Tile.Robot);
    }

    public void ScaleUp()
    {
        _tiles = _tiles
            .Select(l =>
                l.SelectMany(t =>
                        t switch
                        {
                            Tile.Barrel => new List<Tile> { Tile.BarrelStart, Tile.BarrelEnd },
                            Tile.Robot => new List<Tile> { Tile.Robot, Tile.None },
                            _ => new List<Tile> { t, t },
                        }
                    )
                    .ToList()
            )
            .ToList();

        _robot = Positions().First(p => Get(p) == Tile.Robot);
    }

    public void Tick(Vec2 instruction)
    {
        var next = _robot + instruction;
        var nextTile = Get(next);

        if (nextTile == Tile.Wall)
        {
            return;
        }
        else if (nextTile == Tile.None)
        {
            MoveRobot(next);
            return;
        }

        // simple case
        var tiles = new List<Vec2>();
        if (nextTile == Tile.Barrel)
        {
            while (nextTile == Tile.Barrel)
            {
                tiles.Add(next);
                next += instruction;
                nextTile = Get(next);
            }

            if (nextTile == Tile.None)
            {
                MoveBarrel(tiles.First(), next);
                MoveRobot(_robot + instruction);
            }
            return;
        }

        var barrels = GetBarrels(next, instruction);

        if (barrels is not null)
        {
            if (instruction == new Vec2(0, 1))
            {
                barrels = barrels.OrderBy(b => b.y);
            }
            else if (instruction == new Vec2(0, -1))
            {
                barrels = barrels.OrderByDescending(b => b.y);
            }

            foreach (var tile in barrels.AsEnumerable().Reverse())
            {
                MoveBarrel(tile, tile + instruction);
            }
            MoveRobot(_robot + instruction);
        }
    }

    public void Print()
    {
        /* Console.Clear(); */
        for (int y = 0; y < _tiles.Count(); y++)
        {
            for (int x = 0; x < _tiles[y].Count(); x++)
            {
                var tile = Get(new(x, y));
                Console.Write(
                    tile switch
                    {
                        Tile.Wall => '#',
                        Tile.Robot => '@',
                        Tile.None => '.',
                        Tile.Barrel => 'O',
                        Tile.BarrelStart => '[',
                        Tile.BarrelEnd => ']',
                        _ => '?',
                    }
                );
            }
            Console.WriteLine();
        }
    }

    public IEnumerable<Vec2>? GetBarrels(Vec2 start, Vec2 direction)
    {
        var next = start + direction;
        var startTile = Get(start);
        var nextTile = Get(next);

        if (direction.x != 0)
        {
            var tiles = new List<Vec2>();
            while (nextTile == Tile.BarrelStart || nextTile == Tile.BarrelEnd)
            {
                tiles.Add(next);
                next += direction;
                nextTile = Get(next);
            }

            if (Get(next) != Tile.Wall)
                return tiles;

            return null;
        }

        if (startTile == Tile.Wall)
            return null;
        if (startTile == Tile.None)
            return Enumerable.Empty<Vec2>();

        var add = startTile == Tile.BarrelStart ? new Vec2(1, 0) : new Vec2(-1, 0);
        var list = new List<Vec2> { start };

        var other = GetBarrels(next, direction);
        var other2 = GetBarrels(next + add, direction);
        if (other is null || other2 is null)
            return null;

        list.AddRange(other);
        list.AddRange(other2);

        return list;
    }

    public void MoveRobot(Vec2 pos)
    {
        Set(_robot, Tile.None);
        Set(pos, Tile.Robot);
        _robot = pos;
    }

    public void MoveBarrel(Vec2 from, Vec2 to)
    {
        if (Get(from) == Tile.Barrel)
        {
            Set(from, Tile.None);
            Set(to, Tile.Barrel);
        }
        else if (Get(from) == Tile.BarrelStart)
        {
            Set(from, Tile.None);
            Set(from + new Vec2(1, 0), Tile.None);

            Set(to, Tile.BarrelStart);
            Set(to + new Vec2(1, 0), Tile.BarrelEnd);
        }
        else if (Get(from) == Tile.BarrelEnd)
        {
            Set(from, Tile.None);
            Set(from + new Vec2(-1, 0), Tile.None);

            Set(to, Tile.BarrelEnd);
            Set(to + new Vec2(-1, 0), Tile.BarrelStart);
        }
    }

    public double GpsSum =>
        Positions()
            .Where(p => Get(p) == Tile.Barrel || Get(p) == Tile.BarrelStart)
            .Select(pos => (100 * pos.y) + pos.x)
            .Sum();

    private Tile? Get(Vec2 position) =>
        _tiles.ElementAtOrDefault(position.y)?.ElementAtOrDefault(position.x);

    private void Set(Vec2 position, Tile value) => _tiles[position.y][position.x] = value;

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
