namespace Zint.CLI
{
    public class XDim
    {
        public double Mils { get; set; }
        public double DPI { get; set; }
        public double Scale => Math.Round(Controller.GetScale(Mils / 1000, DPI), 3);
        public double PPE => Math.Round(Controller.GetScale(Mils / 1000, DPI) * 2, 3);
    }

    public class Barcodes
    {
        public static List<XDim> GetXDims(int dpi, bool isVector)
        {
            var ret = new List<XDim>();
            var scaleInc = isVector ? 0.1 : 0.5;

            for (var i = scaleInc; ; i += scaleInc)
            {
                var mil = Math.Round(Controller.GetMils(i, dpi) * 1000, 3);

                if (mil > 100)
                    break;

                ret.Add(new XDim() { Mils = mil, DPI = dpi });
            }
            return ret;
        }

        public static XDim GetClosestXDim(int dpi, double mils, bool isVector = false)
        {
            var xDims = GetXDims(dpi, isVector);
            var closest = xDims.OrderBy(x => Math.Abs(x.Mils - mils)).First();
            return closest;
        }

    }
}
