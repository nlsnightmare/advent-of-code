#if true
var map = new Map("./input/input.txt");
#else
var map = new Map("./input/example-2.txt");
#endif

var plots = map.Plots();
foreach (var p in plots)
{
	Console.WriteLine($"Plot: {p.Char} {p.Area} * {p.Sides} = {p.UpdatedCost}");
	p.PrintPoints();
}
Console.WriteLine("Total: " + plots.Sum(p => p.UpdatedCost));
