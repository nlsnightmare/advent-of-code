public static class Solver
{
    public static void DebugPrint(string solution) {
        var solutions = Enumerable.Range(0, 4).Select(i => Debug(solution,i )).ToList();
        string prev = solutions[0];
        Console.WriteLine(prev);
        for (int i = 0; i < 3; i ++) {
            var gaps = prev.Split('A').Select(s => s.Count());

            var current = string.Join("",
                gaps.Take(solutions[i + 1].Length).Select((count, index) => new string(' ', count) + solutions[i + 1][index])
            );
            Console.WriteLine(current);
            prev = current;
        }
    }

    public static string Debug(string steps, int levels, bool throwException = true)
    {
        var keypads = new[] { Utils.Arrows, Utils.Arrows, Utils.Numpad }.Take(levels);
        foreach (var keypad in keypads)
        {
            string value = "";
            var pos = keypad['A'];
            for (int i = 0; i < steps.Count(); i++)
            {
                var step = steps[i];
                var dir = step switch
                {
                    '>' => new Vec2(1, 0),
                    '<' => new Vec2(-1, 0),
                    '^' => new Vec2(0, -1),
                    'v' => new Vec2(0, 1),
                    'A' => null,
                    _ => throw new Exception($"invalid step {step}")
                };

                // A key pressed
                if (dir is null)
                {
                    var key = keypad.First(p => p.Value == pos).Key;
                    value += key;
                    continue;
                }


                pos += dir;
                var currentKey = keypad.FirstOrDefault(p => p.Value == pos).Key;
                if (currentKey == ' ' && throwException)
                {
                    Console.WriteLine($"""
                    Invalid instruction '{step}'!
                    {steps}
                    {new string(' ', i)}^
                    """);
                    // throw new Exception();
                }
            }
            steps = value;
        }
        return steps;
    }

    public static TypeResult Type(string value, Keypad keys)
    {
        var current = keys['A'];
        var steps = new List<char>();
        var empty = keys[' '];
        bool isNumeric = keys.ContainsKey('9');

        foreach (var character in value.ToCharArray())
        {
            var target = keys[character];
            if (target is null)
                throw new Exception($"invalid character '{character}' detected!");

            var direction = target - current;

            var chunk = new List<char>();

            chunk.AddRange(Enumerable.Repeat(direction.x > 0 ? '>' : '<', Math.Abs(direction.x)));
            chunk.AddRange(Enumerable.Repeat(direction.y > 0 ? 'v' : '^', Math.Abs(direction.y)));

            // wait, do we cross the empty space?
            var simulated = current;
            foreach (var pos in chunk) {
                simulated += Vec2.FromChar(pos);
                var key = keys.SingleOrDefault(k => k.Value == simulated).Key;
                if (key == ' ') { 
                    var path = string.Join("", chunk);
                    var reversed = string.Join("", chunk.Reverse<char>());
                    // Console.WriteLine($"Found invalid path '{path}'");
                    // Console.WriteLine($"{path} -> {reversed}");
                    
                    chunk.Reverse();
                    break;
                    // throw new Exception($"found invalid path '{string.Join("", chunk)}'!");
                }
            }

            steps.AddRange(chunk);
            steps.Add('A');

            current = target;
        }
        return new(steps, keys.First(p => p.Value == current).Key);
    }
}
