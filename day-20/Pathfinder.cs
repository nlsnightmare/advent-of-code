class Node
{
    public required Vec2 pos;
    public Node? parent;
    public float cost = 0;
    public float h = 0;
    public float f => cost + h;

    public IEnumerable<Node> Children() =>
        pos.Neighbours()
            .Where(n => n.x >= 0 && n.y >= 0)
            .Select(n => new Node { parent = this, pos = n });
}

class Pathfinder
{
    public int Dijkstra(Vec2 start, Vec2 goal, Map map)
    {
        var startNode = new Node { pos = start };
        var endNode = new Node { pos = goal };

        var nodeList = map.Positions().ToList();

        int vertices = map.Positions().Count();
        int[] distances = new int[vertices];
        bool[] shortestPathTreeSet = new bool[vertices];

        for (int i = 0; i < vertices; i++)
        {
            distances[i] = int.MaxValue;
            shortestPathTreeSet[i] = false;
        }

        int startNodeIndex = nodeList.IndexOf(startNode.pos);
        distances[startNodeIndex] = 0;

        for (int count = 0; count < vertices - 1; count++)
        {
            int u = MinimumDistance(distances, shortestPathTreeSet);
            shortestPathTreeSet[u] = true;

            foreach (var neighbor in nodeList[u].Neighbours())
            {
                if (map.Get(neighbor) == Tile.Wall)
                    continue;

                int v = nodeList.IndexOf(neighbor);
                int weight = 1;

                if (
                    !shortestPathTreeSet[v]
                    && distances[u] != int.MaxValue
                    && distances[u] + weight < distances[v]
                )
                {
                    distances[v] = distances[u] + weight;
                }
            }
        }

        return distances[nodeList.IndexOf(goal)];
    }

    private static int MinimumDistance(int[] distances, bool[] shortestPathTreeSet)
    {
        int min = int.MaxValue,
            minIndex = -1;

        for (int v = 0; v < distances.Length; v++)
        {
            if (!shortestPathTreeSet[v] && distances[v] <= min)
            {
                min = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    public Path? AStar(Vec2 src, Vec2 dst, Map map)
    {
        var start = new Node { pos = src };
        var goal = new Node { pos = dst };

        start.parent = null;

        List<Node> open = new() { start };
        List<Node> closed = new();

        while (open.Count() > 0)
        {
            var minF = open.Min(node => node.f);
            var q = open.First(node => node.f == minF);
            open.Remove(q);

            foreach (var successor in q.Children())
            {
                var tileFound = map.Get(successor.pos);
                if (tileFound == Tile.Wall)
                    continue;

                // successor.cost = q.cost + Manhattan(q, successor, map);
                // successor.h = Manhattan(goal, successor, map);

                successor.cost = q.cost + Euclidean(q, successor, map);
                successor.h = Euclidean(goal, successor, map);

                var path = new List<Vec2>();
                if (successor.pos == goal.pos)
                {
                    var curr = successor;
                    while (curr is not null)
                    {
                        path.Add(curr.pos);
                        curr = curr.parent;
                    }

                    path.Reverse();
                    return new Path(path, path.Last()!, path.First()!, path.Count());
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

    private float Manhattan(Node a, Node b, Map map) =>
        Math.Abs(a.pos.x - b.pos.x) + Math.Abs(a.pos.y - a.pos.y);

    private float Euclidean(Node a, Node b, Map map) =>
        (float)(Math.Pow(a.pos.x - b.pos.x, 2) + Math.Pow(a.pos.y - a.pos.y, 2));
}
