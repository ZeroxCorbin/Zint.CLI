using CommunityToolkit.Mvvm.ComponentModel;
using System.Xml;

namespace Zint.CLI;

public partial class Barcode : ObservableObject
{
    [ObservableProperty] private Symbologies type = Symbologies.DataMatrix;
    [ObservableProperty] private byte[] image = [];
    [ObservableProperty] private string data = "";
    [ObservableProperty] private bool dataIsEscaped = false;
    [ObservableProperty] private string outPath = "output.bmp";

    [ObservableProperty] private double targetDpi;
    [ObservableProperty] private double scale;

    [ObservableProperty] private TriState quietZones = TriState.Auto;
    [ObservableProperty] private bool isGs1 = false;

    /// <summary>
    /// null = Automatic, true = Square, false = Rectangular
    /// </summary>
    [ObservableProperty] private TriState dataMatrixShape = TriState.Auto;

    public string CommandArgs { get; set; }

    [ObservableProperty] private bool isValid = false;

}
