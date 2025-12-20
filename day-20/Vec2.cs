record class Vec2(int x, int y)
{
    public List<Vec2> Neighbours() =>
        new List<Vec2> { new(x, y - 1), new(x, y + 1), new(x - 1, y), new(x + 1, y) };

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.x - b.x, a.y - b.y);

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

    public static Vec2 operator *(Vec2 a, int b) => new(a.x * b, a.y * b);

    public override string ToString() => $"Vec2({x},{y})";

    public int Distance(Vec2 other) => Math.Abs(this.x - other.x) + Math.Abs(this.y - other.y);

    public float Euclidean(Vec2 other) =>
        (float)Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));

    public Vec2 Direction() => new(Math.Sign(x), Math.Sign(y));

    public static bool IsSameLine(Vec2 a, Vec2 b) => a.x == b.x || a.y == b.y;
}
