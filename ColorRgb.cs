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

    public override string ToString() => $"{R:X2}{G:X2}{B:X2}";

    public static ColorRgb Parse(string hex)
    {
        hex = hex.TrimStart('#');
        if (hex.Length != 6) throw new FormatException("Color must be 6 hex digits (RRGGBB)");
        return new ColorRgb(
            Convert.ToByte(hex.Substring(0, 2), 16),
            Convert.ToByte(hex.Substring(2, 2), 16),
            Convert.ToByte(hex.Substring(4, 2), 16));
    }
}
