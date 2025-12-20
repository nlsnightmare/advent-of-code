using System.Text.RegularExpressions;

var regex = new Regex(@"-?\d+");
var robots = File.ReadLines("./input/input.txt")
    .Select(line =>
    {
        var matches = regex.Matches(line).Select(m => int.Parse(m.ToString())).ToList();
        return new Robot
        {
            Position = new Vec2(matches[0], matches[1]),
            Velocity = new Vec2(matches[2], matches[3]),
        };
    })
    .ToList();

/* Vec2 bounds = new(11, 7); */
Vec2 bounds = new(101, 103);

int counter = 0;
while (true)
{
    var map = new Dictionary<Vec2, int>();
    counter++;
    foreach (var robot in robots)
    {
        robot.Tick();
        robot.Constrain(bounds);

        if (map.ContainsKey(robot.Position) == false)
        {
            map.Add(robot.Position, 1);
        }
    }

    Console.WriteLine(counter);
	if(map.Count() == robots.Count()) break;
}

var quadrants = new List<int>
{
    robots.Where(r => r.Position.x < (bounds.x / 2) && r.Position.y < (bounds.y / 2)).Count(),
    robots.Where(r => r.Position.x < (bounds.x / 2) && r.Position.y > (bounds.y / 2)).Count(),
    robots.Where(r => r.Position.x > (bounds.x / 2) && r.Position.y > (bounds.y / 2)).Count(),
    robots.Where(r => r.Position.x > (bounds.x / 2) && r.Position.y < (bounds.y / 2)).Count(),
};

var score = quadrants.Aggregate((int count, int aggr) => count * aggr);
/* Console.WriteLine(score); */
