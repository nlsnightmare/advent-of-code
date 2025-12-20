#if true
var list = new StoneList("./input/input.txt");
#else
var list = new StoneList("./input/example.txt");
#endif

Console.WriteLine($"Stones: {list.Simulate(75)}");
