using Microsoft.Xna.Framework;

namespace Core;

public static class Global
{
    public static GameServiceContainer Services { get; set; }
}

public enum Layer
{
    Background = 0,
    Midground = 1,
    Foreground = 2,
    UI = 3
}
public enum Side
{
    Right,
    Bottom,
    Left,
    Top
}