const bool DEBUG = false;
const int DELAY = 0;

var input = DEBUG ? "./input/example-2.txt" : "./input/input.txt";
var lines = File.ReadLines(input).Select(line => line.ToCharArray().ToList());

var tiles = lines
    .TakeWhile(line => line.FirstOrDefault() == '#')
    .Select(line =>
        line.Select(c =>
                c switch
                {
                    '#' => Tile.Wall,
                    '.' => Tile.None,
                    'O' => Tile.Barrel,
                    '[' => Tile.BarrelStart,
                    ']' => Tile.BarrelEnd,
                    '@' => Tile.Robot,
                    _ => throw new Exception("Invalid tile type"),
                }
            )
            .ToList()
    );

var map = new Map(tiles.ToList());
map.ScaleUp();

var instructions = lines
    .SkipWhile(l => l.Count() == 0 || l.First() == '#')
    .SelectMany(line => line)
    .Select(c =>
        c switch
        {
            '^' => new Vec2(0, -1),
            '>' => new Vec2(1, 0),
            'v' => new Vec2(0, 1),
            '<' => new Vec2(-1, 0),
            _ => throw new Exception($"Invalid instruction '{c}'"),
        }
    );

map.Print();
foreach (var i in instructions)
{
    map.Tick(i);
    if (DEBUG)
    {
        Thread.Sleep(DELAY);
        map.Print();
        Console.WriteLine();
    }
}
map.Print();
Console.WriteLine("Sum: " + map.GpsSum);


enum Tile
{
    None,
    Wall,
    Barrel,
    BarrelStart,
    BarrelEnd,
    Robot,
};
