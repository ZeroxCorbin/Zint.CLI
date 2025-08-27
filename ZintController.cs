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
    private readonly string _zintExecutablePath = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.15.0\\zint.exe");

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

        if (!string.IsNullOrEmpty(barcode.PrimaryData)) switches.Primary(barcode.PrimaryData);

        // Sizing and Appearance
        if (barcode.Height.HasValue) switches.Height(barcode.Height.Value);
        if (barcode.Scale.HasValue) switches.Scale(barcode.Scale.Value);
        if (!string.IsNullOrEmpty(barcode.ScaleXDimDp)) switches.ScaleXDimDp(barcode.ScaleXDimDp);
        if (barcode.BorderWidth.HasValue) switches.Border(barcode.BorderWidth.Value);
        if (barcode.Whitespace.HasValue) switches.Whitespace(barcode.Whitespace.Value);
        if (barcode.VerticalWhitespace.HasValue) switches.VWhitespace(barcode.VerticalWhitespace.Value);
        if (barcode.RotationAngle.HasValue) switches.Rotate(barcode.RotationAngle.Value);
        if (barcode.BindBars) switches.Bind();
        if (barcode.AddBox) switches.Box();
        if (barcode.CompliantHeight) switches.CompliantHeight();
        if (barcode.HeightPerRow) switches.HeightPerRow();
        if (barcode.BindTop) switches.BindTop();
        if (barcode.DottyMode) switches.Dotty();
        if (barcode.DotSize.HasValue) switches.DotSize(barcode.DotSize.Value);
        if (barcode.Columns.HasValue) switches.Cols(barcode.Columns.Value);
        if (barcode.Rows.HasValue) switches.Rows(barcode.Rows.Value);
        if (barcode.SeparatorHeight.HasValue) switches.Separator(barcode.SeparatorHeight.Value);

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
        if (barcode.AddOnGap.HasValue) switches.AddOnGap(barcode.AddOnGap.Value);
        if (barcode.GuardWhitespace) switches.GuardWhitespace();
        if (barcode.GuardDescent.HasValue) switches.GuardDescent(barcode.GuardDescent.Value);

        // Flags
        if (barcode.ProcessTilde) switches.EscapeInput();
        if (barcode.IsGs1Data) switches.GS1();
        if (barcode.BinaryMode) switches.Binary();
        if (barcode.Eci.HasValue) switches.Eci(barcode.Eci.Value);
        if (barcode.Gs1Separator) switches.Gs1Separator();
        if (barcode.QuietZones.HasValue)
        {
            if (barcode.QuietZones.Value)
            {
                switches.QuietZones();
            }
            else
            {
                switches.NoQuietZones();
            }
        }
        if (barcode.ForceSquare) switches.Square();
        if (barcode.Gs1Parens) switches.Gs1Parens();
        if (barcode.Version.HasValue) switches.Vers(barcode.Version.Value);
        if (barcode.SecurityLevel.HasValue) switches.Secure(barcode.SecurityLevel.Value);
        if (barcode.Mask.HasValue) switches.Mask(barcode.Mask.Value);
        if (barcode.Scmvv.HasValue) switches.Scmvv(barcode.Scmvv.Value);
        if (barcode.Mode.HasValue) switches.Mode(barcode.Mode.Value);
        if (barcode.UseDmre) switches.Dmre();
        if (barcode.UseDmIso144) switches.DmIso144();
        if (barcode.UseFastEncoding) switches.Fast();
        if (barcode.UseFullMultibyte) switches.FullMultibyte();
        if (barcode.ReaderInitialization) switches.Init();


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