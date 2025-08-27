using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace Zint.CLI
{
    public class Switches
    {
        private string commandLine;
        public Switches(string zintPath) => commandLine = zintPath ?? string.Empty;
        public Switches() => commandLine = string.Empty;
        public override string ToString() => commandLine;

        //4.1 Inputting Data

        /// <summary>
        /// The data to encode can be entered at the command line using the -d or --data
        /// option, for example
        /// 
        ///     zint -d "This Text"
        /// 
        /// This will encode the text "This Text". Zint will use the default symbology, Code
        /// 128, and output to the default file "out.png" in the current directory.
        /// Alternatively, if libpng was not present when Zint was built, the default output
        /// file will be "out.gif".
        /// 
        /// The data input to the Zint CLI is assumed to be encoded in UTF-8 (Unicode)
        /// format (Zint will correctly handle UTF-8 data on Windows). If you are encoding
        /// characters beyond the 7-bit ASCII set using a scheme other than UTF-8 then you
        /// will need to set the appropriate input options as shown in 4.11 Input Modes
        /// below.
        /// 
        /// Non-printing characters can be entered on the command line using backslash (\)
        /// as an escape character in combination with the --esc switch. Permissible
        /// sequences are shown in the table below.
        /// 
        ///   ----------------------------------------------------------------------------
        ///   Escape      ASCII        Name    Interpretation
        ///   Sequence    Equivalent
        ///   ----------- ------------ ------- -------------------------------------------
        ///   \0          0x00         NUL     Null character
        /// 
        ///   \E          0x04         EOT     End of Transmission
        /// 
        ///   \a          0x07         BEL     Bell
        /// 
        ///   \b          0x08         BS      Backspace
        /// 
        ///   \t          0x09         HT      Horizontal Tab
        /// 
        ///   \n          0x0A         LF      Line Feed
        /// 
        ///   \v          0x0B         VT      Vertical Tab
        /// 
        ///   \f          0x0C         FF      Form Feed
        /// 
        ///   \r          0x0D         CR      Carriage Return
        /// 
        ///   \e          0x1B         ESC     Escape
        /// 
        ///   \G          0x1D         GS      Group Separator
        /// 
        ///   \R          0x1E         RS      Record Separator
        /// 
        ///   \\          0x5C         \       Backslash
        /// 
        ///   \dNNN       NNN                  Any 8-bit character where NNN is decimal
        ///                                    (000-255)
        /// 
        ///   \oNNN       0oNNN                Any 8-bit character where NNN is octal
        ///                                    (000-377)
        /// 
        ///   \xNN        0xNN                 Any 8-bit character where NN is hexadecimal
        ///                                    (00-FF)
        /// 
        ///   \uNNNN                           Any 16-bit Unicode BMP[2] character where
        ///                                    NNNN is hexadecimal (0000-FFFF)
        /// 
        ///   \UNNNNNN                         Any 21-bit Unicode character where NNNNNN
        ///                                    is hexadecimal (000000-10FFFF)
        ///   ----------------------------------------------------------------------------
        /// 
        ///   : Table : Escape Sequences:
        /// 
        /// (Special escape sequences are available for Code 128 only to manually switch
        /// Code Sets - see 6.1.10.1 Standard Code 128 (ISO 15417) for details.)
        /// </summary>
        /// <param name="data"></param>
        /// <returns> --data=\"{data}\"</returns>
        public Switches Data(string data) { commandLine += $" --data=\"{data}\""; return this; }

        /// <summary>
        /// Input data can be read directly from file using the -i or --input switch as
        /// shown below. The input file is assumed to be UTF-8 formatted unless an
        /// alternative mode is selected. This command replaces the use of the -d switch.
        /// 
        ///     zint -i somefile.txt
        /// 
        /// To read from stdin specify a single hyphen "-" as the input file.
        /// 
        /// Note that except when batch processing (see 4.12 Batch Processing below), the
        /// file (or stdin) should not end with a newline (LF on Unix, CR+LF on Windows)
        /// unless you want the newline to be encoded in the symbol.
        /// </summary>
        /// <param name="path"></param>
        /// <returns> --input=\"{path}\"</returns>
        public Switches Input(string path) { commandLine += $" --input=\"{path}\""; return this; }

        //4.2 Directing Output

        /// <summary>
        /// Output can be directed to a file other than the default using the -o or --output
        /// switch. For example:
        /// 
        ///     zint -o here.png -d "This Text"
        /// 
        /// This draws a Code 128 barcode in the file "here.png". If an Encapsulated
        /// PostScript file is needed simply append the filename with ".eps", and so on for
        /// the other supported file types:
        /// 
        ///     zint -o there.eps -d "This Text"
        /// 
        /// The currently supported output file formats are shown in the following table.
        /// 
        ///   Extension   File format
        ///   ----------- ------------------------------------
        ///   bmp         Windows Bitmap
        ///   emf         Enhanced Metafile Format
        ///   eps         Encapsulated PostScript
        ///   gif         Graphics Interchange Format
        ///   pcx         ZSoft Paintbrush image
        ///   png         Portable Network Graphic
        ///   svg         Scalable Vector Graphic
        ///   tif         Tagged Image File Format
        ///   txt         Text file (see 4.19 Other Options)
        /// 
        ///   : Table : Output File Formats:
        /// 
        /// The filename can contain directories and sub-directories also, which will be
        /// created if they don’t already exist:
        /// 
        ///     zint -o "dir/subdir/filename.eps" -d "This Text"
        /// 
        /// Note that on Windows, filenames are assumed to be UTF-8 encoded.
        /// </summary>
        /// <param name="path"></param>
        /// <returns> --output=\"{path}\"</returns>
        public Switches Output(string path) { commandLine += $" --output=\"{path}\""; return this; }

        //4.3 Selecting Symbology

        /// <summary>
        /// Selecting which type of barcode you wish to produce (i.e. which symbology to
        /// use) can be done at the command line using the -b or --barcode switch followed
        /// by the appropriate integer value or name in the following table. For example to
        /// create a Data Matrix symbol you could use:
        /// 
        ///     zint -b 71 -o datamatrix.png -d "Data to encode"
        /// 
        /// or
        /// 
        ///     zint -b DATAMATRIX -o datamatrix.png -d "Data to encode"
        /// 
        /// Names are treated case-insensitively by the CLI, and the BARCODE_ prefix and any
        /// underscores are optional.
        /// </summary>
        /// <param name="path"></param>
        /// <returns> --barcode=\"{type}\"</returns>
        public Switches Barcode(Symbologies type) { commandLine += $" --barcode={(int)type}"; return this; }

        //4.4 Adjusting Height

        /// <summary>
        /// The height of a symbol (except those with a fixed width-to-height ratio) can be
        /// adjusted using the --height switch. For example:
        ///
        ///    zint --height=100 -d "This Text"
        ///
        /// This specifies a symbol height of 100 times the X-dimension of the symbol.
        /// </summary>
        /// <param name="height"></param>
        /// <returns> --height={height}</returns>
        public Switches Height(double height) { commandLine += $" --height={height}"; return this; }

        /// <summary>
        /// The default height of most linear barcodes is 50.0X, but this can be changed for
        /// barcodes whose specifications give a standard height by using the switch
        /// --compliantheight. For instance
        ///
        ///    zint -b LOGMARS -d "This Text" --compliantheight
        ///
        /// will produce a barcode of height 45.455X instead of the normal default of 50.0X.
        /// The flag also causes Zint to return a warning if a non-compliant height is
        /// given:
        ///
        ///    zint -b LOGMARS -d "This Text" --compliantheight --height=6.2
        ///    Warning 247: Height not compliant with standards
        /// </summary>
        /// <returns> --compliantheight</returns>
        public Switches CompliantHeight() { commandLine += $" --compliantheight"; return this; }

        /// <summary>
        /// Another switch is --heightperrow, which can be useful for symbologies that have
        /// a variable number of linear rows, namely Codablock-F, Code 16K, Code 49, GS1
        /// DataBar Expanded Stacked, MicroPDF417 and PDF417, as it changes the treatment of
        /// the height value from overall height to per-row height, allowing you to specify
        /// a consistent height for each linear row without having to know how many there
        /// are.For instance
        ///
        ///    zint -b PDF417 -d "This Text" --height=4 --heightperrow
        ///
        /// will produce a barcode of height 32X, with each of the 8 rows 4X high.
        /// </summary>
        /// <returns> --heightperrow</returns>
        public Switches HeightPerRow() { commandLine += $" --heightperrow"; return this; }

        //4.5 Adjusting Whitespace

        /// <summary>
        /// The amount of horizontal whitespace to the left and right of the generated
        /// barcode can be altered using the -w or --whitesp switch, in integral multiples
        /// of the X-dimension.For example:
        ///
        ///     zint -w 10 -d "This Text"
        ///
        /// This specifies a whitespace width of 10 times the X-dimension of the symbol both
        /// to the left and to the right of the barcode.
        /// 
        /// Horizontal and vertical whitespace can of course be used together:
        ///
        /// zint -b DATAMATRIX --whitesp=1 --vwhitesp=1 -d "This Text"
        ///
        /// A --quietzones option is also available which adds quiet zones compliant with
        /// the symbology’s specification.This is in addition to any whitespace specified
        /// with the --whitesp or --vwhitesp switches.
        ///
        /// Note that Codablock-F, Code 16K, Code 49, ITF-14, EAN-2 to EAN-13, ISBN, UPC-A
        /// and UPC-E have compliant quiet zones added by default. This can be disabled with
        /// the option --noquietzones.
        /// </summary>
        /// <param name="whitespace"></param>
        /// <returns> --whitesp={whitespace}</returns>
        public Switches Whitespace(int whitespace) { commandLine += $" --whitesp={whitespace}"; return this; }
        /// <summary>
        /// The amount of vertical whitespace above and below the barcode can be altered
        /// using the --vwhitesp switch, in integral multiples of the X-dimension.For
        /// example for 3 times the X-dimension:
        ///
        ///     zint --vwhitesp=3 -d "This Text"
        ///
        /// Note that the whitespace at the bottom appears below the text, if any.
        /// 
        /// Horizontal and vertical whitespace can of course be used together:
        ///
        /// zint -b DATAMATRIX --whitesp=1 --vwhitesp=1 -d "This Text"
        ///
        /// A --quietzones option is also available which adds quiet zones compliant with
        /// the symbology’s specification.This is in addition to any whitespace specified
        /// with the --whitesp or --vwhitesp switches.
        ///
        /// Note that Codablock-F, Code 16K, Code 49, ITF-14, EAN-2 to EAN-13, ISBN, UPC-A
        /// and UPC-E have compliant quiet zones added by default. This can be disabled with
        /// the option --noquietzones.
        /// </summary>
        /// <param name="whitespace"></param>
        /// <returns> --vwhitesp={whitespace}</returns>
        public Switches VWhitespace(int whitespace) { commandLine += $" --vwhitesp={whitespace}"; return this; }

        //4.6 Adding Boundary Bars and Boxes

        /// <summary>
        /// Zint allows the symbol to be bound with ‘boundary bars’ (also known as ‘bearer
        /// bars’) using the option --bind. These bars help to prevent misreading of the
        /// symbol by corrupting a scan if the scanning beam strays off the top or bottom of
        /// the symbol.
        /// </summary>
        /// <returns> --bind</returns>
        public Switches Bind() { commandLine += " --bind"; return this; }

        /// <summary>
        /// Zint can also put a border right around the symbol and its
        /// horizontal whitespace with the --box option.
        /// </summary>
        /// <returns> --box</returns>
        public Switches Box() { commandLine += " --box"; return this; }

        /// <summary>
        /// The width of the boundary bars or box borders, in integral multiples of the
        /// X-dimension, must be specified using the --border switch. For example:
        ///
        ///     zint --box --border=10 -w 10 -d "This Text"
        ///
        /// gives a box with a width 10 times the X-dimension of the symbol. Note that when
        /// specifying a box, horizontal whitespace is usually required in order to create a
        /// quiet zone between the barcode and the sides of the box.
        /// </summary>
        /// <param name="width">Width in X-dimensions</param>
        /// <returns> --border={width}</returns>
        public Switches Border(int width) { commandLine += $" --border={width}"; return this; }

        /// <summary>
        /// To add a boundary bar to the top only use --bindtop.
        /// </summary>
        /// <returns> --bindtop</returns>
        public Switches BindTop() { commandLine += " --bindtop"; return this; }

        //4.7 Using Colour

        /// <summary>
        /// The -r or --reverse switch allows the default colours
        /// to be inverted so that a white symbol is shown on a black background (known as
        /// “reflectance reversal” or “reversed reflectance”).
        /// </summary>
        /// <returns> --reverse</returns>
        public Switches Reverse() { commandLine += " --reverse"; return this; }

        /// <summary>
        /// The foreground (ink) colour can be specified using the --fg option followed by a
        /// number in "RRGGBB" or "RRGGBBAA" hexadecimal notation or in "C,M,Y,K" decimal
        /// percentages format.
        /// </summary>
        /// <param name="colour">Colour string</param>
        /// <returns> --fg={colour}</returns>
        public Switches Foreground(string colour) { commandLine += $" --fg={colour}"; return this; }

        /// <summary>
        /// The background (paper) colour can be specified using the --bg option followed by a
        /// number in "RRGGBB" or "RRGGBBAA" hexadecimal notation or in "C,M,Y,K" decimal
        /// percentages format.
        /// </summary>
        /// <param name="colour">Colour string</param>
        /// <returns> --bg={colour}</returns>
        public Switches Background(string colour) { commandLine += $" --bg={colour}"; return this; }

        /// <summary>
        /// The --nobackground option will remove the background from all output
        /// formats except BMP.
        /// </summary>
        /// <returns> --nobackground</returns>
        public Switches NoBackground() { commandLine += " --nobackground"; return this; }

        /// <summary>
        /// The --cmyk option is specific to output in Encapsulated PostScript (EPS) and
        /// TIF, and selects the CMYK colour space.
        /// </summary>
        /// <returns> --cmyk</returns>
        public Switches Cmyk() { commandLine += " --cmyk"; return this; }

        //4.8 Rotating the Symbol

        /// <summary>
        /// The symbol can be rotated through four orientations using the --rotate option
        /// followed by the angle of rotation as shown below.
        /// 
        /// --rotate=0 (default)
        /// --rotate=90
        /// --rotate=180
        /// --rotate=270
        ///
        /// zint -d "This Text" --rotate=90
        /// 
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns> --rotate={degrees}</returns>
        public Switches Rotate(int degrees) { commandLine += $" --rotate={degrees}"; return this; }

        //4.9 Adjusting Image Size (X-dimension)

        /// <summary>
        /// The size of the image can be altered using the --scale option, which sets the
        /// X-dimension. The default scale is 1.0.
        /// </summary>
        /// <param name="scale">The scaling factor.</param>
        /// <returns> --scale={scale}</returns>
        public Switches Scale(double scale) { commandLine += $" --scale={scale}"; return this; }

        /// <summary>
        // An alternative way to specify the scale is to specify measurable units using the --scalexdimdp option.
        /// </summary>
        /// <param name="xDimension">X-dimension in mm or in.</param>
        /// <param name="resolution">Resolution in dpmm or dpi.</param>
        /// <returns>--scalexdimdp={xDimension},{resolution}</returns>
        public Switches ScaleXDimDp(string xDimension, string resolution) { commandLine += $" --scalexdimdp={xDimension},{resolution}"; return this; }

        //4.10 Human Readable Text (HRT) Options

        /// <summary>
        /// For linear barcodes the text present in the output image can be removed by
        /// using the --notext option.
        /// </summary>
        /// <returns> --notext</returns>
        public Switches NoText() { commandLine += " --notext"; return this; }

        /// <summary>
        /// Text can be set to bold using the --bold option.
        /// </summary>
        /// <returns> --bold</returns>
        public Switches Bold() { commandLine += " --bold"; return this; }

        /// <summary>
        /// A smaller font can be substituted using the --small option.
        /// </summary>
        /// <returns> --small</returns>
        public Switches Small() { commandLine += " --small"; return this; }

        /// <summary>
        /// The gap between the barcode and the text can be adjusted using the --textgap
        /// option, where the gap is given in X-dimensions.
        /// </summary>
        /// <param name="gap">Gap in X-dimensions (-5.0 to 10.0)</param>
        /// <returns> --textgap={gap}</returns>
        public Switches TextGap(double gap) { commandLine += $" --textgap={gap}"; return this; }

        /// <summary>
        /// For SVG output, the font can be embedded in the file for portability using the
        /// --embedfont option.
        /// </summary>
        /// <returns> --embedfont</returns>
        public Switches EmbedFont() { commandLine += " --embedfont"; return this; }

        //4.11 Input Modes

        /// <summary>
        /// To encode GS1 data use the --gs1 option.
        /// </summary>
        /// <returns> --gs1</returns>
        public Switches GS1() { commandLine += " --gs1"; return this; }

        /// <summary>
        /// The --binary option encodes the input data as given.
        /// </summary>
        /// <returns> --binary</returns>
        public Switches Binary() { commandLine += " --binary"; return this; }

        /// <summary>
        /// The --fullmultibyte option uses the multibyte modes of QR Code, Micro QR Code,
        /// Rectangular Micro QR Code, Han Xin Code and Grid Matrix for non-ASCII data,
        /// maximizing density.
        /// </summary>
        /// <returns> --fullmultibyte</returns>
        public Switches FullMultibyte() { commandLine += " --fullmultibyte"; return this; }

        /// <summary>
        /// The ECI value may be specified with the --eci switch.
        /// </summary>
        /// <param name="eci">ECI value</param>
        /// <returns> --eci={eci}</returns>
        public Switches Eci(int eci) { commandLine += $" --eci={eci}"; return this; }

        //4.12 Batch Processing

        /// <summary>
        /// Data can be batch processed by reading from a text file and producing a separate
        /// barcode image for each line of text in that file. To do this use the --batch
        /// switch together with -i to select the input file from which to read data.
        /// </summary>
        /// <returns> --batch</returns>
        public Switches Batch() { commandLine += " --batch"; return this; }

        /// <summary>
        /// The --mirror option instructs Zint to use the data to be encoded as an indicator
        /// of the filename to be used. This is particularly useful if you are processing
        /// batch data.
        /// </summary>
        /// <returns> --mirror</returns>
        public Switches Mirror() { commandLine += " --mirror"; return this; }

        //4.13 Direct Output to stdout

        /// <summary>
        /// The finished image files can be output directly to stdout for use as part of a
        /// pipe by using the --direct option.By default --direct will output data as a PNG
        /// image (or GIF image if libpng is not present), but this can be altered by
        /// supplementing the --direct option with a --filetype option followed by the
        /// suffix of the file type required.For example:
        ///
        ///     zint -b 84 --direct --filetype= pcx - d "Data to encode"
        ///
        /// This command will output the symbol as a PCX file to stdout.For the supported
        /// output file formats see Table : Output File Formats.
        ///
        /// --------------------------------------------------------------------------------
        ///
        /// CAUTION: Outputting binary files to the command shell without catching that data
        /// in a pipe can have unpredictable results. Use with care!
        ///
        /// --------------------------------------------------------------------------------
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns> --direct --filetype={fileType}</returns>
        public Switches DirectStdout(string fileType = "png") { commandLine += $" --direct --filetype={fileType}"; return this; }

        //4.14 Automatic Filenames

        /// <summary>
        /// To set the output file format use the --filetype option.
        /// </summary>
        /// <param name="fileType">File type extension (png, svg, etc.)</param>
        /// <returns> --filetype={fileType}</returns>
        public Switches FileType(string fileType) { commandLine += $" --filetype={fileType}"; return this; }

        //4.15 Working with Dots

        /// <summary>
        /// Matrix codes can be rendered as a series of dots or circles rather than the
        /// normal squares by using the --dotty option.
        /// </summary>
        /// <returns> --dotty</returns>
        public Switches Dotty() { commandLine += " --dotty"; return this; }

        /// <summary>
        /// The size of the dots can be adjusted using the --dotsize option followed by the
        /// diameter of the dot, where that diameter is in X-dimensions.
        /// </summary>
        /// <param name="size">Dot diameter in X-dimensions (0.01 to 20)</param>
        /// <returns> --dotsize={size}</returns>
        public Switches DotSize(double size) { commandLine += $" --dotsize={size}"; return this; }

        //4.16 Multiple Segments

        /// <summary>
        /// If you need to specify different ECIs for different sections of the input data,
        /// the --seg1 to --seg9 options can be used. Each option is of the form
        /// --segN=ECI,data where ECI is the ECI code and data is
        /// the data to which this applies.
        /// </summary>
        /// <param name="segmentNumber">Segment number (1-9)</param>
        /// <param name="eci">ECI code</param>
        /// <param name="data">Data for the segment</param>
        /// <returns> --seg{segmentNumber}={eci},"{data}"</returns>
        public Switches Segment(int segmentNumber, int eci, string data) { commandLine += $" --seg{segmentNumber}={eci},\"{data}\""; return this; }

        //4.17 Structured Append

        /// <summary>
        /// The --structapp option marks a symbol as part of a Structured Append sequence,
        /// and has the format --structapp=I,C[,ID]
        /// </summary>
        /// <param name="index">Position of the symbol in the sequence (1-based)</param>
        /// <param name="count">Total number of symbols in the sequence</param>
        /// <param name="id">Optional identifier</param>
        /// <returns> --structapp={index},{count},{id}</returns>
        public Switches StructAppend(int index, int count, string id = null)
        {
            commandLine += $" --structapp={index},{count}";
            if (!string.IsNullOrEmpty(id))
            {
                commandLine += $",{id}";
            }
            return this;
        }

        // Other methods from original file
        public Switches QuietZones(bool? quitZones) { commandLine += quitZones.HasValue ? quitZones.Value ? $" --quietzones" : $" --noquietzones" : string.Empty; return this; }

        /// <summary>
        /// Process input data for escape sequences.
        /// </summary>
        /// <returns> --esc</returns>
        public Switches EscapeInput() { commandLine += $" --esc"; return this; }

        /// <summary>
        /// For Data Matrix in GS1 mode, use GS (0x1D) as the GS1 data separator instead of FNC1.
        /// </summary>
        /// <returns> --gssep</returns>
        public Switches Gs1Separator() { commandLine += " --gssep"; return this; }

        public Switches Symbol_DataMatrix(TriState shape) { commandLine += shape == TriState.Auto ? string.Empty : shape == TriState.On ? $" --square" : " --dmre"; return this; }

    }
}