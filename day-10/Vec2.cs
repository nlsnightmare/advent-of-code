record class Vec2(int x, int y)
{
    public List<Vec2> Neighbours() =>
        new List<Vec2> { new(x - 1, y), new(x + 1, y), new(x, y - 1), new(x, y + 1) };
}
