using System.Collections.Frozen;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace VN.Address;

/// <summary>
/// Service providing high-performance validation for Vietnamese addresses using .NET 10 features.
/// </summary>
public static partial class AddressService
{
    private static readonly FrozenDictionary<string, FrozenSet<string>> AddressDatabase;

    static AddressService()
    {
        AddressDatabase = LoadDatabase();
    }

    private static FrozenDictionary<string, FrozenSet<string>> LoadDatabase()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourcePath = "VN.Address.Resources.data.json";

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream is null) 
            throw new FileNotFoundException("Administrative data resource not found.");
        
        var provinces = JsonSerializer.Deserialize<List<ProvinceModel>>(stream);
        
        if (provinces is null)
            throw new InvalidDataException("Failed to parse province data.");
        
        return provinces.ToDictionary(
            p => p.Name,
            p => p.Wards.Select(w => w.Name).ToFrozenSet(StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase
        ).ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Checks if the string contains only valid characters for Vietnamese names.
    /// </summary>
    public static bool IsValidCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        return VietnameseNameRegex().IsMatch(input);
    }

    /// <summary>
    /// Validates if a province name exists.
    /// </summary>
    public static bool IsValidProvince(string provinceName)
    {
        if (string.IsNullOrWhiteSpace(provinceName)) return false;
        return AddressDatabase.ContainsKey(provinceName.Trim());
    }

    /// <summary>
    /// Strictly validates if a ward belongs to a specific province.
    /// </summary>
    public static bool IsValidAddressPair(string provinceName, string wardName)
    {
        if (string.IsNullOrWhiteSpace(provinceName) || string.IsNullOrWhiteSpace(wardName))
            return false;

        if (AddressDatabase.TryGetValue(provinceName.Trim(), out var wards))
        {
            return wards.Contains(wardName.Trim());
        }

        return false;
    }

    /// <summary>
    /// Returns all provinces available in the 2025 data.
    /// </summary>
    public static IReadOnlyCollection<string> GetAllProvinces() => AddressDatabase.Keys;

    // Source Generator 
    [GeneratedRegex(@"^[\p{L}\p{M}0-9\s\-,.]+$", RegexOptions.Compiled)]
    private static partial Regex VietnameseNameRegex();
}