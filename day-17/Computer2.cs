public class CachedComputer : Computer
{
    public HashSet<State> Cache = new();
    private static int hits = 0;
    private static int misses = 0;

    public bool Execute(bool debug = false)
    {
        if (debug)
            State.Inspect();

        while (State.IP < Program.Count())
        {
            var instruction = (Instruction)Program[State.IP];
            int operant = Program[State.IP + 1];

            if (Cache.Contains(State))
            {
                hits++;
                return false;
            }

            // Cache.Add(State);
            misses++;

            var newState = instruction switch
            {
                Instruction.ADV => Adv(State, operant),
                Instruction.BXL => Bxl(State, operant),
                Instruction.BST => Bst(State, operant),
                Instruction.BXC => Bxc(State, operant),
                Instruction.BDV => Bdv(State, operant),
                Instruction.CDV => Cdv(State, operant),

                // Instruction.OUT => Out(State, operant),
                // Instruction.JNZ => Jnz(State, operant),
                _ => State,
            };

            if (instruction == Instruction.JNZ)
            {
                newState = State.copyWith(IP: State.A == 0 ? State.IP : operant);
            }
            else if (instruction == Instruction.OUT)
            {
                var value = (int)(ComboOperant(operant) % 8);
                // var expected = Program[Output.Count()];
                Output.Add(value);
                if (debug)
                {
                    Console.WriteLine($"outputting {value}");
                }
            }

            if (newState.IP == State.IP)
                State = newState.copyWith(IP: State.IP + 2);
            else
                State = newState;

            if (debug)
            {
                Console.Write($"{instruction.ToString()}, {operant} | ");
                State.Inspect();
            }
        }
        return false;
    }

    public void CacheInfo()
    {
        var perc = (hits / (float)(hits + misses)) * 100;
        Console.WriteLine(
            $"hits:{hits}, misses: {misses} ({Math.Round(perc, 2)}%) ({Cache.Count()} entries)"
        );
    }
}
