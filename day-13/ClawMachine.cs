class ClawMachine
{
    public Vec2 Position { get; set; } = new(0, 0);
    public Vec2 Prize { get; set; } = new(0, 0);

    public void Press(Button button)
    {
        Position += button.Direction;
    }

    public decimal? Solve(Button A, Button B)
    {
        var m1 = MaxMoves(A, B);
        if (Math.Floor(m1) < m1)
            return null;

        var m2 = MaxMoves(B, A);

        return ((m1 * A.Cost) + (m2 * B.Cost));
    }

    private decimal MaxMoves(Button A, Button B)
    {
        decimal a1 = A.Direction.x;
        decimal a2 = A.Direction.y;

        decimal b1 = B.Direction.x;
        decimal b2 = B.Direction.y;

        decimal Y = Prize.y;
        decimal X = Prize.x;

        return (b1 * Y - b2 * X) / (a2 * b1 - a1 * b2);
    }
}
