
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
        public Switches Barcode(Symbologies type) { commandLine += $" --barcode=\"{type}\""; return this; }

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
        public Switches Height(int height) { commandLine += $" --height={height}"; return this; }

        /// <summary>
        /// The default height of most linear barcodes is 50X, but this can be changed for
        /// barcodes whose specifications give a standard height by using the switch
        /// --compliantheight. For instance
        ///
        ///    zint -b LOGMARS -d "This Text" --compliantheight
        ///
        /// will produce a barcode of height 45.455X instead of the normal default of 50X.
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

        //4.7 Using Colour

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

        public Switches XDimensionMM(double xDimensionMM, int dpi) { commandLine += $" --scalexdimdp={xDimensionMM:F2}mm,{dpi}dpi"; return this; }
        public Switches XDimensionMils(double xDimensionMils, int dpi) { commandLine += $" --scalexdimdp={xDimensionMils:F4}in,{dpi}dpi"; return this; }
        public Switches XDimensionPixels(double xDimensionPixels) { commandLine += $" --scale={xDimensionPixels / 2}"; return this; }
        public Switches XDimensionScale(double scale) { commandLine += $" --scale={scale}"; return this; }

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

        public Switches QueitZones(bool? state) {  commandLine += state == null ? string.Empty : state == true ? $" --quietzones" : $" --noquietzones"; return this; }

        public Switches GS1() { commandLine += $" --gs1 --gs1parens"; return this; }

        public Switches EscapeInput() { commandLine += $" --esc --gssep"; return this; }

        public Switches Symbol_DataMatrix(bool? shape) { commandLine += shape == null ? string.Empty : shape == true ? $" --square" : " --dmre"; return this; }

    }
}
