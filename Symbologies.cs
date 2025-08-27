using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [Description("Code11")]
    Code11 = 1,
    /// <summary>Standard Code 2 of 5</summary>
    [Description("Code2Of5Standard")]
    Code2Of5Standard = 2,
    /// <summary>Interleaved Code 2 of 5</summary>
    [Description("Code2Of5Interleaved")]
    Code2Of5Interleaved = 3,
    /// <summary>IATA Code 2 of 5</summary>
    [Description("Code2Of5Iata")]
    Code2Of5Iata = 4,
    /// <summary>Data Logic Code 2 of 5</summary>
    [Description("Code2Of5DataLogic")]
    Code2Of5DataLogic = 6,
    /// <summary>Industrial Code 2 of 5</summary>
    [Description("Code2Of5Industrial")]
    Code2Of5Industrial = 7,
    /// <summary>Code 39 (Code 3 of 9)</summary>
    [Description("Code39")]
    Code39 = 8,
    /// <summary>Extended Code 39</summary>
    [Description("ExtendedCode39")]
    ExtendedCode39 = 9,
    /// <summary>EAN (EAN-13 and EAN-8)</summary>
    [Description("Ean")]
    Ean = 13,
    /// <summary>EAN with check digit</summary>
    [Description("EanWithChecksum")]
    EanWithChecksum = 14,
    /// <summary>Codabar</summary>
    [Description("Codabar")]
    Codabar = 18,
    /// <summary>Code 128</summary>
    [Description("Code128")]
    Code128 = 20,
    /// <summary>Code 128 subset B</summary>
    [Description("Code128B")]
    Code128B = 60,
    /// <summary>NVE-18 (SSCC-18)</summary>
    [Description("Nve18")]
    Nve18 = 73,
    /// <summary>Code 16k</summary>
    [Description("Code16k")]
    Code16k = 23,
    /// <summary>Code 49</summary>
    [Description("Code49")]
    Code49 = 24,
    /// <summary>Code 93</summary>
    [Description("Code93")]
    Code93 = 25,
    /// <summary>UPC-A</summary>
    [Description("UpcA")]
    UpcA = 34,
    /// <summary>UPC-A with check digit</summary>
    [Description("UpcAWithChecksum")]
    UpcAWithChecksum = 35,
    /// <summary>UPC-E</summary>
    [Description("UpcE")]
    UpcE = 37,
    /// <summary>UPC-E with check digit</summary>
    [Description("UpcEWithChecksum")]
    UpcEWithChecksum = 38,
    /// <summary>MSI Plessey</summary>
    [Description("MsiPlessey")]
    MsiPlessey = 44,
    /// <summary>Plessey</summary>
    [Description("Plessey")]
    Plessey = 85,
    /// <summary>LOGMARS</summary>
    [Description("Logmars")]
    Logmars = 49,
    /// <summary>Pharmacode One-Track</summary>
    [Description("Pharmacode")]
    Pharmacode = 50,
    /// <summary>PZN-8</summary>
    [Description("Pzn8")]
    Pzn8 = 51,
    /// <summary>Pharmacode Two-Track</summary>
    [Description("PharmacodeTwoTrack")]
    PharmacodeTwoTrack = 53,
    /// <summary>ITF-14</summary>
    [Description("Itf14")]
    Itf14 = 89,
    /// <summary>Vehicle Identification Number (VIN)</summary>
    [Description("Vin")]
    Vin = 149,
    /// <summary>Telepen Alpha</summary>
    [Description("Telepen")]
    Telepen = 32,
    /// <summary>Telepen Numeric</summary>
    [Description("TelepenNumeric")]
    TelepenNumeric = 87,
    /// <summary>Flat</summary>
    [Description("Flat")]
    Flat = 28,

    // -- GS1 DataBar (formerly RSS) --

    /// <summary>GS1 DataBar Omnidirectional / RSS-14</summary>
    [Description("Gs1DataBar")]
    Gs1DataBar = 29,
    /// <summary>GS1 DataBar Limited / RSS Limited</summary>
    [Description("Gs1DataBarLimited")]
    Gs1DataBarLimited = 30,
    /// <summary>GS1 DataBar Expanded / RSS Expanded</summary>
    [Description("Gs1DataBarExpanded")]
    Gs1DataBarExpanded = 31,
    /// <summary>GS1 DataBar Stacked</summary>
    [Description("Gs1DataBarStacked")]
    Gs1DataBarStacked = 76,
    /// <summary>GS1 DataBar Stacked Omnidirectional</summary>
    [Description("Gs1DataBarStackedOmnidirectional")]
    Gs1DataBarStackedOmnidirectional = 77,
    /// <summary>GS1 DataBar Expanded Stacked</summary>
    [Description("Gs1DataBarExpandedStacked")]
    Gs1DataBarExpandedStacked = 79,

    // -- GS1-128 and EAN-128 --

    /// <summary>EAN-128</summary>
    [Description("Ean128")]
    Ean128 = 16,
    /// <summary>GS1-128</summary>
    [Description("Gs1_128")]
    Gs1_128 = 128,

    // -- Postal Barcodes --

    /// <summary>Postnet</summary>
    [Description("Postnet")]
    Postnet = 40,
    /// <summary>PLANET</summary>
    [Description("Planet")]
    Planet = 80,
    /// <summary>FIM (Facing Identification Mark)</summary>
    [Description("Fim")]
    Fim = 47,
    /// <summary>USPS Intelligent Mail (OneCode)</summary>
    [Description("UspsIntelligentMail")]
    UspsIntelligentMail = 84,
    /// <summary>Royal Mail 4-State Customer Code (RM4SCC)</summary>
    [Description("RoyalMail4State")]
    RoyalMail4State = 70,
    /// <summary>Royal Mail Mailmark 2D</summary>
    [Description("RoyalMailMailmark2D")]
    RoyalMailMailmark2D = 121,
    /// <summary>Deutsche Post Leitcode</summary>
    [Description("DeutschePostLeitcode")]
    DeutschePostLeitcode = 21,
    /// <summary>Deutsche Post Identcode</summary>
    [Description("DeutschePostIdentcode")]
    DeutschePostIdentcode = 22,
    /// <summary>Australia Post Standard Customer</summary>
    [Description("AustraliaPost")]
    AustraliaPost = 63,
    /// <summary>Australia Post Reply Paid</summary>
    [Description("AustraliaPostReplyPaid")]
    AustraliaPostReplyPaid = 66,
    /// <summary>Australia Post Routing</summary>
    [Description("AustraliaPostRouting")]
    AustraliaPostRouting = 67,
    /// <summary>Australia Post Redirect</summary>
    [Description("AustraliaPostRedirect")]
    AustraliaPostRedirect = 68,
    /// <summary>Japan Post</summary>
    [Description("JapanPost")]
    JapanPost = 74,
    /// <summary>Korea Post</summary>
    [Description("KoreaPost")]
    KoreaPost = 75,
    /// <summary>KIX Code</summary>
    [Description("KixCode")]
    KixCode = 90,
    /// <summary>DAFT Code</summary>
    [Description("DaftCode")]
    DaftCode = 93,

    // -- 2D Matrix Barcodes --

    /// <summary>PDF417</summary>
    [Description("Pdf417")]
    Pdf417 = 55,
    /// <summary>Truncated PDF417</summary>
    [Description("Pdf417Truncated")]
    Pdf417Truncated = 56,
    /// <summary>MicroPDF417</summary>
    [Description("MicroPdf417")]
    MicroPdf417 = 82,
    /// <summary>MaxiCode</summary>
    [Description("MaxiCode")]
    MaxiCode = 57,
    /// <summary>QR Code</summary>
    [Description("QrCode")]
    QrCode = 58,
    /// <summary>Micro QR Code</summary>
    [Description("MicroQrCode")]
    MicroQrCode = 97,
    /// <summary>Rectangular Micro QR Code (rMQR)</summary>
    [Description("RectangularMicroQrCode")]
    RectangularMicroQrCode = 144,
    /// <summary>Data Matrix</summary>
    [Description("DataMatrix")]
    DataMatrix = 71,
    /// <summary>Aztec Code</summary>
    [Description("AztecCode")]
    AztecCode = 92,
    /// <summary>Aztec Runes</summary>
    [Description("AztecRunes")]
    AztecRunes = 114,
    /// <summary>DotCode</summary>
    [Description("DotCode")]
    DotCode = 115,
    /// <summary>Han Xin Code</summary>
    [Description("HanXinCode")]
    HanXinCode = 116,
    /// <summary>Ultracode</summary>
    [Description("Ultracode")]
    Ultracode = 143,
    /// <summary>Codablock-F</summary>
    [Description("CodablockF")]
    CodablockF = 152,

    // -- Other & Stacked Barcodes --

    /// <summary>ISBN (from EAN-13)</summary>
    [Description("Isbn")]
    Isbn = 69,
    /// <summary>EAN-14</summary>
    [Description("Ean14")]
    Ean14 = 72,
    /// <summary>Channel Code</summary>
    [Description("ChannelCode")]
    ChannelCode = 147,
    /// <summary>BC412</summary>
    [Description("Bc412")]
    Bc412 = 145,
    /// <summary>Digimarc</summary>
    [Description("Digimarc")]
    Digimarc = 150,

    // -- HIBC (Health Industry Bar Code) Variants --

    /// <summary>HIBC Code 128</summary>
    [Description("HibcCode128")]
    HibcCode128 = 98,
    /// <summary>HIBC Code 39</summary>
    [Description("HibcCode39")]
    HibcCode39 = 99,
    /// <summary>HIBC Data Matrix</summary>
    [Description("HibcDataMatrix")]
    HibcDataMatrix = 102,
    /// <summary>HIBC QR Code</summary>
    [Description("HibcQrCode")]
    HibcQrCode = 104,
    /// <summary>HIBC PDF417</summary>
    [Description("HibcPdf417")]
    HibcPdf417 = 106,
    /// <summary>HIBC MicroPDF417</summary>
    [Description("HibcMicroPdf417")]
    HibcMicroPdf417 = 108,
    /// <summary>HIBC Codablock-F</summary>
    [Description("HibcBlockCodeF")]
    HibcBlockCodeF = 110,
    /// <summary>HIBC Aztec Code</summary>
    [Description("HibcAztecCode")]
    HibcAztecCode = 112,

    // -- GS1 Composite Symbologies --

    /// <summary>GS1-128 Composite</summary>
    [Description("Gs1_128_Composite")]
    Gs1_128_Composite = 129,
    /// <summary>GS1 DataBar Omnidirectional Composite</summary>
    [Description("Gs1DataBar_Composite")]
    Gs1DataBar_Composite = 130,
    /// <summary>GS1 DataBar Limited Composite</summary>
    [Description("Gs1DataBarLimited_Composite")]
    Gs1DataBarLimited_Composite = 131,
    /// <summary>GS1 DataBar Expanded Composite</summary>
    [Description("Gs1DataBarExpanded_Composite")]
    Gs1DataBarExpanded_Composite = 132,
    /// <summary>UPC-A Composite</summary>
    [Description("UpcA_Composite")]
    UpcA_Composite = 133,
    /// <summary>UPC-E Composite</summary>
    [Description("UpcE_Composite")]
    UpcE_Composite = 134,
    /// <summary>GS1 DataBar Stacked Composite</summary>
    [Description("Gs1DataBarStacked_Composite")]
    Gs1DataBarStacked_Composite = 135,
    /// <summary>GS1 DataBar Stacked Omnidirectional Composite</summary>
    [Description("Gs1DataBarStackedOmni_Composite")]
    Gs1DataBarStackedOmni_Composite = 136,
    /// <summary>GS1 DataBar Expanded Stacked Composite</summary>
    [Description("Gs1DataBarExpandedStacked_Composite")]
    Gs1DataBarExpandedStacked_Composite = 137
}