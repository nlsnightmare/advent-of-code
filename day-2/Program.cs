
bool isValidListDampened(List<int> list)
{
	var index = IsValidList(list);
	if (index == -1) return true;

	var joined = string.Join(' ', list);


	var start = Math.Max(index - 2, 0);
	var end = Math.Min(index + 1, list.Count());
	/* for (int i = 0; i < list.Count(); i++) */
	for (int i = start; i < end; i++)
	{
		List<int> case1 = new List<int>(list);
		case1.RemoveAt(i);
		if (IsValidList(case1) == -1) return true;
	}
	// Console.WriteLine("Attempting to check");
	// Console.WriteLine($"original: {joined}");
	// Console.WriteLine($"case1   : " + string.Join(' ', case1));
	// Console.WriteLine($"case2   : " + string.Join(' ', case2));
	/* if (IsValidList(case2) == -1) return true; */
	return false;
}

int IsValidList(List<int> list)
{
	int? direction = null;

	for (int i = 1; i < list.Count(); i++)
	{
		var number = list[i];
		var previousNumber = list[i - 1];
		var difference = number - previousNumber;

		if (Math.Abs(difference) > 3 || difference == 0)
		{
			return i;
		}
		else if (i == 1)
		{
			direction = Math.Sign(difference);
		}
		else if (direction != Math.Sign(difference))
		{
			return i;
		}

	}

	return -1;
}

Console.WriteLine("Hello, baby <3");

// step 1. read the file line by line
// step 2. for each line
//		   read the numbers
var lines = File.ReadLines("./input.txt");
var lists = lines.Select(line => line.Split(' ')).Select(numbers => numbers.Select(int.Parse).ToList()).ToList();


var correct = lists.Where(isValidListDampened).Count();
Console.WriteLine($"Number of correct lists: {correct}");


/* var tests = new List<List<int>> { */
/* 	new List<int> { 7, 6, 4, 2, 1, }, */
/* 	new List<int> { 1, 2, 7, 8, 9, }, */
/* 	new List<int> { 9, 7, 6, 2, 1, }, */
/* 	new List<int> { 1, 3, 2, 4, 5, }, */
/* 	new List<int> { 8, 6, 4, 4, 1, }, */
/* 	new List<int> { 1, 3, 6, 7, 9, }, */
/* }; ; */

/* var correct = tests.Where(list => isValidListDampened(list)) */
/* 	.Select(l => string.Join(" ", l)); */
/* foreach (var l in correct) */
/* 	Console.WriteLine($"Number of correct lists: {l}"); */
