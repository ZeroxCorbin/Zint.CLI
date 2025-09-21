using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;

namespace Zint.CLI;

/// <summary>
/// Controller for interacting with the Zint.exe command-line application.
/// </summary>
public class ZintController
{
    private readonly string _zintExecutablePath = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.15.0\\zint.exe");

    public async Task<BarcodeSettings> GenerateAsync(BarcodeSettings barcode, int targetDpi)
    {
        barcode.IsValid = false;
        barcode.GeneratedImage = null;

        var outputPath = barcode.OutputPath ?? Path.ChangeExtension(Path.GetTempFileName(), ".png");
        var arguments = BuildArguments(barcode, outputPath);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = _zintExecutablePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = processStartInfo };
        _ = process.Start();

        var error = await process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();

        if (process.ExitCode != 0)
            throw new InvalidOperationException($"Zint failed with exit code {process.ExitCode}. Error: {error}");

        try
        {
            // Ensure DPI tagging for raster output
            barcode.GeneratedImage = ImageUtilities.lib.Wpf.ImageFormatHelpers.EnsureDpi(
                File.ReadAllBytes(outputPath), targetDpi, targetDpi, out double _, out double _, true);
            barcode.IsValid = true;
        }
        finally
        {
            if (string.IsNullOrEmpty(barcode.OutputPath) && File.Exists(outputPath))
                File.Delete(outputPath);
        }

        return barcode;
    }

    private string BuildArguments(BarcodeSettings barcode, string outputPath)
    {
        var switches = new Switches();

        // Core
        _ = switches.Barcode(barcode.Symbology);
        _ = switches.Output(outputPath);
        if (!string.IsNullOrEmpty(barcode.InputPath))
            _ = switches.Input(barcode.InputPath);
        else
            _ = switches.Data(barcode.Data);
        if (!string.IsNullOrEmpty(barcode.PrimaryData))
            _ = switches.Primary(barcode.PrimaryData);

        // Sizing & Appearance
        if (barcode.Height.HasValue) _ = switches.Height(barcode.Height.Value);
        if (barcode.UseScaleXDimDp) _ = switches.ScaleXDimDp(barcode.ScaleXDimDp);
        else if (barcode.Scale.HasValue) _ = switches.Scale(barcode.Scale.Value);
        if (barcode.BorderWidth.HasValue) _ = switches.Border(barcode.BorderWidth.Value);
        if (barcode.Whitespace.HasValue) _ = switches.Whitespace(barcode.Whitespace.Value);
        if (barcode.VerticalWhitespace.HasValue) _ = switches.VWhitespace(barcode.VerticalWhitespace.Value);
        if (barcode.RotationAngle.HasValue) _ = switches.Rotate(barcode.RotationAngle.Value);
        if (barcode.BindBars) _ = switches.Bind();
        if (barcode.AddBox) _ = switches.Box();
        if (barcode.CompliantHeight) _ = switches.CompliantHeight();
        if (barcode.HeightPerRow) _ = switches.HeightPerRow();
        if (barcode.BindTop) _ = switches.BindTop();
        if (barcode.DottyMode) _ = switches.Dotty();
        if (barcode.DotSize.HasValue) _ = switches.DotSize(barcode.DotSize.Value);
        if (barcode.Columns.HasValue) _ = switches.Cols(barcode.Columns.Value);
        if (barcode.Rows.HasValue) _ = switches.Rows(barcode.Rows.Value);
        if (barcode.SeparatorHeight.HasValue) _ = switches.Separator(barcode.SeparatorHeight.Value);

        // Colors
        if (barcode.ForegroundColor.HasValue) _ = switches.Foreground(ColorToHex(barcode.ForegroundColor.Value));
        if (barcode.BackgroundColor.HasValue) _ = switches.Background(ColorToHex(barcode.BackgroundColor.Value));
        if (barcode.ReverseColors) _ = switches.Reverse();
        if (barcode.NoBackground) _ = switches.NoBackground();
        if (barcode.UseCmyk) _ = switches.Cmyk();

        // Text
        if (barcode.HideText) _ = switches.NoText();
        if (barcode.BoldText) _ = switches.Bold();
        if (barcode.SmallText) _ = switches.Small();
        if (barcode.TextGap.HasValue) _ = switches.TextGap(barcode.TextGap.Value);
        if (barcode.EmbedFont) _ = switches.EmbedFont();
        if (barcode.AddOnGap.HasValue) _ = switches.AddOnGap(barcode.AddOnGap.Value);
        if (barcode.GuardWhitespace) _ = switches.GuardWhitespace();
        if (barcode.GuardDescent.HasValue) _ = switches.GuardDescent(barcode.GuardDescent.Value);

        // Special / Flags
        if (barcode.ProcessTilde) _ = switches.EscapeInput();

        if (barcode.IsGs1Data && !IsImplicitGs1Symbology(barcode.Symbology))
            _ = switches.GS1();

        if (barcode.BinaryMode) _ = switches.Binary();
        if (barcode.Eci.HasValue) _ = switches.Eci(barcode.Eci.Value);
        if (barcode.Gs1Separator) _ = switches.Gs1Separator();
        if (barcode.QuietZones.HasValue)
            _ = (barcode.QuietZones.Value ? switches.QuietZones() : switches.NoQuietZones());
        if (barcode.ForceSquare) _ = switches.Square();
        if (barcode.Gs1Parens) _ = switches.Gs1Parens();
        if (barcode.Version.HasValue) _ = switches.Vers(barcode.Version.Value);
        if (barcode.SecurityLevel.HasValue) _ = switches.Secure(barcode.SecurityLevel.Value);
        if (barcode.Mask.HasValue) _ = switches.Mask(barcode.Mask.Value);
        if (barcode.Scmvv.HasValue) _ = switches.Scmvv(barcode.Scmvv.Value);
        if (barcode.Mode.HasValue) _ = switches.Mode(barcode.Mode.Value);
        if (barcode.UseDmre) _ = switches.Dmre();
        if (barcode.UseDmIso144) _ = switches.DmIso144();
        if (barcode.UseFastEncoding) _ = switches.Fast();
        if (barcode.UseFullMultibyte) _ = switches.FullMultibyte();
        if (barcode.ReaderInitialization) _ = switches.Init();

        var args = switches.ToString();
        if (!string.IsNullOrWhiteSpace(barcode.AdvancedOptions))
            args += $" {barcode.AdvancedOptions}";

        return args;
    }

    // Correct implicit GS1 detection: use actual Zint.CLI Symbologies names (Gs1*).
    // We also treat any future additions whose enum name starts with "Gs1DataBar" or contains "Gs1_128".
    private static readonly HashSet<Symbologies> ExplicitImplicitGs1 = new()
    {
        Symbologies.Gs1DataBar,
        Symbologies.Gs1DataBarLimited,
        Symbologies.Gs1DataBarExpanded,
        Symbologies.Gs1DataBarStacked,
        Symbologies.Gs1DataBarStackedOmnidirectional,
        Symbologies.Gs1DataBarExpandedStacked,
        Symbologies.Gs1DataBar_Composite,
        Symbologies.Gs1DataBarLimited_Composite,
        Symbologies.Gs1DataBarExpanded_Composite,
        Symbologies.Gs1DataBarStacked_Composite,
        Symbologies.Gs1DataBarStackedOmni_Composite,
        Symbologies.Gs1DataBarExpandedStacked_Composite,
    };

    private static bool IsImplicitGs1Symbology(Symbologies sym)
    {
        var name = sym.ToString();
        if (ExplicitImplicitGs1.Contains(sym))
            return true;

        // Fallback pattern-based detection for future enum members
        if (name.StartsWith("Gs1DataBar", StringComparison.OrdinalIgnoreCase) ||
            name.Contains("Gs1_128", StringComparison.OrdinalIgnoreCase) ||
            name.Equals("Ean128", StringComparison.OrdinalIgnoreCase))
            return true;

        return false;
    }

    private string ColorToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";

    public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetMils(double scale, int dpi) => scale / dpi * 2;
    public static double GetDPI(double xdimMils, double scale) => scale / xdimMils * 2;
    public static double MMtoMils(double mm) => mm * 39.3701;
    public static double MilsToMM(double mils) => mils / 39.3701;
    public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
}