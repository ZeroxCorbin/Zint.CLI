
using System.Diagnostics;
using System.IO;

namespace Zint.CLI
{
    public class Controller
    {
        public static string ZintPath { get; } = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Zint\\zint-2.13.0\\zint.exe");

        public static string GetCommand(Symbologies type, string data, string output, double scale, int dpi) => new Switches().Barcode(type).XDimensionMils(scale, dpi).Data(data).Output(output).ToString();

        public static bool GetBarcodePath(Symbologies type, string data, ref string output, double xDimMils, int dpi)
        {
            output = Path.Combine(System.IO.Directory.GetCurrentDirectory(), output);
            
            string res = "";
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo(ZintPath, GetCommand(type, data, Path.Combine(System.IO.Directory.GetCurrentDirectory(), output), xDimMils, dpi))
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false,
                        Verb = "open",
                    },
                
            };
            process.OutputDataReceived += (sender, e) => res += e.Data;
            process.Start();

            if (process.WaitForExit(10000))
                return true;
            else
            {
                process.Kill();
                return false;
            }
        }

        public static double GetScale(double xdimMils, int dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
        public static double GetScale(double xdimMils, double dpi) => Math.Round(xdimMils * dpi * 2, MidpointRounding.AwayFromZero) / 2;
        public static double GetMils(double scale, int dpi) => scale / dpi;

        public static double MMtoMils(double mm) => (mm * 39.3701);
        public static double MilsToMM(double mils) => (mils) / 39.3701;

        public static int DPItoDPMM(int dpi) => (int)Math.Round(dpi / 25.4, 0);
    }
}
