Console.WriteLine("Hello, World!");


#if true
var map = new Map("./input/input.txt");
#else
var map = new Map("./input/example.txt");
#endif


var antinodes = map.Antinodes();

map.Print(antinodes);

Console.WriteLine($"Count: {antinodes.Count()}");

