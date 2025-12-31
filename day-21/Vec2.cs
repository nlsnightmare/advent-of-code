public record class Vec2(int x, int y)
{
    public List<Vec2> Neighbours() =>
        new List<Vec2> { new(x, y - 1), new(x, y + 1), new(x - 1, y), new(x + 1, y) };

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.x - b.x, a.y - b.y);

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

    public static Vec2 operator *(Vec2 a, int b) => new(a.x * b, a.y * b);

    public override string ToString() => $"Vec2({x},{y})";

    public static float Manhattan(Vec2 a, Vec2 b) =>
        Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

    internal static Vec2 FromChar(char pos) => pos switch {
        '>' => new(1, 0),
        '<' => new (-1, 0),
        'v' => new (0, 1),
        '^' => new (0, -1),
        _ => throw new ArgumentException(nameof(pos))
    };
    
}
