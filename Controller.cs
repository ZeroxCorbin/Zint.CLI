
using System.Diagnostics;
using System.IO;

namespace Zint.CLI
{
    public class Controller
    {
        public static string ZintPath { get; set; } = "\\Zint\\zint-2.13.0\\zint.exe";

        public static string GetCommand(BarcodeTypes type, string data, string output) => new Switches().Barcode(type).Data(data).Output(output).ToString();

        public static bool GetBarcodePath(BarcodeTypes type, string data, string output)
        {
            var ps = new ProcessStartInfo(ZintPath, GetCommand(type, data, output))
            {
                UseShellExecute = true,
                Verb = "open"
            };

            var process = Process.Start(ps);
            if (process == null)
                return false;

            if (process.WaitForExit(10000))
                return true;
            else
            {
                process.Kill();
                return false;
            }
        }
    }
}
