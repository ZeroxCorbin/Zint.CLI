using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Text.Json.Serialization;


namespace Zint.CLI
{
    [SQLite.StoreAsText]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TriState
    {
        [Description("Off")]
        Off = 0,
        [Description("On")]
        On = 1,
        [Description("Auto")]
        Auto = 2
    }
    //public enum Symbologies
    //{
    //    [Description("Code 11")]
    //    BARCODE_CODE11 = 1,   // Code 11
    //    [Description("2 of 5 Standard (Matrix)")]
    //    BARCODE_C25STANDARD = 2,   // 2 of 5 Standard (Matrix)
    //    [Description("2 of 5 Interleaved")]
    //    BARCODE_C25INTER = 3,   // 2 of 5 Interleaved
    //    [Description("2 of 5 IATA")]
    //    BARCODE_C25IATA = 4,   // 2 of 5 IATA
    //    [Description("2 of 5 Data Logic")]
    //    BARCODE_C25LOGIC = 6,   // 2 of 5 Data Logic
    //    [Description("2 of 5 Industrial")]
    //    BARCODE_C25IND = 7,   // 2 of 5 Industrial
    //    [Description("Code 39")]
    //    BARCODE_CODE39 = 8,   // Code 39
    //    [Description("Extended Code 39")]
    //    BARCODE_EXCODE39 = 9,   // Extended Code 39
    //    [Description("EAN (European Article Number)")]
    //    BARCODE_EANX = 13,  // EAN (European Article Number)
    //    [Description("EAN + Check Digit")]
    //    BARCODE_EANX_CHK = 14,  // EAN + Check Digit
    //    [Description("GS1-128")]
    //    BARCODE_GS1_128 = 16,  // GS1-128
    //    [Description("Codabar")]
    //    BARCODE_CODABAR = 18,  // Codabar
    //    [Description("Code 128")]
    //    BARCODE_CODE128 = 20,  // Code 128
    //    [Description("Deutsche Post Leitcode")]
    //    BARCODE_DPLEIT = 21,  // Deutsche Post Leitcode
    //    [Description("Deutsche Post Identcode")]
    //    BARCODE_DPIDENT = 22,  // Deutsche Post Identcode
    //    [Description("Code 16k")]
    //    BARCODE_CODE16K = 23,  // Code 16k
    //    [Description("Code 49")]
    //    BARCODE_CODE49 = 24,  // Code 49
    //    [Description("Code 93")]
    //    BARCODE_CODE93 = 25,  // Code 93
    //    [Description("Flattermarken")]
    //    BARCODE_FLAT = 28,  // Flattermarken
    //    [Description("GS1 DataBar Omnidirectional")]
    //    BARCODE_DBAR_OMN = 29,  // GS1 DataBar Omnidirectional
    //    [Description("GS1 DataBar Limited")]
    //    BARCODE_DBAR_LTD = 30,  // GS1 DataBar Limited
    //    [Description("GS1 DataBar Expanded")]
    //    BARCODE_DBAR_EXP = 31,  // GS1 DataBar Expanded
    //    [Description("Telepen Alpha")]
    //    BARCODE_TELEPEN = 32,  // Telepen Alpha
    //    [Description("UPC-A")]
    //    BARCODE_UPCA = 34,  // UPC-A
    //    [Description("UPC-A + Check Digit")]
    //    BARCODE_UPCA_CHK = 35,  // UPC-A + Check Digit
    //    [Description("UPC-E")]
    //    BARCODE_UPCE = 37,  // UPC-E
    //    [Description("UPC-E + Check Digit")]
    //    BARCODE_UPCE_CHK = 38,  // UPC-E + Check Digit
    //    [Description("USPS (U.S. Postal Service) POSTNET")]
    //    BARCODE_POSTNET = 40,  // USPS (U.S. Postal Service) POSTNET
    //    [Description("MSI Plessey")]
    //    BARCODE_MSI_PLESSEY = 47,  // MSI Plessey
    //    [Description("Facing Identification Mark")]
    //    BARCODE_FIM = 49,  // Facing Identification Mark
    //    [Description("LOGMARS")]
    //    BARCODE_LOGMARS = 50,  // LOGMARS
    //    [Description("Pharmacode One-Track")]
    //    BARCODE_PHARMA = 51,  // Pharmacode One-Track
    //    [Description("Pharmazentralnummer")]
    //    BARCODE_PZN = 52,  // Pharmazentralnummer
    //    [Description("Pharmacode Two-Track")]
    //    BARCODE_PHARMA_TWO = 53,  // Pharmacode Two-Track
    //    [Description("Brazilian CEPNet Postal Code")]
    //    BARCODE_CEPNET = 54,  // Brazilian CEPNet Postal Code
    //    [Description("PDF417")]
    //    BARCODE_PDF417 = 55,  // PDF417
    //    [Description("Compact PDF417 (Truncated PDF417)")]
    //    BARCODE_PDF417COMP = 56,  // Compact PDF417 (Truncated PDF417)
    //    [Description("MaxiCode")]
    //    BARCODE_MAXICODE = 57,  // MaxiCode
    //    [Description("QR Code")]
    //    BARCODE_QRCODE = 58,  // QR Code
    //    [Description("Code 128 (Suppress Code Set C)")]
    //    BARCODE_CODE128AB = 60,  // Code 128 (Suppress Code Set C)
    //    [Description("Australia Post Standard Customer")]
    //    BARCODE_AUSPOST = 63,  // Australia Post Standard Customer
    //    [Description("Australia Post Reply Paid")]
    //    BARCODE_AUSREPLY = 66,  // Australia Post Reply Paid
    //    [Description("Australia Post Routing")]
    //    BARCODE_AUSROUTE = 67,  // Australia Post Routing
    //    [Description("Australia Post Redirection")]
    //    BARCODE_AUSREDIRECT = 68,  // Australia Post Redirection
    //    [Description("ISBN")]
    //    BARCODE_ISBNX = 69,  // ISBN
    //    [Description("Royal Mail 4-State Customer Code")]
    //    BARCODE_RM4SCC = 70,  // Royal Mail 4-State Customer Code
    //    [Description("Data Matrix (ECC200)")]
    //    BARCODE_DATAMATRIX = 71,  // Data Matrix (ECC200)
    //    [Description("EAN-14")]
    //    BARCODE_EAN14 = 72,  // EAN-14
    //    [Description("Vehicle Identification Number")]
    //    BARCODE_VIN = 73,  // Vehicle Identification Number
    //    [Description("Codablock-F")]
    //    BARCODE_CODABLOCKF = 74,  // Codablock-F
    //    [Description("NVE-18 (SSCC-18)")]
    //    BARCODE_NVE18 = 75,  // NVE-18 (SSCC-18)
    //    [Description("Japanese Postal Code")]
    //    BARCODE_JAPANPOST = 76,  // Japanese Postal Code
    //    [Description("Korea Post")]
    //    BARCODE_KOREAPOST = 77,  // Korea Post
    //    [Description("GS1 DataBar Stacked")]
    //    BARCODE_DBAR_STK = 79,  // GS1 DataBar Stacked
    //    [Description("GS1 DataBar Stacked Omnidirectional")]
    //    BARCODE_DBAR_OMNSTK = 80,  // GS1 DataBar Stacked Omnidirectional
    //    [Description("GS1 DataBar Expanded Stacked")]
    //    BARCODE_DBAR_EXPSTK = 81,  // GS1 DataBar Expanded Stacked
    //    [Description("USPS PLANET")]
    //    BARCODE_PLANET = 82,  // USPS PLANET
    //    [Description("MicroPDF417")]
    //    BARCODE_MICROPDF417 = 84,  // MicroPDF417
    //    [Description("USPS Intelligent Mail (OneCode)")]
    //    BARCODE_USPS_IMAIL = 85,  // USPS Intelligent Mail (OneCode)
    //    [Description("UK Plessey")]
    //    BARCODE_PLESSEY = 86,  // UK Plessey
    //    [Description("Telepen Numeric")]
    //    BARCODE_TELEPEN_NUM = 87,  // Telepen Numeric
    //    [Description("ITF-14")]
    //    BARCODE_ITF14 = 89,  // ITF-14
    //    [Description("Dutch Post KIX Code")]
    //    BARCODE_KIX = 90,  // Dutch Post KIX Code
    //    [Description("Aztec Code")]
    //    BARCODE_AZTEC = 92,  // Aztec Code
    //    [Description("DAFT Code")]
    //    BARCODE_DAFT = 93,  // DAFT Code
    //    [Description("DPD Code")]
    //    BARCODE_DPD = 96,  // DPD Code
    //    [Description("Micro QR Code")]
    //    BARCODE_MICROQR = 97,  // Micro QR Code
    //    [Description("HIBC (Health Industry Barcode) Code 128")]
    //    BARCODE_HIBC_128 = 98,  // HIBC (Health Industry Barcode) Code 128
    //    [Description("HIBC Code 39")]
    //    BARCODE_HIBC_39 = 99,  // HIBC Code 39
    //    [Description("HIBC Data Matrix")]
    //    BARCODE_HIBC_DM = 102, // HIBC Data Matrix
    //    [Description("HIBC QR Code")]
    //    BARCODE_HIBC_QR = 104, // HIBC QR Code
    //    [Description("HIBC PDF417")]
    //    BARCODE_HIBC_PDF = 106, // HIBC PDF417
    //    [Description("HIBC MicroPDF417")]
    //    BARCODE_HIBC_MICPDF = 108, // HIBC MicroPDF417
    //    [Description("HIBC Codablock-F")]
    //    BARCODE_HIBC_BLOCKF = 110, // HIBC Codablock-F
    //    [Description("HIBC Aztec Code")]
    //    BARCODE_HIBC_AZTEC = 112, // HIBC Aztec Code
    //    [Description("DotCode")]
    //    BARCODE_DOTCODE = 115, // DotCode
    //    [Description("Han Xin (Chinese Sensible) Code")]
    //    BARCODE_HANXIN = 116, // Han Xin (Chinese Sensible) Code
    //    [Description("Royal Mail 2D Mailmark (CMDM) (Data Matrix)")]
    //    BARCODE_MAILMARK_2D = 119, // Royal Mail 2D Mailmark (CMDM) (Data Matrix)
    //    [Description("Universal Postal Union S10")]
    //    BARCODE_UPU_S10 = 120, // Universal Postal Union S10
    //    [Description("Royal Mail 4-State Mailmark")]
    //    BARCODE_MAILMARK_4S = 121, // Royal Mail 4-State Mailmark
    //    [Description("Aztec Runes")]
    //    BARCODE_AZRUNE = 128, // Aztec Runes
    //    [Description("Code 32")]
    //    BARCODE_CODE32 = 129, // Code 32
    //    [Description("EAN Composite")]
    //    BARCODE_EANX_CC = 130, // EAN Composite
    //    [Description("GS1-128 Composite")]
    //    BARCODE_GS1_128_CC = 131, // GS1-128 Composite
    //    [Description("GS1 DataBar Omnidirectional Composite")]
    //    BARCODE_DBAR_OMN_CC = 132, // GS1 DataBar Omnidirectional Composite
    //    [Description("GS1 DataBar Limited Composite")]
    //    BARCODE_DBAR_LTD_CC = 133, // GS1 DataBar Limited Composite
    //    [Description("GS1 DataBar Expanded Composite")]
    //    BARCODE_DBAR_EXP_CC = 134, // GS1 DataBar Expanded Composite
    //    [Description("UPC-A Composite")]
    //    BARCODE_UPCA_CC = 135, // UPC-A Composite
    //    [Description("UPC-E Composite")]
    //    BARCODE_UPCE_CC = 136, // UPC-E Composite
    //    [Description("GS1 DataBar Stacked Composite")]
    //    BARCODE_DBAR_STK_CC = 137, // GS1 DataBar Stacked Composite
    //    [Description("GS1 DataBar Stacked Omnidirectional Composite")]
    //    BARCODE_DBAR_OMNSTK_CC = 138, // GS1 DataBar Stacked Omnidirectional Composite
    //    [Description("GS1 DataBar Expanded Stacked Composite")]
    //    BARCODE_DBAR_EXPSTK_CC = 139, // GS1 DataBar Expanded Stacked Composite
    //    [Description("Channel Code")]
    //    BARCODE_CHANNEL = 140, // Channel Code
    //    [Description("Code One")]
    //    BARCODE_CODEONE = 141, // Code One
    //    [Description("Grid Matrix")]
    //    BARCODE_GRIDMATRIX = 142, // Grid Matrix
    //    [Description("UPNQR (Univerzalnega Plačilnega Naloga QR)")]
    //    BARCODE_UPNQR = 143, // UPNQR (Univerzalnega Plačilnega Naloga QR)
    //    [Description("Ultracode")]
    //    BARCODE_ULTRA = 144, // Ultracode
    //    [Description("Rectangular Micro QR Code (rMQR)")]
    //    BARCODE_RMQR = 145, // Rectangular Micro QR Code (rMQR)
    //    [Description("IBM BC412 (SEMI T1-95)")]
    //    BARCODE_BC412 = 146, // IBM BC412 (SEMI T1-95)
    //}
}
