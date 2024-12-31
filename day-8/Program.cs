// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
var dic = new Dictionary<char, List<Vec2>>();

// populate it somehow
record class Vec2(int x, int y)
{
	public static Vec2 operator -(Vec2 a, Vec2 b)
		=> new(a.x - b.x, a.y - b.y);

	public static Vec2 operator +(Vec2 a, Vec2 b)
		=> new(a.x + b.x, a.y + b.y);
}
class City
{
	private List<List<char>> _items;

}
