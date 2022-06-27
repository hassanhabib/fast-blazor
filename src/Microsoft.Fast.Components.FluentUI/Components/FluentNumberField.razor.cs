using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentNumberField<TValue> : FluentInputBase<TValue>
{
    /// <summary>
    /// Gets or sets the disabled state
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the readonly state
    /// </summary>
    [Parameter]
    public bool? Readonly { get; set; }

    /// <summary>
    /// Gets or sets if a value is required
    /// </summary>
    [Parameter]
    public bool? Required { get; set; }

    /// <summary>
    /// Gets or sets if the field should automatically receive focus
    /// </summary>
    [Parameter]
    public bool? Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the size
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 20;

    /// <summary>
    /// Gets or sets the <see cref="FluentUI.Appearance" />
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the placeholder text
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the error message to show when the field can not be parsed
    /// </summary>
    [Parameter]
    public string ParsingErrorMessage { get; set; } = "The {0} field must be a number.";

    /// <summary>
    /// Gets or sets the minimum value
    /// </summary>
    [Parameter]
    public string? Min { get; set; } = GetMinValue();

    /// <summary>
    /// Gets or sets the maximum value
    /// </summary>
    [Parameter]
    public string? Max { get; set; } = GetMaxValue();

    /// <summary>
    /// Gets or sets the minimum length
    /// </summary>
    [Parameter]
    public int MinLength { get; set; } = 1;

    /// <summary>
    /// Gets or sets the maximum lenth
    /// </summary>
    [Parameter]
    public int MaxLength { get; set; } = 14;

    /// <summary>
    /// Gets or sets the amount to increase/decrease the number with. Only use whole number when TValue is int or long. 
    /// </summary>
    [Parameter]
    public string Step { get; set; } = _stepAttributeValue;

    private static readonly string _stepAttributeValue = GetStepAttributeValue();

    private static string GetStepAttributeValue()
    {
        // Unwrap Nullable<T>, because InputBase already deals with the Nullable aspect
        // of it for us. We will only get asked to parse the T for nonempty inputs.
        var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        if (targetType == typeof(int) ||
            targetType == typeof(long) ||
            targetType == typeof(short) ||
            targetType == typeof(float) ||
            targetType == typeof(double) ||
            targetType == typeof(decimal))
        {
            return "1";
        }
        else
        {
            throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
        }
    }

    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        if (BindConverter.TryConvertTo<TValue>(value, CultureInfo.InvariantCulture, out result))
        {
            validationErrorMessage = null;
            return true;
        }
        else
        {
            validationErrorMessage = string.Format(CultureInfo.InvariantCulture, ParsingErrorMessage, FieldIdentifier.FieldName);
            return false;
        }
    }

    /// <summary>
    /// Formats the value as a string. Derived classes can override this to determine the formatting used for <c>CurrentValueAsString</c>.
    /// </summary>
    /// <param name = "value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    protected override string? FormatValueAsString(TValue? value)
    {
        // Avoiding a cast to IFormattable to avoid boxing.
        return value switch
        {
            null => null,
            int @int => BindConverter.FormatValue(@int, CultureInfo.InvariantCulture),
            long @long => BindConverter.FormatValue(@long, CultureInfo.InvariantCulture),
            short @short => BindConverter.FormatValue(@short, CultureInfo.InvariantCulture),
            float @float => BindConverter.FormatValue(@float, CultureInfo.InvariantCulture),
            double @double => BindConverter.FormatValue(@double, CultureInfo.InvariantCulture),
            decimal @decimal => BindConverter.FormatValue(@decimal, CultureInfo.InvariantCulture),
            _ => throw new InvalidOperationException($"Unsupported type {value.GetType()}"),
        };
    }

    public static string GetMaxValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);
        string value = typeCode switch
        {
            TypeCode.Decimal => decimal.MaxValue.ToString("G12", CultureInfo.InvariantCulture),
            TypeCode.Double => double.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Int16 => short.MaxValue.ToString(),
            TypeCode.Int32 => int.MaxValue.ToString(),
            TypeCode.Int64 => "999999999999",
            TypeCode.SByte => sbyte.MaxValue.ToString(),
            TypeCode.Single => float.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt16 => ushort.MaxValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MaxValue.ToString(),
            TypeCode.UInt64 => "999999999999",
            _ => ""
        };

        return value;
    }

    public static string GetMinValue()
    {
        Type? targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        TypeCode typeCode = Type.GetTypeCode(targetType);

        string value = typeCode switch
        {

            TypeCode.Decimal => decimal.MinValue.ToString("G12", CultureInfo.InvariantCulture),
            TypeCode.Double => double.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.Int16 => short.MinValue.ToString(),
            TypeCode.Int32 => int.MinValue.ToString(),
            TypeCode.Int64 => "-999999999999",
            TypeCode.SByte => sbyte.MinValue.ToString(),
            TypeCode.Single => float.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt16 => ushort.MinValue.ToString(CultureInfo.InvariantCulture),
            TypeCode.UInt32 => uint.MinValue.ToString(),
            TypeCode.UInt64 => "-999999999999",
            _ => ""
        };

        return value;
    }
}