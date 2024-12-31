public class Equation
{
	public long Result;
	public List<long> Numbers = new();

	public bool Test(List<Operation> operations)
	{
		long left = Numbers.First();
		for (int i = 1; i < Numbers.Count(); i++)
		{
			var op = operations[i - 1];
			var right = Numbers[i];
			left = op.Apply(left, right);
		}

		return left == Result;
	}
}
