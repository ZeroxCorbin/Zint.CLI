
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Zint.CLI;

public class Controller
{
    public delegate void OutputHandler(string output, bool isError);
    public static event OutputHandler? OutputReceived;

    public static bool IsError { get; private set; }
    public static byte[] Image { get { lock (__imgBufLockObj) return imgBuf.ToArray(); } }

    private static List<byte> imgBuf = new();
    private static object __imgBufLockObj = new();
    private static bool __imgBufLockWasTaken = false;
    private static bool __receivingImage = false;
    public static string ZintPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.13.0\\zint.exe");

    public static string GetCommand(Symbologies type, string data, string output, double scale) => new Switches().Barcode(type).XDimensionScale(scale).Data(data).Output(output).ToString();
    public static string GetCommand(Symbologies type, string data, string output, double mils, int dpi) => new Switches().Barcode(type).XDimensionMils(mils, dpi).Data(data).Output(output).ToString();

    public static string GetCommandStdout(Symbologies type, string data, string fileType, double scale) => new Switches().Barcode(type).XDimensionScale(scale).Data(data).DirectStdout(fileType).ToString();
    public static string GetCommandStdout(Symbologies type, string data, string fileType, double mils, int dpi) => new Switches().Barcode(type).XDimensionMils(mils, dpi).Data(data).DirectStdout(fileType).ToString();

    public static bool GenerateBarcodeFile(Symbologies type, string data, ref string output, double scale)
    {
        output = Path.Combine(Directory.GetCurrentDirectory(), output);

        IsError = false;
        var process = LaunchProcess(ZintPath, GetCommand(type, data, output, scale), Directory.GetCurrentDirectory(), false);

        if (process.WaitForExit(10000))
            return !IsError;
        else
        {
            process.Kill();
            return false;
        }
    }
    public static bool GenerateBarcodeFile(Symbologies type, string data, ref string output, double xDimMils, int dpi)
    {
        output = Path.Combine(Directory.GetCurrentDirectory(), output);

        IsError = false;
        var process = LaunchProcess(ZintPath, GetCommand(type, data, output, xDimMils, dpi), Directory.GetCurrentDirectory(), false);

        if (process.WaitForExit(10000))
            return !IsError;
        else
        {
            process.Kill();
            return false;
        }
    }

    public static bool GenerateBarcodeStdout(Symbologies type, string data, string fileType, double scale)
    {
        IsError = false;


        var process = LaunchProcess(ZintPath, GetCommandStdout(type, data, fileType, scale), Directory.GetCurrentDirectory(), true);

        if (process.WaitForExit(10000))
        {
            lock (__imgBufLockObj)
            {
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();

                imgBuf.Clear();
                imgBuf.AddRange(Encoding.Unicode.GetBytes(output));

                File.WriteAllBytes("test.png", imgBuf.ToArray());
            }

            return !IsError;
        }

        else
        {
            process.Kill();
            return false;
        }




    }

    private static Process LaunchProcess(string file, string arguments, string working, bool noOutData)
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
                StandardOutputEncoding = noOutData ? Encoding.Unicode : null,
            }
        };

        build.ErrorDataReceived += Build_ErrorDataReceived;

        _ = build.Start();

        build.BeginErrorReadLine();

        if (!noOutData)
        {
            build.OutputDataReceived += Build_OutputDataReceived;
            build.BeginOutputReadLine();
        }

        return build;
    }

    private static void Build_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null)
            return;

        OutputReceived?.Invoke(e.Data, false);
    }
    private static void Build_ErrorDataReceived(object sender, DataReceivedEventArgs e) { if (e.Data == null) return; IsError = true; __receivingImage = false; OutputReceived?.Invoke(e.Data, true); }

    public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
    public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
    public static double GetMils(double scale, int dpi) => scale / dpi;

    public static double MMtoMils(double mm) => mm * 39.3701;
    public static double MilsToMM(double mils) => mils / 39.3701;

    public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
}
