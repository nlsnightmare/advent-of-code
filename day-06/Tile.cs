public record class Position(int x, int y) {
    public static Position operator +(Position a, Position b) => new(a.x + b.x, a.y + b.y);
}

public enum Tile
{
    NONE,
    GUARD,
    WALL,
    EMPTY,
    OBSTRUCTION,
}
