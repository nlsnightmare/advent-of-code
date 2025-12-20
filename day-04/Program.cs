char? GetChar(int x, int y, List<List<char>> board)
{
	var line = board[y];
	if (line is null) return null;
	if (x >= line.Count()) return null;
	return line[x];
}


bool Traverse(List<List<char>> board, List<Position> offsets, Position start)
{
	var targetString = "XMAS";
	for (int i = 0; i < 3; i++)
	{
		var offset = offsets[i];
		var character = GetChar(start.X + offset.X, start.Y + offset.Y, board);
		if (character != targetString[i + 1])
		{
			return false;
		}
	}
	return true;
}

var board = File.ReadLines("./input.txt").Select(line => line.ToCharArray().ToList()).ToList();

var offsets = new List<Position> {
	new(0, 0),
	new(1, 0),
	new(2, 0),
	new(3, 0),
};




for (var y = 0; y < board.Count(); y++)
{
	Console.WriteLine($"entering line {y}");
	var line = board[y];
	for (var x = 0; x < line.Count(); x++)
	{
		var currentChar = GetChar(x, y, board);
		if (Traverse(board, offsets, new(x, y)))
		{
			Console.WriteLine($"found one on line {y}, pos {x}");
		}
	}
}


record class Position(int X, int Y);
