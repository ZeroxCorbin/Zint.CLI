namespace Zint.CLI;

public struct ColorRgb
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }

    public ColorRgb(byte r, byte g, byte b)
    {
        R = r;
        G = g;
        B = b;
    }

    public static ColorRgb FromRgb(byte r, byte g, byte b) => new(r, g, b);
}
