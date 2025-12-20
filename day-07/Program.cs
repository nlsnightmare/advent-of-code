using System.Text.RegularExpressions;

var lines = File.ReadLines("./input/input.txt");
/* var lines = File.ReadLines("./input/example.txt"); */
var regex = new Regex(@"(\d+): (.*)");

List<Equation> equations = new();

foreach (var line in lines)
{
	Console.WriteLine(line);
	var matches = regex.Match(line);
	var result = long.Parse(matches.Groups[1].Value);
	var numbers = matches.Groups[2].Value.Split(' ').Select(long.Parse);

	equations.Add(new()
	{
		Result = result,
		Numbers = numbers.ToList()
	});
}




decimal total = 0;
foreach (var eq in equations)
{
	var options = OperationExtensions.AllOptions(eq.Numbers.Count() - 1);
	if (options.Any(op => eq.Test(op.ToList())))
	{
		total += eq.Result;
	}

}
Console.WriteLine(total);

