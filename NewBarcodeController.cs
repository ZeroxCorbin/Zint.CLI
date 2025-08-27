using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zint.CLI;

/// <summary>
/// Controller for interacting with the Zint.exe command-line application.
/// </summary>
public class ZintController
{
    private readonly string _zintExecutablePath;

    /// <summary>
    /// Initializes a new instance of the ZintController.
    /// </summary>
    /// <param name="zintExecutablePath">The full path to Zint.exe.</param>
    public ZintController(string zintExecutablePath)
    {
        if (!File.Exists(zintExecutablePath))
        {
            throw new FileNotFoundException("Zint executable not found at the specified path.", zintExecutablePath);
        }
        _zintExecutablePath = zintExecutablePath;
    }

    /// <summary>
    /// Asynchronously generates a barcode image using the provided settings.
    /// </summary>
    /// <param name="barcode">The Barcode object containing generation settings.</param>
    /// <returns>The updated Barcode object with the GeneratedImage property set.</returns>
    public async Task<NewBarcode> GenerateAsync(NewBarcode barcode)
    {
        barcode.IsValid = false;
        barcode.GeneratedImage = null;

        string outputPath = barcode.OutputPath ?? Path.ChangeExtension(Path.GetTempFileName(), ".png");
        string arguments = BuildArguments(barcode, outputPath);

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
        process.Start();

        string error = await process.StandardError.ReadToEndAsync();
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

    private string BuildArguments(NewBarcode barcode, string outputPath)
    {
        var switches = new Switches();

        // Core arguments
        switches.Barcode(barcode.Symbology);
        switches.Output(outputPath);

        if (!string.IsNullOrEmpty(barcode.InputPath))
        {
            switches.Input(barcode.InputPath);
        }
        else
        {
            switches.Data(barcode.Data);
        }

        // Sizing and Appearance
        if (barcode.Height.HasValue) switches.Height(barcode.Height.Value);
        if (barcode.Scale.HasValue) switches.Scale(barcode.Scale.Value);
        if (barcode.BorderWidth.HasValue) switches.Border(barcode.BorderWidth.Value);
        if (barcode.Whitespace.HasValue) switches.Whitespace(barcode.Whitespace.Value);
        if (barcode.VerticalWhitespace.HasValue) switches.VWhitespace(barcode.VerticalWhitespace.Value);
        if (barcode.RotationAngle.HasValue) switches.Rotate(barcode.RotationAngle.Value);
        if (barcode.BindBars) switches.Bind();
        if (barcode.AddBox) switches.Box();
        if (barcode.CompliantHeight) switches.CompliantHeight();
        if (barcode.HeightPerRow) switches.HeightPerRow();
        if (barcode.BindTop) switches.BindTop();

        // Coloring
        if (barcode.ForegroundColor.HasValue) switches.Foreground(ColorToHex(barcode.ForegroundColor.Value));
        if (barcode.BackgroundColor.HasValue) switches.Background(ColorToHex(barcode.BackgroundColor.Value));
        if (barcode.ReverseColors) switches.Reverse();
        if (barcode.NoBackground) switches.NoBackground();
        if (barcode.UseCmyk) switches.Cmyk();

        // Text and Font
        if (barcode.HideText) switches.NoText();
        if (barcode.BoldText) switches.Bold();
        if (barcode.SmallText) switches.Small();
        if (barcode.TextGap.HasValue) switches.TextGap(barcode.TextGap.Value);
        if (barcode.EmbedFont) switches.EmbedFont();

        // Flags
        if (barcode.ProcessTilde) switches.EscapeInput();
        if (barcode.IsGs1Data) switches.GS1();
        if (barcode.BinaryMode) switches.Binary();
        if (barcode.Eci.HasValue) switches.Eci(barcode.Eci.Value);
        if (barcode.Gs1Separator) switches.Gs1Separator();
        switches.QuietZones(barcode.QuietZones);
        if (barcode.ForceSquare) switches.Symbol_DataMatrix(TriState.On);


        // Advanced Options
        string arguments = switches.ToString();
        if (!string.IsNullOrWhiteSpace(barcode.AdvancedOptions))
        {
            arguments += $" {barcode.AdvancedOptions}";
        }

        return arguments;
    }

    private string ColorToHex(Color c) => $"{c.R:X2}{c.G:X2}{c.B:X2}";
}