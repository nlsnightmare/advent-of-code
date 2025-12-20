using System.Linq;

public class Map
{
    public readonly List<List<char>> Data;
    public Dictionary<char, List<Vec2>> Antennas { get; private set; } = new();
    public Vec2 Bounds { get; private set; }

    /* public Dictionary<char, List<Vec2>> Antinodes { get; private set; } = new(); */

    public Map(string fileName)
    {
        Data = File.ReadLines(fileName).Select(line => line.ToCharArray().ToList()).ToList();

        Bounds = new(Data.First().Count(), Data.Count());

        foreach (Vec2 pos in Positions())
        {
            char value = Get(pos) ?? '.';

            if (value == '.')
                continue;
            if (value == '#')
                continue;

            List<Vec2>? list;
            if (Antennas.TryGetValue(value, out list))
            {
                list.Add(pos);
            }
            else
            {
                list = new();
                list.Add(pos);
                Antennas.Add(value, list);
            }
        }
    }

    public List<Vec2> Antinodes()
    {
        var antinodes = new HashSet<Vec2>();

        foreach (var c in Antennas.Keys)
        {
            var list = Antennas[c];
            var pairs = from p1 in list from p2 in list where p1 != p2 select new { p1, p2 };

            foreach (var pair in pairs)
            {
                var diff = pair.p1 - pair.p2;
                var point = diff + pair.p1;
                antinodes.Add(pair.p1);

                while (WithinBounds(point))
                {
                    antinodes.Add(point);
                    point += diff;
                }
            }
        }

        return antinodes.ToList();
    }

    public bool WithinBounds(Vec2 Point) =>
        Point.x >= 0 && Point.y >= 0 && Point.x < Bounds.x && Point.y < Bounds.y;

    public char? Get(Vec2 position) =>
        Data.ElementAtOrDefault(position.y)?.ElementAtOrDefault(position.x);

    public void Print(List<Vec2> antinodes)
    {
        for (int y = 0; y < Bounds.y; y++)
        {
            for (int x = 0; x < Bounds.x; x++)
            {
                var point = new Vec2(x, y);
                var c = Get(point);
                if (c == '#' && antinodes.Contains(point))
                {
                    Console.Write('#');
                }
                else if (c == '#')
                {
                    Console.Write('!');
                }
                else
                {
                    Console.Write(c);
                }
            }
            Console.WriteLine();
        }
    }

    public IEnumerable<Vec2> Positions()
    {
        for (int y = 0; y < Bounds.y; y++)
        {
            for (int x = 0; x < Bounds.x; x++)
            {
                yield return new Vec2(x, y);
            }
        }
    }
}
