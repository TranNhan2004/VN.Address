using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace VN.Address;

public static class AddressService
{
    private static readonly Dictionary<string, HashSet<string>> AddressDatabase;

    static AddressService()
    {
        AddressDatabase = LoadDatabase();
    }

    private static Dictionary<string, HashSet<string>> LoadDatabase()
    {
        var assembly = Assembly.GetExecutingAssembly();
        const string resourcePath = "VN.Address.Resources.data.json";

        using var stream = assembly.GetManifestResourceStream(resourcePath);
        if (stream is null) 
            throw new FileNotFoundException("Resource data.json not found.");

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        var provinces = JsonSerializer.Deserialize<List<ProvinceModel>>(json);
        
        if (provinces is null)
            throw new FileNotFoundException("Provinces not found.");
        
        return provinces.ToDictionary(
            p => p.Name,
            p => new HashSet<string>(p.Wards.Select(w => w.Name), StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase
        );
    }

    /// <summary>
    /// Checks if the string contains only valid characters for Vietnamese names.
    /// (Letters, numbers, spaces, and basic punctuation).
    /// </summary>
    public static bool IsValidCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return false;
        // Regex to allow Vietnamese Unicode characters and standard text
        var regex = new Regex(@"^[\p{L}\p{M}0-9\s\-,.]+$");
        return regex.IsMatch(input);
    }

    /// <summary>
    /// Validates if a province name exists and is correctly formatted.
    /// </summary>
    public static bool IsValidProvince(string provinceName)
    {
        if (!IsValidCharacters(provinceName)) return false;
        return AddressDatabase.ContainsKey(provinceName.Trim());
    }

    /// <summary>
    /// Strictly validates if a ward belongs to a specific province.
    /// </summary>
    /// <param name="provinceName">Full name of the province</param>
    /// <param name="wardName">Full name of the ward/commune</param>
    public static bool IsValidAddressPair(string provinceName, string wardName)
    {
        if (!IsValidCharacters(provinceName) || !IsValidCharacters(wardName))
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
    public static List<string> GetAllProvinces() => AddressDatabase.Keys.ToList();
}



