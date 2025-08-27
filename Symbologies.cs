using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zint.CLI;

/// <summary>
/// Represents an exhaustive list of barcode symbologies supported by the Zint library.
/// The integer value of each member corresponds to the Zint internal symbology ID.
/// </summary>
public enum Symbologies
{
    // -- Linear Barcodes --

    /// <summary>Code 11</summary>
    Code11 = 1,
    /// <summary>Standard Code 2 of 5</summary>
    Code2Of5Standard = 2,
    /// <summary>Interleaved Code 2 of 5</summary>
    Code2Of5Interleaved = 3,
    /// <summary>IATA Code 2 of 5</summary>
    Code2Of5Iata = 4,
    /// <summary>Data Logic Code 2 of 5</summary>
    Code2Of5DataLogic = 6,
    /// <summary>Industrial Code 2 of 5</summary>
    Code2Of5Industrial = 7,
    /// <summary>Code 39 (Code 3 of 9)</summary>
    Code39 = 8,
    /// <summary>Extended Code 39</summary>
    ExtendedCode39 = 9,
    /// <summary>EAN (EAN-13 and EAN-8)</summary>
    Ean = 13,
    /// <summary>EAN with check digit</summary>
    EanWithChecksum = 14,
    /// <summary>Codabar</summary>
    Codabar = 18,
    /// <summary>Code 128</summary>
    Code128 = 20,
    /// <summary>Code 128 subset B</summary>
    Code128B = 60,
    /// <summary>NVE-18 (SSCC-18)</summary>
    Nve18 = 73,
    /// <summary>Code 16k</summary>
    Code16k = 23,
    /// <summary>Code 49</summary>
    Code49 = 24,
    /// <summary>Code 93</summary>
    Code93 = 25,
    /// <summary>UPC-A</summary>
    UpcA = 34,
    /// <summary>UPC-A with check digit</summary>
    UpcAWithChecksum = 35,
    /// <summary>UPC-E</summary>
    UpcE = 37,
    /// <summary>UPC-E with check digit</summary>
    UpcEWithChecksum = 38,
    /// <summary>MSI Plessey</summary>
    MsiPlessey = 44,
    /// <summary>Plessey</summary>
    Plessey = 85,
    /// <summary>LOGMARS</summary>
    Logmars = 49,
    /// <summary>Pharmacode One-Track</summary>
    Pharmacode = 50,
    /// <summary>PZN-8</summary>
    Pzn8 = 51,
    /// <summary>Pharmacode Two-Track</summary>
    PharmacodeTwoTrack = 53,
    /// <summary>ITF-14</summary>
    Itf14 = 89,
    /// <summary>Vehicle Identification Number (VIN)</summary>
    Vin = 149,
    /// <summary>Telepen Alpha</summary>
    Telepen = 32,
    /// <summary>Telepen Numeric</summary>
    TelepenNumeric = 87,
    /// <summary>Flat</summary>
    Flat = 28,

    // -- GS1 DataBar (formerly RSS) --

    /// <summary>GS1 DataBar Omnidirectional / RSS-14</summary>
    Gs1DataBar = 29,
    /// <summary>GS1 DataBar Limited / RSS Limited</summary>
    Gs1DataBarLimited = 30,
    /// <summary>GS1 DataBar Expanded / RSS Expanded</summary>
    Gs1DataBarExpanded = 31,
    /// <summary>GS1 DataBar Stacked</summary>
    Gs1DataBarStacked = 76,
    /// <summary>GS1 DataBar Stacked Omnidirectional</summary>
    Gs1DataBarStackedOmnidirectional = 77,
    /// <summary>GS1 DataBar Expanded Stacked</summary>
    Gs1DataBarExpandedStacked = 79,

    // -- GS1-128 and EAN-128 --

    /// <summary>EAN-128</summary>
    Ean128 = 16,
    /// <summary>GS1-128</summary>
    Gs1_128 = 128,

    // -- Postal Barcodes --

    /// <summary>Postnet</summary>
    Postnet = 40,
    /// <summary>PLANET</summary>
    Planet = 80,
    /// <summary>FIM (Facing Identification Mark)</summary>
    Fim = 47,
    /// <summary>USPS Intelligent Mail (OneCode)</summary>
    UspsIntelligentMail = 84,
    /// <summary>Royal Mail 4-State Customer Code (RM4SCC)</summary>
    RoyalMail4State = 70,
    /// <summary>Royal Mail Mailmark 2D</summary>
    RoyalMailMailmark2D = 121,
    /// <summary>Deutsche Post Leitcode</summary>
    DeutschePostLeitcode = 21,
    /// <summary>Deutsche Post Identcode</summary>
    DeutschePostIdentcode = 22,
    /// <summary>Australia Post Standard Customer</summary>
    AustraliaPost = 63,
    /// <summary>Australia Post Reply Paid</summary>
    AustraliaPostReplyPaid = 66,
    /// <summary>Australia Post Routing</summary>
    AustraliaPostRouting = 67,
    /// <summary>Australia Post Redirect</summary>
    AustraliaPostRedirect = 68,
    /// <summary>Japan Post</summary>
    JapanPost = 74,
    /// <summary>Korea Post</summary>
    KoreaPost = 75,
    /// <summary>KIX Code</summary>
    KixCode = 90,
    /// <summary>DAFT Code</summary>
    DaftCode = 93,

    // -- 2D Matrix Barcodes --

    /// <summary>PDF417</summary>
    Pdf417 = 55,
    /// <summary>Truncated PDF417</summary>
    Pdf417Truncated = 56,
    /// <summary>MicroPDF417</summary>
    MicroPdf417 = 82,
    /// <summary>MaxiCode</summary>
    MaxiCode = 57,
    /// <summary>QR Code</summary>
    QrCode = 58,
    /// <summary>Micro QR Code</summary>
    MicroQrCode = 97,
    /// <summary>Rectangular Micro QR Code (rMQR)</summary>
    RectangularMicroQrCode = 144,
    /// <summary>Data Matrix</summary>
    DataMatrix = 71,
    /// <summary>Aztec Code</summary>
    AztecCode = 92,
    /// <summary>Aztec Runes</summary>
    AztecRunes = 114,
    /// <summary>DotCode</summary>
    DotCode = 115,
    /// <summary>Han Xin Code</summary>
    HanXinCode = 116,
    /// <summary>Ultracode</summary>
    Ultracode = 143,
    /// <summary>Codablock-F</summary>
    CodablockF = 152,

    // -- Other & Stacked Barcodes --

    /// <summary>ISBN (from EAN-13)</summary>
    Isbn = 69,
    /// <summary>EAN-14</summary>
    Ean14 = 72,
    /// <summary>Channel Code</summary>
    ChannelCode = 147,
    /// <summary>BC412</summary>
    Bc412 = 145,
    /// <summary>Digimarc</summary>
    Digimarc = 150,

    // -- HIBC (Health Industry Bar Code) Variants --

    /// <summary>HIBC Code 128</summary>
    HibcCode128 = 98,
    /// <summary>HIBC Code 39</summary>
    HibcCode39 = 99,
    /// <summary>HIBC Data Matrix</summary>
    HibcDataMatrix = 102,
    /// <summary>HIBC QR Code</summary>
    HibcQrCode = 104,
    /// <summary>HIBC PDF417</summary>
    HibcPdf417 = 106,
    /// <summary>HIBC MicroPDF417</summary>
    HibcMicroPdf417 = 108,
    /// <summary>HIBC Codablock-F</summary>
    HibcBlockCodeF = 110,
    /// <summary>HIBC Aztec Code</summary>
    HibcAztecCode = 112,

    // -- GS1 Composite Symbologies --

    /// <summary>GS1-128 Composite</summary>
    Gs1_128_Composite = 129,
    /// <summary>GS1 DataBar Omnidirectional Composite</summary>
    Gs1DataBar_Composite = 130,
    /// <summary>GS1 DataBar Limited Composite</summary>
    Gs1DataBarLimited_Composite = 131,
    /// <summary>GS1 DataBar Expanded Composite</summary>
    Gs1DataBarExpanded_Composite = 132,
    /// <summary>UPC-A Composite</summary>
    UpcA_Composite = 133,
    /// <summary>UPC-E Composite</summary>
    UpcE_Composite = 134,
    /// <summary>GS1 DataBar Stacked Composite</summary>
    Gs1DataBarStacked_Composite = 135,
    /// <summary>GS1 DataBar Stacked Omnidirectional Composite</summary>
    Gs1DataBarStackedOmni_Composite = 136,
    /// <summary>GS1 DataBar Expanded Stacked Composite</summary>
    Gs1DataBarExpandedStacked_Composite = 137
}
