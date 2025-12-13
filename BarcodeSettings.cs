using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zint.CLI;

//[NotifyDataErrorInfo]
public partial class BarcodeSettings : ObservableValidator
{
    [ObservableProperty] private bool isValid = false;
    [ObservableProperty] private byte[]? generatedImage;

    // IMPORTANT:
    // Errors raised inside the constructor occur BEFORE WPF bindings subscribe to INotifyDataErrorInfo.ErrorsChanged
    // so they are not shown initially. We still validate here for internal state, but you MUST call
    // RefreshValidationForUi() AFTER the instance is assigned as a DataContext (e.g. in the View's Loaded event).
    public BarcodeSettings()
    {
        ValidateAllProperties();
        UpdateIsValid();
    }

    // Public method to force re‑raising validation errors after bindings are listening.
    public void RefreshValidationForUi()
    {
        // Re-run validation for every property that has validation attributes
        foreach (var propName in _validatedProperties)
        {
            PropertyInfo? pi = GetType().GetProperty(propName,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (pi is null) continue;
            var value = pi.GetValue(this);
            // This raises ErrorsChanged for that property
            ValidateProperty(value, propName);
        }
        UpdateIsValid();
    }

    // CORE ------------------------------------------------------------------
    [ObservableProperty]
    [property: OneOfRequired(nameof(InputPath))]
    [property: MaxLength(8192)]
    private string data = string.Empty;

    [ObservableProperty]
    [property: Required(ErrorMessage = "Symbology is required.")]
    private Symbologies symbology = Symbologies.DataMatrix;

    [ObservableProperty]
    [property: MaxLength(4096)]
    private string? outputPath;

    [ObservableProperty]
    [property: OneOfRequired(nameof(Data))]
    [property: MaxLength(4096)]
    private string? inputPath;

    [ObservableProperty]
    [property: MaxLength(8192)]
    private string? primaryData;

    // SIZING & APPEARANCE ---------------------------------------------------
    [ObservableProperty]
    [property: Range(0.01, 10000, ErrorMessage = "Height must be > 0.")]
    private double? height;

    [ObservableProperty]
    [property: Range(0.01, 500, ErrorMessage = "Scale must be > 0.")]
    private double? scale;

    [ObservableProperty]
    private bool useScaleXDimDp;   // When true, ScaleXDimDp drives Scale & DPI; when false user edits Scale directly.

    [ObservableProperty]
    [property: RegularExpression(
        @"^\s*\d+(\.\d+)?(mm|in)\s*,\s*\d+(\.\d+)?\s*(dpi|dpmm)\s*$",
        ErrorMessage = "ScaleXDimDp format: <value>(mm|in),<res>(dpi|dpmm) e.g. 0.33mm,300dpi or 0.33mm,11.8dpmm")]
    private string scaleXDimDp;

    [ObservableProperty]
    [property: Range(0, 500)]
    private int? borderWidth;

    [ObservableProperty]
    [property: Range(0, 2000)]
    private int? whitespace;

    [ObservableProperty]
    [property: Range(0, 2000)]
    private int? verticalWhitespace;

    [ObservableProperty]
    [property: Range(0, 270)]
    [property: CustomValidation(typeof(BarcodeSettings), nameof(ValidateRotation))]
    private int? rotationAngle;

    [ObservableProperty] private bool bindBars;
    [ObservableProperty] private bool addBox;
    [ObservableProperty] private bool compliantHeight;
    [ObservableProperty] private bool heightPerRow;
    [ObservableProperty] private bool bindTop;
    [ObservableProperty] private bool dottyMode;

    [ObservableProperty]
    [property: Range(0.01, 20, ErrorMessage = "DotSize 0.01–20.")]
    private double? dotSize;

    // COLORING --------------------------------------------------------------
    [ObservableProperty] private ColorRgb? foregroundColor;
    [ObservableProperty] private ColorRgb? backgroundColor;
    [ObservableProperty] private bool reverseColors;
    [ObservableProperty] private bool noBackground;
    [ObservableProperty] private bool useCmyk;

    // TEXT ------------------------------------------------------------------
    [ObservableProperty] private bool hideText;
    [ObservableProperty] private bool boldText;
    [ObservableProperty] private bool smallText;

    [ObservableProperty]
    [property: Range(-5.0, 10.0, ErrorMessage = "TextGap -5.0–10.0.")]
    private double? textGap;

    [ObservableProperty]
    [property: Range(7, 12, ErrorMessage = "AddOnGap 7–12.")]
    private int? addOnGap;

    [ObservableProperty] private bool guardWhitespace;
    [ObservableProperty] private bool embedFont;

    // FLAGS / SPECIAL -------------------------------------------------------
    [ObservableProperty] private bool processTilde;
    [ObservableProperty] private bool isGs1Data;
    [ObservableProperty] private bool forceSquare;
    [ObservableProperty] private bool binaryMode;

    [ObservableProperty]
    [property: Range(0, 999999)]
    private int? eci;

    [ObservableProperty] private bool? quietZones;
    [ObservableProperty] private bool gs1Separator;
    [ObservableProperty] private bool gs1Parens;

    [ObservableProperty]
    [property: Range(1, 200, ErrorMessage = "Version >= 1.")]
    private int? version;

    [ObservableProperty]
    [property: Range(0, 20, ErrorMessage = "SecurityLevel 0–20 (PDF417 typically 0–8).")]
    private int? securityLevel;

    [ObservableProperty]
    [property: Range(0, 15, ErrorMessage = "Mask 0–15.")]
    private int? mask;

    [ObservableProperty]
    [property: Range(0, 99, ErrorMessage = "Scmvv 0–99.")]
    private int? scmvv;

    [ObservableProperty]
    [property: MaxLength(2048)]
    private string? advancedOptions;

    [ObservableProperty]
    [property: Range(1, 99)]
    private int? columns;

    [ObservableProperty]
    [property: Range(1, 99)]
    private int? rows;

    [ObservableProperty]
    [property: Range(0, 50)]
    private int? mode;

    [ObservableProperty]
    [property: Range(0.0, 50.0)]
    private double? guardDescent;

    [ObservableProperty]
    [property: Range(0, 50)]
    private int? separatorHeight;

    [ObservableProperty] private bool useDmre;
    [ObservableProperty] private bool useDmIso144;
    [ObservableProperty] private bool useFastEncoding;
    [ObservableProperty] private bool useFullMultibyte;
    [ObservableProperty] private bool readerInitialization;

    // VALIDATION SUPPORT ----------------------------------------------------
    public bool ValidateAll(out IReadOnlyList<string> errors)
    {
        ValidateAllProperties();
        var list = new List<string>();
        foreach (KeyValuePair<string, List<string>> kv in GetErrorsDictionary())
            list.AddRange(kv.Value);

        errors = list;
        IsValid = errors.Count == 0;
        return IsValid;
    }

    public IReadOnlyDictionary<string, List<string>> GetErrorsDictionary()
    {
        var dict = new Dictionary<string, List<string>>();
        foreach (PropertyInfo prop in GetType().GetProperties())
        {
            IEnumerable<ValidationResult> errs = GetErrors(prop.Name);
            if (errs is IEnumerable<object> e)
            {
                var collected = new List<string>();
                foreach (var o in e)
                    if (o is string s) collected.Add(s);
                if (collected.Count > 0)
                    dict[prop.Name] = collected;
            }
        }
        return dict;
    }

    public BarcodeSettings Clone() => (BarcodeSettings)MemberwiseClone();

    public static ValidationResult? ValidateRotation(int? value, ValidationContext _)
        => !value.HasValue || value is 0 or 90 or 180 or 270
            ? ValidationResult.Success
            : new ValidationResult("RotationAngle must be 0, 90, 180, or 270.");

    // AUTOMATIC PER-PROPERTY + ASSOCIATED PROPERTY VALIDATION ---------------
    private static readonly HashSet<string> _validatedProperties =
        typeof(BarcodeSettings)
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(p => p.GetCustomAttributes<ValidationAttribute>(true).Any())
            .Select(p => p.Name)
            .ToHashSet(StringComparer.Ordinal);

    private static readonly Dictionary<string, string[]> _associatedValidation = new(StringComparer.Ordinal)
    {
        { nameof(Data),      new[] { nameof(InputPath) } },
        { nameof(InputPath), new[] { nameof(Data) } }
    };

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        var name = e.PropertyName;
        if (string.IsNullOrEmpty(name))
            return;

        if (_validatedProperties.Contains(name))
            ValidateSingle(name);

        if (_associatedValidation.TryGetValue(name, out var related))
        {
            foreach (var other in related)
                if (_validatedProperties.Contains(other))
                    ValidateSingle(other);
        }

        IsValid = !GetErrorsDictionary().Any();
    }

    private void ValidateSingle(string propertyName)
    {
        PropertyInfo? prop = GetType().GetProperty(propertyName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (prop is null) return;
        var value = prop.GetValue(this);
        ValidateProperty(value, propertyName);
    }

    private void UpdateIsValid() => IsValid = !GetErrorsDictionary().Any();
    private void RevalidateEitherInputPair()
    {
        ValidateProperty(Data, nameof(Data));
        ValidateProperty(InputPath, nameof(InputPath));
    }
}