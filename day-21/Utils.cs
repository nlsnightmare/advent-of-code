global using Keypad = System.Collections.Generic.Dictionary<char, Vec2>;
using System.Text.RegularExpressions;

public class Utils
{
    public static Keypad Numpad = new()
    {
        { '7', new(0, 0) },
        { '8', new(1, 0) },
        { '9', new(2, 0) },
        { '4', new(0, 1) },
        { '5', new(1, 1) },
        { '6', new(2, 1) },
        { '1', new(0, 2) },
        { '2', new(1, 2) },
        { '3', new(2, 2) },
        { ' ', new(0, 3) },
        { '0', new(1, 3) },
        { 'A', new(2, 3) },
    };

    public static Keypad Arrows = new()
    {
        { ' ', new(0, 0) },
        { '^', new(1, 0) },
        { 'A', new(2, 0) },
        { '<', new(0, 1) },
        { 'v', new(1, 1) },
        { '>', new(2, 1) },
    };

    public static int Complexity(string steps, string code) =>
        int.Parse(new Regex(@"(\d+)").Match(code).Value) * steps.Count();
}


public record TypeResult(List<char> steps, char position)
{
    public string Value => string.Join("", steps);
}

