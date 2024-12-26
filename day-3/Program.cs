using System.Text.RegularExpressions;


int Execute(string text) =>
	new Regex(@"mul\((\d+,\d+)\)")
	.Matches(text)
	.Select(match => match.Groups[1].Value)
	.Select(value => value
			.Split(',')
			.Select(int.Parse)
			.Aggregate((a, b) => a * b))
	.Sum();

var text = File.ReadAllText("./input.txt");
var doRegex = new Regex(@"(do\(\)|don't\(\))");
var segments = doRegex.Split(text);

var sum = 0;
var isActive = true;
foreach (var segment in segments)
{
	if (segment == "do()")
		isActive = true;
	else if (segment == "don't()")
		isActive = false;
	else if (isActive)
		sum += Execute(segment);

}
Console.WriteLine($"Sum: {sum}");

