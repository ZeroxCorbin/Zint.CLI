using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zint.CLI;

/// <summary>
/// Validates that at least one of two string properties is non?empty / non?whitespace.
/// Apply to BOTH properties for symmetric UI feedback:
/// [OneOfRequired(nameof(InputPath))] on Data
/// [OneOfRequired(nameof(Data))] on InputPath
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
internal sealed class OneOfRequiredAttribute : ValidationAttribute
{
    public string OtherPropertyName { get; }

    public OneOfRequiredAttribute(string otherPropertyName)
    {
        OtherPropertyName = otherPropertyName;
        ErrorMessage = "Either Data or InputPath must be provided.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Locate the "other" property (private/public OK)
        var otherProp = validationContext.ObjectType.GetProperty(
            OtherPropertyName,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        var otherValue = otherProp?.GetValue(validationContext.ObjectInstance);
        bool hasCurrent = value is string s && !string.IsNullOrWhiteSpace(s);
        bool hasOther = otherValue is string o && !string.IsNullOrWhiteSpace(o);

        if (hasCurrent || hasOther)
            return ValidationResult.Success;

        // IMPORTANT: include the current member name so ObservableValidator associates the error.
        var member = validationContext.MemberName ?? OtherPropertyName;
        return new ValidationResult(ErrorMessage!, new[] { member });
    }
}