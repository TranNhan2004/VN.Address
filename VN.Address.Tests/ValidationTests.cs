namespace VN.Address.Tests;

public class ValidationTests
{
    #region 1. Character Validity Tests

    [Theory]
    [InlineData("Thành phố Hà Nội")] // Valid Vietnamese with spaces
    [InlineData("Thừa Thiên-Huế")]    // Valid with hyphen
    [InlineData("Quận 1")]           // Valid with numbers
    public void IsValidCharacters_ShouldReturnTrue_ForValidInputs(string input)
    {
        Assert.True(AddressService.IsValidCharacters(input));
    }

    [Theory]
    [InlineData("Hà Nội <script>")]  // Potential XSS/Injection
    [InlineData("Thành phố @Hà Nội")] // Special characters
    [InlineData("")]                 // Empty string
    public void IsValidCharacters_ShouldReturnFalse_ForInvalidInputs(string input)
    {
        Assert.False(AddressService.IsValidCharacters(input));
    }

    #endregion

    #region 2. Province Existence Tests

    [Fact]
    public void IsValidProvince_ShouldReturnTrue_ForExistingProvince()
    {
        // Act
        bool result = AddressService.IsValidProvince("Thành phố Hà Nội");
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidProvince_ShouldReturnTrue_IgnoringCase()
    {
        // Act
        bool result = AddressService.IsValidProvince("thành phố hà nội");
        
        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidProvince_ShouldReturnFalse_ForNonExistentProvince()
    {
        // Act
        bool result = AddressService.IsValidProvince("Tỉnh Wakanda");
        
        // Assert
        Assert.False(result);
    }

    #endregion

    #region 3. Logical Pairing Tests (Province -> Ward)

    [Theory]
    [InlineData("Thành phố Hà Nội", "Phường Ba Đình")]
    [InlineData("Thành phố Hà Nội", "Phường Cầu Giấy")]
    [InlineData("thành phố hà nội", "phường cầu giấy")] // Case insensitive pair
    public void IsValidAddressPair_ShouldReturnTrue_ForCorrectMappings(string province, string ward)
    {
        // Act & Assert
        Assert.True(AddressService.IsValidAddressPair(province, ward));
    }

    [Fact]
    public void IsValidAddressPair_ShouldReturnFalse_ForMismatchedPair()
    {
        // Act
        // Phường Bến Thành is in Ho Chi Minh City, not Hanoi
        bool result = AddressService.IsValidAddressPair("Thành phố Hà Nội", "Phường Bến Thành");
        
        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidAddressPair_ShouldReturnFalse_ForInvalidProvinceOrWard()
    {
        // Act
        bool result = AddressService.IsValidAddressPair("Tỉnh Không Tồn Tại", "Phường Ba Đình");
        
        // Assert
        Assert.False(result);
    }

    #endregion
}