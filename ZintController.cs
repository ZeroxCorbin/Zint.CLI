using System.Diagnostics;
using System.Drawing;

namespace Zint.CLI;

/// <summary>
/// Controller for interacting with the Zint.exe command-line application.
/// </summary>
public class ZintController
{
    private readonly string _zintExecutablePath = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.15.0\\zint.exe");

    /// <summary>
    /// Asynchronously generates a barcode image using the provided settings.
    /// </summary>
    /// <param name="barcode">The Barcode object containing generation settings.</param>
    /// <returns>The updated Barcode object with the GeneratedImage property set.</returns>
    public async Task<BarcodeSettings> GenerateAsync(BarcodeSettings barcode)
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
        {
            throw new InvalidOperationException($"Zint failed with exit code {process.ExitCode}. Error: {error}");
        }

        try
        {
            // Load the image from file into a memory stream to prevent file locking
            barcode.GeneratedImage = await File.ReadAllBytesAsync(outputPath);
            barcode.IsValid = true;
        }
        finally
        {
            // Clean up the temporary file if one was used
            if (string.IsNullOrEmpty(barcode.OutputPath) && File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        return barcode;
    }

    private string BuildArguments(BarcodeSettings barcode, string outputPath)
    {
        var switches = new Switches();

        // Core arguments
        _ = switches.Barcode(barcode.Symbology);
        _ = switches.Output(outputPath);

        if (!string.IsNullOrEmpty(barcode.InputPath))
        {
            _ = switches.Input(barcode.InputPath);
        }
        else
        {
            _ = switches.Data(barcode.Data);
        }

        if (!string.IsNullOrEmpty(barcode.PrimaryData)) _ = switches.Primary(barcode.PrimaryData);

        // Sizing and Appearance
        if (barcode.Height.HasValue) _ = switches.Height(barcode.Height.Value);

        if (!string.IsNullOrEmpty(barcode.ScaleXDimDp)) _ = switches.ScaleXDimDp(barcode.ScaleXDimDp);
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

        // Coloring
        if (barcode.ForegroundColor.HasValue) _ = switches.Foreground(ColorToHex(barcode.ForegroundColor.Value));
        if (barcode.BackgroundColor.HasValue) _ = switches.Background(ColorToHex(barcode.BackgroundColor.Value));
        if (barcode.ReverseColors) _ = switches.Reverse();
        if (barcode.NoBackground) _ = switches.NoBackground();
        if (barcode.UseCmyk) _ = switches.Cmyk();

        // Text and Font
        if (barcode.HideText) _ = switches.NoText();
        if (barcode.BoldText) _ = switches.Bold();
        if (barcode.SmallText) _ = switches.Small();
        if (barcode.TextGap.HasValue) _ = switches.TextGap(barcode.TextGap.Value);
        if (barcode.EmbedFont) _ = switches.EmbedFont();
        if (barcode.AddOnGap.HasValue) _ = switches.AddOnGap(barcode.AddOnGap.Value);
        if (barcode.GuardWhitespace) _ = switches.GuardWhitespace();
        if (barcode.GuardDescent.HasValue) _ = switches.GuardDescent(barcode.GuardDescent.Value);

        // Flags
        if (barcode.ProcessTilde) _ = switches.EscapeInput();
        if (barcode.IsGs1Data && !IsImplicitGs1Symbology(barcode.Symbology)) _ = switches.GS1();
        if (barcode.BinaryMode) _ = switches.Binary();
        if (barcode.Eci.HasValue) _ = switches.Eci(barcode.Eci.Value);
        if (barcode.Gs1Separator) _ = switches.Gs1Separator();
        if (barcode.QuietZones.HasValue)
        {
            if (barcode.QuietZones.Value)
            {
                _ = switches.QuietZones();
            }
            else
            {
                _ = switches.NoQuietZones();
            }
        }
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

        // Advanced Options
        var arguments = switches.ToString();
        if (!string.IsNullOrWhiteSpace(barcode.AdvancedOptions))
        {
            arguments += $" {barcode.AdvancedOptions}";
        }

        return arguments;
    }

    private bool IsImplicitGs1Symbology(Symbologies symbology) => symbology switch
    {
        Symbologies.Ean128 => true,
        Symbologies.Gs1_128 => true,
        Symbologies.Gs1DataBar => true,
        Symbologies.Gs1DataBarLimited => true,
        Symbologies.Gs1DataBarExpanded => true,
        Symbologies.Gs1DataBarStacked => true,
        Symbologies.Gs1DataBarStackedOmnidirectional => true,
        Symbologies.Gs1DataBarExpandedStacked => true,
        Symbologies.Gs1_128_Composite => true,
        Symbologies.Gs1DataBar_Composite => true,
        Symbologies.Gs1DataBarLimited_Composite => true,
        Symbologies.Gs1DataBarExpanded_Composite => true,
        Symbologies.Gs1DataBarStacked_Composite => true,
        Symbologies.Gs1DataBarStackedOmni_Composite => true,
        Symbologies.Gs1DataBarExpandedStacked_Composite => true,
        _ => false,
    };

    private string ColorToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";

    public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetMils(double scale, int dpi) => scale / dpi * 2;

    public static double GetDPI(double xdimMils, double scale) => scale / xdimMils * 2;

    public static double MMtoMils(double mm) => mm * 39.3701;
    public static double MilsToMM(double mils) => mils / 39.3701;

    public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
}