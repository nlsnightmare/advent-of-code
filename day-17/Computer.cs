
public record State(ulong A = 0, ulong B = 0, ulong C = 0, int IP = 0)
{
    public State copyWith(ulong? A = null, ulong? B = null, ulong? C = null, int? IP = null) =>
        new State(A ?? this.A, B ?? this.B, C ?? this.C, IP ?? this.IP);

    public void Inspect(int b = 8, bool newLine = true)
    {
        // var base8 =
        //     $"State[A={Convert.ToString(A, 8)}, B={Convert.ToString(B, 8)}, C={Convert.ToString(C, 8)} / {IP}]";
        // var base10 =
        //     $"State[A={Convert.ToString(A, 10)}, B={Convert.ToString(B, 10)}, C={Convert.ToString(C, 10)} / {IP}]";
        // Console.Write($"State[A={Convert.ToString(A, b).PadLeft(8, '0')}, B={Convert.ToString(B, b).PadLeft(8, '0')}, C={Convert.ToString(C, b).PadLeft(8, '0')} / {IP}]");
        Console.Write(
            $"State[A={Convert.ToString((long)A, b)}, B={Convert.ToString((long)B, b)}, C={Convert.ToString((long)C, b)} / {IP}]"
        );
        if (newLine)
            Console.Write("\n");
    }
}

public enum Instruction
{
    ADV = 0,
    BXL = 1,
    BST = 2,
    JNZ = 3,
    BXC = 4,
    OUT = 5,
    BDV = 6,
    CDV = 7,
}

public class Computer
{
    public required State State;
    public List<int> Output = new();
    public required int[] Program;
    protected bool Debug = false;

    public void Reset(State state)
    {
        State = state;
        Output.Clear();
    }

    public virtual bool Step()
    {
        var instruction = (Instruction)Program[State.IP];
        var operant = (int)Program[State.IP + 1];

        var newState = instruction switch
        {
            Instruction.ADV => Adv(State, operant),
            Instruction.BXL => Bxl(State, operant),
            Instruction.BST => Bst(State, operant),
            Instruction.JNZ => Jnz(State, operant),
            Instruction.BXC => Bxc(State, operant),
            Instruction.OUT => Out(State, operant),
            Instruction.BDV => Bdv(State, operant),
            Instruction.CDV => Cdv(State, operant),

            _ => throw new Exception("Invalid instruction"),
        };

        // FIXME: this should not happen if the instruction is Jnz
        if (newState.IP == State.IP)
            State = newState.copyWith(IP: State.IP + 2);
        else
            State = newState;

        return State.IP < Program.Count();
    }

    public ulong ComboOperant(int index)
    {
        return index switch
        {
            0 or 1 or 2 or 3 => (ulong)index,
            4 => State.A,
            5 => State.B,
            6 => State.C,
            7 => throw new Exception("Reserved!"),
            _ => throw new Exception($"Operand {index} is Out Of Range!"),
        };
    }

    /// The adv instruction (opcode 0) performs division.
    /// The numerator is the value in the A register.
    /// The denominator is found by raising 2 to the power of the instruction's combo operand.
    /// (So, an operand of 2 would divide A by 4 (2^2); an operand of 5 would divide A by 2^B.)
    /// The result of the division operation is truncated to an integer and then written to the A register.
    protected State Adv(State state, int operant)
    {
        ulong result = (ulong)(state.A / Math.Pow(2, ComboOperant(operant)));

        return state.copyWith(A: result);
    }

    /// The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand,
    /// then stores the result in register B.
    protected State Bxl(State state, int operant) => state.copyWith(B: (state.B ^ (ulong)operant));

    /// The bst instruction (opcode 2) calculates the value of its combo operand modulo 8
    /// (thereby keeping only its lowest 3 bits), then writes that value to the B register.
    protected State Bst(State state, int operant) => state.copyWith(B: ComboOperant(operant) % 8);

    /// The jnz instruction (opcode 3) does nothing if the A register is 0.
    /// However, if the A register is not zero, it jumps by setting the instruction pointer to the value of its literal operand;
    /// if this instruction jumps, the instruction pointer is not increased by 2 after this instruction.
    protected State Jnz(State state, int operant)
    {
        if (state.A == 0)
            return state;

        return state.copyWith(IP: operant);
    }

    /// The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
    /// then stores the result in register B.
    /// (For legacy reasons, this instruction reads an operand but ignores it.)
    protected State Bxc(State state, int operant) => state.copyWith(B: state.B ^ state.C);

    /// The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
    /// then outputs that value. (If a program outputs multiple values, they are separated by commas.)
    protected virtual State Out(State state, int operant)
    {
        Output.Add((int)(ComboOperant(operant) % 8));
        return State;
    }

    /// The bdv instruction (opcode 6) works exactly like the adv instruction except that the result is stored in the B register.
    /// (The numerator is still read from the A register.)
    protected State Bdv(State state, int operant)
    {
        ulong result = (ulong)(state.A / Math.Pow(2, ComboOperant(operant)));

        return state.copyWith(B: result);
    }

    /// The cdv instruction (opcode 7) works exactly like the adv instruction except that the result is stored in the C register.
    /// (The numerator is still read from the A register.)
    protected State Cdv(State state, int operant)
    {
        ulong result = (ulong)(state.A / Math.Pow(2, ComboOperant(operant)));

        return state.copyWith(C: result);
    }
}
