class Robot
{
    public required Vec2 Position { get; set; }
    public required Vec2 Velocity { get; set; }

    public void Tick()
    {
        Position += Velocity;
    }

    public void Tick(int times)
    {
        Position += Velocity * times;
    }

    public void Constrain(Vec2 bounds)
    {
        Position = new(
            ((Position.x % bounds.x) + bounds.x) % bounds.x,
            ((Position.y % bounds.y) + bounds.y) % bounds.y
        );
    }
}
