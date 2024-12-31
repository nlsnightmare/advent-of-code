public class Guard
{
    public Position Position { get; set; }
    public Position Direction { get; set; }

    public Guard(Position position, Position direction)
    {
        Position = position;
        Direction = direction;
    }

    public void TurnRight()
    {
        Direction = Direction switch
        {
            { x: 1, y: 0 } => new(0, 1),
            { x: -1, y: 0 } => new(0, -1),

            { x: 0, y: 1 } => new(-1, 0),
            { x: 0, y: -1 } => new(1, 0),
            _ => throw new Exception("invalid direction"),
        };
    }


    public void Advance() {
        Position = Position + Direction;

    }

    public override string ToString()
    {
        return $"Guard(Pos={Position}, Dir={Direction})";
    }
}
