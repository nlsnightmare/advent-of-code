class Stone
{
    public long Value;

    public Stone(long value)
    {
        Value = value;
    }

    public List<Stone> Tick()
    {
        if (Value == 0)
        {
            return new List<Stone> { new Stone(1) };
        }

        var digits = Math.Floor(Math.Log10(Value)) + 1;
        if (digits % 2 == 0)
        {
            var top = Value / (long)Math.Pow(10, digits / 2);
            var bottom = Value % (long)Math.Pow(10, digits / 2);

            return new List<Stone> { new Stone(top), new Stone(bottom) };
        }

        return new List<Stone> { new Stone(Value * 2024) };
    }
}
