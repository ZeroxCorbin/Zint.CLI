﻿
using System.Diagnostics;
using System.IO;

namespace Zint.CLI;

public class Controller
{
    public delegate void OutputHandler(string output, bool isError);
    public static event OutputHandler? OutputReceived;

    public static bool IsError { get; private set; }

    public static string ZintPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.13.0\\zint.exe");

    public static string GetCommandScale(Symbologies type, string data, string output, double scale) => new Switches().Barcode(type).XDimensionScale(scale).Data(data).Output(output).ToString();
    public static string GetCommandMils(Symbologies type, string data, string output, double mils, int dpi) => new Switches().Barcode(type).XDimensionMils(mils, dpi).Data(data).Output(output).ToString();

    public static bool GetBarcodePathScale(Symbologies type, string data, ref string output, double xDimMils)
    {
        output = Path.Combine(Directory.GetCurrentDirectory(), output);

        IsError = false;
        var process = LaunchProcess(ZintPath, GetCommandScale(type, data, output, xDimMils), Directory.GetCurrentDirectory());

        if (process.WaitForExit(10000))
            return !IsError;
        else
        {
            process.Kill();
            return false;
        }
    }

    public static bool GetBarcodePathMils(Symbologies type, string data, ref string output, double xDimMils, int dpi)
    {
        output = Path.Combine(Directory.GetCurrentDirectory(), output);

        IsError = false;
        var process = LaunchProcess(ZintPath, GetCommandMils(type, data, output, xDimMils, dpi), Directory.GetCurrentDirectory());

        if (process.WaitForExit(10000))
            return !IsError;
        else
        {
            process.Kill();
            return false;
        }
    }

    private static Process LaunchProcess(string file, string arguments, string working)
    {
        var build = new Process()
        {
            EnableRaisingEvents = true,

            StartInfo = new ProcessStartInfo(file, arguments)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WorkingDirectory = working,
            }
        };

        build.ErrorDataReceived += Build_ErrorDataReceived; ;
        build.OutputDataReceived += Build_OutputDataReceived; ;

        _ = build.Start();
        build.BeginOutputReadLine();
        build.BeginErrorReadLine();

        return build;
    }

    private static void Build_OutputDataReceived(object sender, DataReceivedEventArgs e) => OutputReceived?.Invoke(e.Data, false);
    private static void Build_ErrorDataReceived(object sender, DataReceivedEventArgs e) { if (e.Data == null) return; IsError = true; OutputReceived?.Invoke(e.Data, true); }

    public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
    public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
    public static double GetMils(double scale, int dpi) => scale / dpi;

    public static double MMtoMils(double mm) => mm * 39.3701;
    public static double MilsToMM(double mils) => mils / 39.3701;

    public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
}
