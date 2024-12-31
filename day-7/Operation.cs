public enum Operation
{
	Addition,
	Multiplication,
	Concatenation
}

public static class OperationExtensions
{
	public static long Apply(this Operation op, long left, long right)
		=> op switch
		{
			Operation.Addition => left + right,
			Operation.Multiplication => left * right,
			Operation.Concatenation => long.Parse($"{left}{right}"),
			_ => throw new Exception($"Invalid op {op}")
		};

	public static int Apply(this Operation op, int left, int right)
		=> op switch
		{
			Operation.Addition => left + right,
			Operation.Multiplication => left * right,
			_ => throw new Exception($"Invalid op {op}")
		};

	public static IEnumerable<Operation> From(int number, int count)
	{
		for (int i = 0; i < count; i++)
		{
			var remainder = number % 3;
			if (remainder == 0) yield return Operation.Addition;
			if (remainder == 1) yield return Operation.Multiplication;
			if (remainder == 2) yield return Operation.Concatenation;
			number /= 3;
		}
	}

	public static IEnumerable<IEnumerable<Operation>> AllOptions(int count)
	{
		int max = (int)Math.Pow(3, count);
		return Enumerable.Range(0, max)
			.Select(num => From(num, count));
	}
}
