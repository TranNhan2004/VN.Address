# VN.Address

**VN.Address** is a lightweight, high-performance .NET library designed to validate Vietnamese administrative units (Provinces and Wards/Communes) based on the latest **2025 administrative data**.

It is built for developers who need a reliable way to ensure address integrity without the overhead of complex GIS systems or external API calls.

## Key Features

* **2025 Data Ready**: Updated with the latest resolutions regarding administrative boundaries in Vietnam.
* **Zero Configuration**: The address database is embedded directly into the DLL as a resource. No external JSON files or database setups are required.
* **High Performance**: Uses internal `Dictionary` and `HashSet` structures to provide  lookup speed.
* **Character Validation**: Includes built-in Regex-based validation to ensure names contain only valid Vietnamese Unicode characters, preventing common input errors or injection attempts.
* **Flexible Matching**: Fully supports case-insensitive validation for better user experience.
* **Flat Structure**: Designed for modern web forms where a direct "Province -> Ward" relationship is preferred.

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package VN.Address

```

## Usage

### 1. Character Validation

Check if a string contains valid Vietnamese characters before processing.

```csharp
using VN.Address;

bool isSafe = AddressService.IsValidCharacters("Thành phố Hà Nội"); // True
bool isUnsafe = AddressService.IsValidCharacters("Hanoi <script>"); // False

```

### 2. Province Validation

Verify if a province exists in the 2025 database.

```csharp
bool exists = AddressService.IsValidProvince("Thành phố Hà Nội"); // True
bool notFound = AddressService.IsValidProvince("Tỉnh Wakanda"); // False

```

### 3. Strict Address Pairing

Verify if a specific Ward/Commune belongs to a specific Province. This is the most critical check for data integrity.

```csharp
// Valid pair
bool validPair = AddressService.IsValidAddressPair("Thành phố Hà Nội", "Phường Ba Đình"); // True

// Invalid pair (mismatch)
// Phường Bến Thành exists in the database, but it belongs to Ho Chi Minh City
bool invalidPair = AddressService.IsValidAddressPair("Thành phố Hà Nội", "Phường Bến Thành"); // False

```

### 4. Retrieval

Get a list of all available provinces for dropdowns or selection menus.

```csharp
List<string> provinces = AddressService.GetAllProvinces();

```

## Data Source

The administrative data is sourced and flattened from the [dvhcvn repository](https://github.com/thanhtrungit97/dvhcvn) (Version 2025).

## Running Tests

The solution includes a comprehensive xUnit test suite. To run the tests:

```bash
dotnet test

```

## License

This project is licensed under the [MIT License](https://opensource.org/license/mit).

## GitHub Repository:
https://github.com/TranNhan2004/VN.Address

---
