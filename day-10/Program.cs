#if true
var map = new Map("./inputs/input.txt");
#else
var map = new Map("./inputs/example.txt");
#endif 

Console.WriteLine($"Solution: {map.PossiblePaths(distinct: true)}");



