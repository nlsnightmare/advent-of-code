public record struct Position(int x, int y)
{
	public static Position operator +(Position a, Position b) =>
		new(a.x + b.x, a.y + b.y);
}
