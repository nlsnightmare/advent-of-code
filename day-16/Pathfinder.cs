class Node
{
    public required Vec2 pos;
    public Node? parent;
    public float cost = 0;
    public float h = 0;
    public float f => cost + h;

    public IEnumerable<Node> Children() =>
        pos.Neighbours().Select(n => new Node { parent = this, pos = n });
}

class Pathfinder
{
    public Path? Search(
        Node start,
        Node goal,
        Map map,
        List<Vec2>? banned = null,
        float? maxCost = null
    )
    {
        // start.parent = null;
        banned ??= new List<Vec2>();

        List<Node> open = new() { start };
        List<Node> closed = new();

        while (open.Count() > 0)
        {
            var minF = open.Min(node => node.f);
            var q = open.First(node => node.f == minF);
            open.Remove(q);

            // Make sure that we do not exceed the maxCost
            if (maxCost is not null && q.cost > maxCost)
            {
                closed.Add(q);
                continue;
            }

            foreach (var successor in q.Children())
            {
                if (map.Get(successor.pos) == Tile.Wall || banned.Contains(successor.pos))
                    continue;

                successor.cost = q.cost + Distance(q, successor, map);
                successor.h = Manhattan(goal, successor, map);

                var path = new List<Node>();
                if (successor.pos == goal.pos)
                {
                    var curr = successor;
                    while (curr is not null)
                    {
                        path.Add(curr);
                        curr = curr.parent;
                    }

                    var pathCost = path.First().cost;
                    if (maxCost != null && pathCost > maxCost)
                        return null;
                    return new Path(path, path.Last()!, path.First()!, path.First()!.cost);
                }

                var betterInOpen = open.Find(n => n.pos == successor.pos && n.f <= successor.f);

                if (betterInOpen is not null)
                    continue;

                var betterInClosed = closed.Find(n => n.pos == successor.pos && n.f <= successor.f);
                if (betterInClosed is not null)
                    continue;

                open.Add(successor);
            }
            closed.Add(q);
        }

        return null;
    }

    private int Distance(Node a, Node b, Map map)
    {
        var direction = a.parent is not null ? a.parent.pos - a.pos : new Vec2(-1, 0);
        var endDirection = a.pos - b.pos;

        if (direction.TurnRight() == endDirection)
        {
            return 1001;
        }
        else if (direction.TurnLeft() == endDirection)
        {
            return 1001;
        }
        else if (direction == endDirection)
        {
            return 1;
        }
        else
        {
            return 2001;
        }
    }

    private float Manhattan(Node a, Node b, Map map) =>
        Math.Abs(a.pos.x - b.pos.x) + Math.Abs(a.pos.y - a.pos.y);
}
