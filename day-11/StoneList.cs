class StoneList
{
    public List<Stone> Stones { get; private set; }

    private record class SimulatedStone(Stone Stone, double Count);

    public StoneList(string fileName)
    {
        Stones = File.ReadAllText(fileName)
            .Split(" ")
            .Select(val => long.Parse(val))
            .Select(val => new Stone(val))
            .ToList();
    }

    public void Tick()
    {
        Stones = Stones.SelectMany(s => s.Tick()).ToList();
    }

    public double Simulate(int ticks)
    {
        var state = Stones.Select(s => new SimulatedStone(s, 1))!;
        do
        {
            var collection = new List<SimulatedStone>();
            foreach (var sim in state)
            {
                foreach (var stone in sim.Stone.Tick())
                {
                    collection.Add(new(stone, sim.Count));
                }
            }

            state = collection
                .GroupBy(s => s.Stone.Value)
                .Select(g => new SimulatedStone(g.First().Stone, g.Select(row => row.Count).Sum()));
        } while (--ticks > 0);
        return state.Sum(row => row.Count);
    }
}
