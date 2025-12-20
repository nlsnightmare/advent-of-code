record class Vec2(int x, int y)
{
    public List<Vec2> Neighbours() =>
        new List<Vec2> { new(x - 1, y), new(x + 1, y), new(x, y - 1), new(x, y + 1) };

    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.x - b.x, a.y - b.y);

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.x + b.x, a.y + b.y);

    public static Vec2 operator *(Vec2 a, int b) => new(a.x * b, a.y * b);

    public override string ToString() => $"Vec2({x},{y})";

    public Vec2 TurnRight() =>
        this switch
        {
            { x: 1, y: 0 } => new(0, 1),
            { x: -1, y: 0 } => new(0, -1),

            { x: 0, y: 1 } => new(-1, 0),
            { x: 0, y: -1 } => new(1, 0),
            _ => throw new Exception("invalid direction"),
        };

    public Vec2 TurnLeft() =>
        TurnRight().TurnRight().TurnRight();

}
