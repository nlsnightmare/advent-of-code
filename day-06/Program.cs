bool ProducesLoop(Board board, Position obsPosition)
{
    var currentTile = board.GetOrDefault(obsPosition);
    if (currentTile != Tile.EMPTY)
        return false;

    board.Set(obsPosition, Tile.OBSTRUCTION);

    var visited = new HashSet<(Position, Position)>();

    while (true)
    {
        var pair = (board.Guard.Position, board.Guard.Direction);

        if (visited.Contains(pair))
            return true;

        visited.Add(pair);

        if (board.GuardFacingExit() || board.GuardOutOfBounds)
            return false;

        board.Tick();
    }
}

var board = Board.FromFile("./inputs/input.txt");
var positions = new HashSet<Position>();

while (true)
{
    if (board.GuardOutOfBounds)
        break;

    positions.Add(board.Guard.Position);
    board.Tick();
}

Console.WriteLine($"Total visited: {positions.Count()}");

var count = 0;
foreach (var position in board.Positions())
{
	Console.WriteLine($"checking {position}");
	
    if (ProducesLoop(board, position))
    {
        count++;
    }

    // Reset board
    board = Board.FromFile("./inputs/input.txt");
}

Console.WriteLine($"Total count: {count}");

