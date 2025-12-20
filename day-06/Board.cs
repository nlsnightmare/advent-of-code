public class Board
{
    private List<List<Tile>> _data { init; get; }
    public Guard Guard { get; private set; }

    public Board(List<List<Tile>> data)
    {
        _data = data;
        var guardPosition = Positions().First(pos => GetOrDefault(pos) == Tile.GUARD);
        Guard = new(guardPosition, new(0, -1));
        Set(Guard.Position, Tile.EMPTY);
    }

    public static Board FromFile(string file)
    {
        var lines = File.ReadLines(file)
            .Select(line => line.Select(Board.ParseTile).ToList())
            .ToList();

        return new Board(lines);
    }

    public bool GuardOutOfBounds =>
        Guard.Position.y < 0
        || Guard.Position.y >= _data.Count()
        || Guard.Position.x < 0
        || Guard.Position.x >= _data[0].Count();

    public bool GuardFacingExit()
    {
        var pos = Guard.Position;
        var direction = Guard.Direction;

        while (true)
        {
            Tile? nextTile = GetOrDefault(pos);
            if (nextTile == Tile.NONE || nextTile is null)
                return true;

            if (nextTile == Tile.WALL || nextTile == Tile.OBSTRUCTION)
                return false;

            pos += direction;
        }
    }

    public void Tick()
    {
        var nextPos = Guard.Position + Guard.Direction;
        var nextTile = GetOrDefault(nextPos, Tile.EMPTY);

        if (nextTile == Tile.WALL || nextTile == Tile.OBSTRUCTION)
            Guard.TurnRight();

        Guard.Advance();
    }

    public Tile? GetOrDefault(int x, int y, Tile? value = null) =>
        _data.ElementAtOrDefault(y)?.ElementAtOrDefault(x) ?? value;

    public Tile? GetOrDefault(Position pos, Tile? value = null) =>
        GetOrDefault(pos.x, pos.y, value);

    public void Set(Position pos, Tile tile)
    {
        if (pos.y < 0 || pos.x < 0)
            return;
        var found = _data.ElementAtOrDefault(pos.y)?.ElementAtOrDefault(pos.x);
        if (found is null)
            return;

        _data[pos.y][pos.x] = tile;
    }

    public IEnumerable<Position> Positions()
    {
        for (int y = 0; y < _data.Count(); y++)
        {
            for (int x = 0; x < _data[y].Count(); x++)
            {
                yield return new(x, y);
            }
        }
    }

    private static Tile ParseTile(char c) =>
        c switch
        {
            '.' => Tile.EMPTY,
            '#' => Tile.WALL,
            '^' => Tile.GUARD,
            _ => throw new Exception($"Invalid tile type {c}"),
        };

    public void Print()
    {
        string s = string.Join('\n', _data.Select(d => string.Join("", d)));
        Console.WriteLine(s);
    }
}
