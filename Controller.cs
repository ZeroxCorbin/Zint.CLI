using System.Diagnostics;
using System.Text;

namespace Zint.CLI;

public class Controller
{

    public delegate void OutputHandler(string output, bool isError);
    public static event OutputHandler? OutputReceived;

    public static bool IsError { get; private set; }
    public static byte[] Image { get { lock (__imgBufLockObj) return imgBuf.ToArray(); } }

    private static List<byte> imgBuf = [];
    private static object __imgBufLockObj = new();
    private static bool __imgBufLockWasTaken = false;
    private static bool __receivingImage = false;
    public static string ZintPath { get; } = Path.Combine(Directory.GetCurrentDirectory(), "Zint\\zint-2.15.0\\zint.exe");

    // public static string GetCommand(Symbologies type, string data, string output, double scale, TriState quiteZones) => new Switches().Barcode(type).XDimensionScale(scale).QueitZones(quiteZones).Data(data).Output(output).ToString();
    //public static string GetCommand(Symbologies type, string data, string output, double mils, int dpi) => new Switches().Barcode(type).XDimensionMils(mils, dpi).QueitZones().Data(data).Output(output).ToString();

    //public static string GetCommandStdout(Symbologies type, string data, string fileType, double scale) => new Switches().Barcode(type).XDimensionScale(scale).QueitZones().Data(data).DirectStdout(fileType).ToString();
    //public static string GetCommandStdout(Symbologies type, string data, string fileType, double mils, int dpi) => new Switches().Barcode(type).XDimensionMils(mils, dpi).QueitZones().Data(data).DirectStdout(fileType).ToString();

    public static bool SaveBarcode(Barcode barcode)
    {
        IsError = false;
        Process process = LaunchProcess(ZintPath, barcode.CommandArgs, Directory.GetCurrentDirectory(), false);

        if (process.WaitForExit(10000))
            return !IsError;
        else
        {
            process.Kill();
            return false;
        }
    }
    //public static bool SaveBarcode(Symbologies type, string data, string filePath, double xDimMils, int dpi)
    //{
    //    IsError = false;
    //    var process = LaunchProcess(ZintPath, GetCommand(type, data, filePath, xDimMils, dpi), Directory.GetCurrentDirectory(), false);

    //    if (process.WaitForExit(10000))
    //        return !IsError;
    //    else
    //    {
    //        process.Kill();
    //        return false;
    //    }
    //}

    //public static byte[]? GetBarcodeBytes(Symbologies type, string data, string fileType, double scale)
    //{
    //    IsError = false;
    //    var process = LaunchProcess(ZintPath, GetCommandStdout(type, data, fileType, scale), Directory.GetCurrentDirectory(), true);

    //    if (process.WaitForExit(10000))
    //    {
    //        if(IsError)
    //            return null;

    //        StreamReader reader = process.StandardOutput;
    //        string output = reader.ReadToEnd();
    //        return Encoding.Unicode.GetBytes(output);
    //    }
    //    else
    //    {
    //        process.Kill();
    //        return null;
    //    }
    //}

    //public static byte[]? GetBarcodeBytes(Symbologies type, string data, string fileType, double xDimMils, int dpi)
    //{
    //    IsError = false;
    //    var process = LaunchProcess(ZintPath, GetCommandStdout(type, data, fileType, xDimMils / 1000, dpi), Directory.GetCurrentDirectory(), true);

    //    if (process.WaitForExit(10000))
    //    {
    //        if (IsError)
    //            return null;

    //        StreamReader reader = process.StandardOutput;
    //        string output = reader.ReadToEnd();
    //        return Encoding.Unicode.GetBytes(output);
    //    }
    //    else
    //    {
    //        process.Kill();
    //        return null;
    //    }
    //}

    //public static Barcode GetBarcode(Symbologies type, string data, string fileType, double scale)
    //{
    //    var code = new Barcode();
    //    IsError = false;

    //    code.CommandArgs = GetCommandStdout(type, data, fileType, scale);
    //    var process = LaunchProcess(ZintPath, code.CommandArgs, Directory.GetCurrentDirectory(), true);

    //    if (process.WaitForExit(10000))
    //    {
    //        if (IsError)
    //            return code;

    //        StreamReader reader = process.StandardOutput;
    //        string output = reader.ReadToEnd();
    //        code.Data = Encoding.Unicode.GetBytes(output);
    //        code.IsValid = !IsError;
    //        return code;
    //    }
    //    else
    //    {
    //        process.Kill();
    //        return code;
    //    }
    //}

    //public static Barcode GetBarcode(Symbologies type, string data, string fileType, double xDimMils, int dpi)
    //{
    //    var code = new Barcode();
    //    IsError = false;

    //    code.CommandArgs = GetCommandStdout(type, data, fileType, xDimMils, dpi);
    //    var process = LaunchProcess(ZintPath, code.CommandArgs, Directory.GetCurrentDirectory(), true);

    //    if (process.WaitForExit(10000))
    //    {
    //        if (IsError)
    //            return code;

    //        StreamReader reader = process.StandardOutput;
    //        string output = reader.ReadToEnd();
    //        code.Data = Encoding.Unicode.GetBytes(output);
    //        code.IsValid = !IsError;

    //        return code;
    //    }
    //    else
    //    {
    //        process.Kill();
    //        return code;
    //    }
    //}

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

    public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi, MidpointRounding.AwayFromZero) / 2;
    public static double GetMils(double scale, int dpi) => scale / dpi * 2;

    public static double GetDPI(double xdimMils, double scale) => scale / xdimMils * 2;

    public static double MMtoMils(double mm) => mm * 39.3701;
    public static double MilsToMM(double mils) => mils / 39.3701;

    public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
}
