record class Vec2(decimal x, decimal y)
{
    public List<Vec2> Neighbours() =>
        new List<Vec2> { new(x - 1, y), new(x + 1, y), new(x, y - 1), new(x, y + 1) };

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.x - b.x, a.y - b.y);

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

    public static Vec2 operator *(Vec2 a, int b) => new(a.x * b, a.y * b);

    public Vec2 TurnRight() =>
        this switch
        {
            { x: 1, y: 0 } => new(0, 1),
            { x: -1, y: 0 } => new(0, -1),

            { x: 0, y: 1 } => new(-1, 0),
            { x: 0, y: -1 } => new(1, 0),
            _ => throw new Exception("invalid direction"),
        };

    public List<Vec2> Vertices()
    {
        return new List<Vec2>
        {
            new(x - 0.5m, y - 0.5m),
            new(x - 0.5m, y + 0.5m),
            new(x + 0.5m, y - 0.5m),
            new(x + 0.5m, y + 0.5m),
        };
    }

    public static bool AreNeighbours(List<Vec2> points)
    {
        for (int i = 0; i < points.Count(); i++)
        {
            for (int j = i + 1; j < points.Count(); j++)
            {
                var p1 = points[i];
                var p2 = points[j];
                if (!AreNeighbours(p1, p2))
                    return false;
            }
        }
        return true;
    }

    public static bool AreNeighbours(Vec2 p1, Vec2 p2) => p1.Neighbours().Contains(p2);
}
