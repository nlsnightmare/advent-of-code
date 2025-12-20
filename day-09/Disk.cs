abstract class DiskBlock
{
    public required int Index { get; set; }
    public abstract override string ToString();
};

class DiskFile : DiskBlock
{
    public required int Id;
    public required int Size;

    public override string ToString()
    {
        return new string((char)(Id + '0'), Size);
    }
}

class DiskSpace : DiskBlock
{
    public required int Size;

    public bool Consume(int size = 1)
    {
        Size -= size;
        Index += size;
        return Size > 0;
    }

    public void GrowLeft(int size = 1)
    {
        Size += size;
        Index -= size;
    }

    public override string ToString()
    {
        return new string('.', Size);
    }
}

class Disk
{
    private List<DiskBlock> _blocks = new();

    public Disk(string input)
    {
        input = input.Trim();

        int id = 0;
        int index = 0;
        for (int i = 0; i < input.Length; i++)
        {
            int size = input[i] - '0';
            if (size == 0)
                continue;

            if (i % 2 == 0)
            {
                var file = new DiskFile
                {
                    Index = index,
                    Id = id,
                    Size = size,
                };

                _blocks.Add(file);
                id++;
            }
            else
            {
                _blocks.Add(new DiskSpace { Index = index, Size = size });
            }

            index++;
        }

        _blocks.Add(new DiskSpace { Index = input.Length, Size = 0 });
    }

    public void Compact()
    {
        while (true)
        {
            var lastSpace = (DiskSpace)_blocks.FindLast(b => b is DiskSpace)!;
            var freespace = FirstFreeSpace();

            if (freespace is null || lastSpace == freespace)
            {
                return;
            }
            var spaceIndex = _blocks.IndexOf(freespace);

            if (freespace.Consume() == false)
            {
                _blocks.Remove(freespace);
            }

            int fileIndex = _blocks.FindLastIndex(block => block is DiskFile);
            DiskFile file = (DiskFile)_blocks[fileIndex]!;

            if (file is null)
                return;

            var copy = new DiskFile
            {
                Id = file.Id,
                Size = 1,
                Index = spaceIndex,
            };
            _blocks.Insert(spaceIndex, copy);

            lastSpace.GrowLeft();

            file.Size--;
            if (file.Size == 0)
                _blocks.Remove(file);
        }
    }

    public void Compact2()
    {
        var maxId = Files.Last().Id;
        var ids = Enumerable.Range(0, maxId).Reverse();
        ids.Reverse();
        foreach (var file in Files.Reverse())
        {
            var space = Spaces.Where(s => s.Size >= file.Size).FirstOrDefault();
            if (space is not null)
            {
                var index = _blocks.IndexOf(space);
                var fileIndex = _blocks.IndexOf(file);

				if (index > fileIndex) continue;

                space.Consume(file.Size);
                _blocks[fileIndex] = new DiskSpace { Size = file.Size, Index = fileIndex };
                _blocks.Insert(index, file);
            }
        }
    }

    private DiskSpace? FirstFreeSpace()
    {
        int spaceIndex = _blocks.FindIndex(b => b is DiskSpace);
        if (spaceIndex == -1)
            return null;
        return (DiskSpace)_blocks[spaceIndex]!;
    }

    public double Checksum()
    {
        return ToString()
            .Select(
                (char c, int index) =>
                {
                    if (c == '.')
                        return 0;
                    double intValue = c - '0';
                    return intValue * index;
                }
            )
            .Sum();
    }

    private IEnumerable<DiskFile> Files => _blocks.OfType<DiskFile>();
    private IEnumerable<DiskSpace> Spaces => _blocks.OfType<DiskSpace>();

    public void Inspect()
    {
        foreach (var block in _blocks)
        {
            var message = block switch
            {
                DiskSpace space => $"Space(Size={space.Size})",
                DiskFile file => $"File(Size={file.Size})",
            };

            Console.WriteLine(message);
        }
    }

    public override string ToString()
    {
        return string.Join("", _blocks);
    }
}
