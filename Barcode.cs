using CommunityToolkit.Mvvm.ComponentModel;

namespace Zint.CLI;

public partial class Barcode : ObservableObject
{
    [ObservableProperty] private Symbologies type;
    [ObservableProperty] private string data;
    [ObservableProperty] private bool dataIsEscaped = false;
    [ObservableProperty] private string outPath;

    [ObservableProperty] private double targetDpi;
    [ObservableProperty] private double scale;

    [ObservableProperty] private TriState quietZones = TriState.Auto;
    [ObservableProperty] private bool isGs1 = false;

    /// <summary>
    /// null = Automatic, true = Square, false = Rectangular
    /// </summary>
    [ObservableProperty] private TriState dataMatrixShape = TriState.Auto;

    public string CommandArgs
    {
        get
        {
            Switches args = new Switches().Barcode(Type).XDimensionScale(Scale).Data(Data).Output(OutPath).QueitZones(QuietZones);

            if(DataIsEscaped)
                _ = args.EscapeInput();

            if (IsGs1)
                _ = args.GS1();

            if (Type == Symbologies.BARCODE_DATAMATRIX)
                _ = args.Symbol_DataMatrix(DataMatrixShape);

            return args.ToString();
        }
    }
    [ObservableProperty] private bool isValid = false;
    public Barcode(Symbologies type, string data, string outPath, double scale)
    {
        Type = type;
        Data = data;
        OutPath = outPath;
        Scale = scale;
    }

    public Barcode()
    {
        Type = Symbologies.BARCODE_CODE128;
        Data = "Hello, World!";
        OutPath = "barcode.png";
        Scale = 1.0;
    }
}
