using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Zint.CLI;

/// <summary>
/// A data class representing all settings for generating a barcode with Zint.
/// </summary>
public partial class NewBarcode : ObservableObject
{
    [ObservableProperty]
    private bool isValid = false;
    // C# and WPF are your primary tools, so we'll use System.Drawing.Image for broad compatibility.
    // This can be easily converted to a WPF BitmapImage in your UI layer.
    [ObservableProperty]
    private byte[]? generatedImage;

    // -- CORE PROPERTIES --

    /// <summary>
    /// The data to be encoded in the barcode.
    /// Corresponds to Zint's -d flag.
    /// </summary>
    [ObservableProperty]
    private string data;

    /// <summary>
    /// The type of barcode symbology to use.
    /// Corresponds to Zint's -b flag.
    /// </summary>
    [ObservableProperty]
    private Symbologies symbology;

    /// <summary>
    /// The full path for the output image file.
    /// If left null, a temporary file will be created.
    /// Corresponds to Zint's -o flag.
    /// </summary>
    [ObservableProperty]
    private string? outputPath;

    /// <summary>
    /// Path to a file to read input data from.
    /// Corresponds to Zint's -i flag.
    /// </summary>
    [ObservableProperty]
    private string? inputPath;

    // -- SIZING AND APPEARANCE --

    /// <summary>
    /// The height of the barcode bars in Zint units.
    /// Corresponds to Zint's --height flag.
    /// </summary>
    [ObservableProperty]
    private double? height;

    /// <summary>
    /// The scaling factor for the barcode.
    /// Corresponds to Zint's --scale option.
    /// </summary>
    [ObservableProperty]
    private double? scale;

    /// <summary>
    /// The width of the border around the barcode.
    /// Corresponds to Zint's --border flag.
    /// </summary>
    [ObservableProperty]
    private int? borderWidth;

    /// <summary>
    /// The amount of horizontal whitespace to the left and right of the symbol.
    /// Corresponds to Zint's --whitesp flag.
    /// </summary>
    [ObservableProperty]
    private int? whitespace;

    /// <summary>
    /// The amount of vertical whitespace above and below the symbol.
    /// Corresponds to Zint's --vwhitesp flag.
    /// </summary>
    [ObservableProperty]
    private int? verticalWhitespace;

    /// <summary>
    /// Rotates the barcode image. Valid values are 0, 90, 180, 270.
    /// Corresponds to Zint's --rotate flag.
    /// </summary>
    [ObservableProperty]
    private int? rotationAngle;

    /// <summary>
    /// Specifies whether to add boundary bars.
    /// Corresponds to Zint's --bind flag.
    /// </summary>
    [ObservableProperty]
    private bool bindBars;

    /// <summary>
    /// Specifies whether to add a box around the symbol.
    /// Corresponds to Zint's --box flag.
    /// </summary>
    [ObservableProperty]
    private bool addBox;

    /// <summary>
    /// Use compliant height for the symbology.
    /// Corresponds to Zint's --compliantheight flag.
    /// </summary>
    [ObservableProperty]
    private bool compliantHeight;

    /// <summary>
    /// Treat height as per-row height.
    /// Corresponds to Zint's --heightperrow flag.
    /// </summary>
    [ObservableProperty]
    private bool heightPerRow;

    /// <summary>
    /// Add a boundary bar to the top of the symbol.
    /// Corresponds to Zint's --bindtop flag.
    /// </summary>
    [ObservableProperty]
    private bool bindTop;

    // -- COLORING --

    /// <summary>
    /// The foreground (bar) color of the barcode.
    /// Corresponds to Zint's --fg flag.
    /// </summary>
    [ObservableProperty]
    private Color? foregroundColor;

    /// <summary>
    /// The background color of the barcode.
    /// Corresponds to Zint's --bg flag.
    /// </summary>
    [ObservableProperty]
    private Color? backgroundColor;

    /// <summary>
    /// Inverts the foreground and background colors.
    /// Corresponds to Zint's --reverse flag.
    /// </summary>
    [ObservableProperty]
    private bool reverseColors;

    /// <summary>
    /// Removes the background color, making it transparent.
    /// Corresponds to Zint's --nobackground flag.
    /// </summary>
    [ObservableProperty]
    private bool noBackground;

    /// <summary>
    /// Use CMYK color space for EPS and TIF files.
    /// Corresponds to Zint's --cmyk flag.
    /// </summary>
    [ObservableProperty]
    private bool useCmyk;

    // -- TEXT AND FONT --

    /// <summary>
    /// If true, hides the human-readable text.
    /// Corresponds to Zint's --notext flag.
    /// </summary>
    [ObservableProperty]
    private bool hideText = false;

    /// <summary>
    /// Use a bold font for the Human Readable Text.
    /// Corresponds to Zint's --bold flag.
    /// </summary>
    [ObservableProperty]
    private bool boldText;

    /// <summary>
    /// Use a smaller font for the Human Readable Text.
    /// Corresponds to Zint's --small flag.
    /// </summary>
    [ObservableProperty]
    private bool smallText;

    /// <summary>
    /// Gap between barcode and text in X-dimensions.
    /// Corresponds to Zint's --textgap flag.
    /// </summary>
    [ObservableProperty]
    private double? textGap;

    /// <summary>
    /// Embed the font in the vector output (SVG only).
    /// Corresponds to Zint's --embedfont flag.
    /// </summary>
    [ObservableProperty]
    private bool embedFont;

    // -- SPECIAL FLAGS --

    /// <summary>
    /// If true, processes tilde (~) characters for advanced formatting.
    /// Corresponds to Zint's --esc flag.
    /// </summary>
    [ObservableProperty]
    private bool processTilde = false;

    /// <summary>
    /// If true, marks the data as GS1 data.
    /// Corresponds to Zint's --gs1 flag.
    /// </summary>
    [ObservableProperty]
    private bool isGs1Data = false;

    /// <summary>
    /// Forces a square symbol for matrix codes like QR, DataMatrix, Aztec.
    /// Corresponds to Zint's --square flag.
    /// </summary>
    [ObservableProperty]
    private bool forceSquare = false;

    /// <summary>
    /// Treat input data as raw 8-bit binary data.
    /// Corresponds to Zint's --binary flag.
    /// </summary>
    [ObservableProperty]
    private bool binaryMode;

    /// <summary>
    /// The Extended Channel Interpretation (ECI) code.
    /// Corresponds to Zint's --eci flag.
    /// </summary>
    [ObservableProperty]
    private int? eci;

    /// <summary>
    /// Controls the quiet zones around the barcode.
    /// Corresponds to Zint's --quietzones and --noquietzones flags.
    /// </summary>
    [ObservableProperty]
    private bool? quietZones;

    /// <summary>
    /// For Data Matrix in GS1 mode, use GS as the separator.
    /// Corresponds to Zint's --gssep flag.
    /// </summary>
    [ObservableProperty]
    private bool gs1Separator;

    /// <summary>
    /// A catch-all for any symbology-specific or advanced Zint commands.
    /// Example: "--vers=10 --secure=2" for a QR Code.
    /// </summary>
    [ObservableProperty]
    private string? advancedOptions;

    public NewBarcode(string data, Symbologies symbology)
    {
        this.data = data;
        this.symbology = symbology;
    }
}