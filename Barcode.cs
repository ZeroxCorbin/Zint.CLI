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
public partial class BarcodeSettings : ObservableObject
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

    /// <summary>
    /// For composite symbols, this holds the data for the linear component.
    /// Corresponds to Zint's --primary flag.
    /// </summary>
    [ObservableProperty]
    private string? primaryData;

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
    /// An alternative way to specify scale using X-dimension and resolution.
    /// Example: "0.33mm,300dpi".
    /// Corresponds to Zint's --scalexdimdp option.
    /// </summary>
    [ObservableProperty]
    private string? scaleXDimDp;

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

    /// <summary>
    /// Renders matrix codes as dots instead of squares.
    /// Corresponds to Zint's --dotty option.
    /// </summary>
    [ObservableProperty]
    private bool dottyMode;

    /// <summary>
    /// The diameter of the dots in dotty mode, in X-dimensions.
    /// Corresponds to Zint's --dotsize option.
    /// </summary>
    [ObservableProperty]
    private double? dotSize;

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
    /// For EAN/UPC, sets the gap between the main symbol and the add-on.
    /// Corresponds to Zint's --addongap option.
    /// </summary>
    [ObservableProperty]
    private int? addOnGap;

    /// <summary>
    /// For EAN/UPC, adds quiet zone indicators ('<', '>') to the human-readable text.
    /// Corresponds to Zint's --guardwhitespace option.
    /// </summary>
    [ObservableProperty]
    private bool guardWhitespace;

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
    /// For GS1 data, allows using parentheses () as AI delimiters instead of square brackets [].
    /// Corresponds to Zint's --gs1parens option.
    /// </summary>
    [ObservableProperty]
    private bool gs1Parens;

    /// <summary>
    /// For matrix symbols, specifies the version or size.
    /// Corresponds to Zint's --vers option.
    /// </summary>
    [ObservableProperty]
    private int? version;

    /// <summary>
    /// For matrix symbols, sets the error correction level.
    /// Corresponds to Zint's --secure option.
    /// </summary>
    [ObservableProperty]
    private int? securityLevel;

    /// <summary>
    /// For matrix symbols, manually specifies the mask pattern.
    /// Corresponds to Zint's --mask option.
    /// </summary>
    [ObservableProperty]
    private int? mask;

    /// <summary>
    /// For MaxiCode, prefixes the secondary message with a standard header.
    /// Corresponds to Zint's --scmvv option.
    /// </summary>
    [ObservableProperty]
    private int? scmvv;

    /// <summary>
    /// A catch-all for any symbology-specific or advanced Zint commands.
    /// Example: "--vers=10 --secure=2" for a QR Code.
    /// </summary>
    [ObservableProperty]
    private string? advancedOptions;

    /// <summary>
    /// For Codablock-F, DotCode, GS1 DataBar Expanded Stacked, MicroPDF417, and PDF417, sets the number of data columns.
    /// Corresponds to Zint's --cols option.
    /// </summary>
    [ObservableProperty]
    private int? columns;

    /// <summary>
    /// For Codablock-F, Code 16K, Code 49, GS1 DataBar Expanded Stacked, and PDF417, sets the number of rows.
    /// Corresponds to Zint's --rows option.
    /// </summary>
    [ObservableProperty]
    private int? rows;

    /// <summary>
    /// For MaxiCode and GS1 Composite symbols, sets the encoding mode.
    /// Corresponds to Zint's --mode option.
    /// </summary>
    [ObservableProperty]
    private int? mode;

    /// <summary>
    /// For EAN/UPC symbols, sets the height the guard bars descend below the main bars.
    /// Corresponds to Zint's --guarddescent option.
    /// </summary>
    [ObservableProperty]
    private double? guardDescent;

    /// <summary>
    /// For stacked symbologies, sets the height of row separator bars.
    /// Corresponds to Zint's --separator option.
    /// </summary>
    [ObservableProperty]
    private int? separatorHeight;

    /// <summary>
    /// For Data Matrix, use DMRE sizes.
    /// Corresponds to Zint's --dmre option.
    /// </summary>
    [ObservableProperty]
    private bool useDmre;

    /// <summary>
    /// For Data Matrix, use standard ISO/IEC codeword placement for 144x144 symbols.
    /// Corresponds to Zint's --dmiso144 option.
    /// </summary>
    [ObservableProperty]
    private bool useDmIso144;

    /// <summary>
    /// Use faster, less optimal encoding if available.
    /// Corresponds to Zint's --fast option.
    /// </summary>
    [ObservableProperty]
    private bool useFastEncoding;

    /// <summary>
    /// Use multibyte modes for Grid Matrix, Han Xin, and QR Code for non-ASCII data.
    /// Corresponds to Zint's --fullmultibyte option.
    /// </summary>
    [ObservableProperty]
    private bool useFullMultibyte;

    /// <summary>
    /// Create a Reader Initialisation (Programming) symbol.
    /// Corresponds to Zint's --init option.
    /// </summary>
    [ObservableProperty]
    private bool readerInitialization;
}