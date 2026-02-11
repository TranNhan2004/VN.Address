# VN.Address

**VN.Address** is a high-performance, ultra-lightweight .NET 10 library designed to validate Vietnamese administrative units (Provinces and Wards/Communes) using the latest **2025 administrative data**.

Leveraging the power of **.NET 10**, this library provides near-instant validation and minimal memory footprint, making it ideal for high-traffic web applications and microservices.

## Key Features

* **2025 Data Ready**: Fully updated with the most recent National Assembly resolutions on administrative boundaries.
* **Built for .NET 10**: Utilizes modern features like `FrozenDictionary`, `FrozenSet`, and **Source-generated Regex** for peak performance.
* **Extreme Performance**: Uses **Frozen Collections** to provide  lookup speeds with optimized memory layout.
* **Zero Configuration**: The database is embedded as a resource within the assembly—no external files or setup required.
* **Advanced Validation**: Includes a source-generated Regex engine to validate Vietnamese Unicode characters safely.
* **AOT-Friendly**: Designed to be compatible with Native AOT deployments.

## Installation

Install the package via NuGet:

```bash
dotnet add package VN.Address

```

## Usage

### 1. Character & Security Validation

Validate if an input string contains only safe, valid Vietnamese characters. Powered by **Source Generators** to eliminate runtime Regex compilation.

```csharp
using VN.Address;

bool isSafe = AddressService.IsValidCharacters("Thành phố Hà Nội"); // True
bool isUnsafe = AddressService.IsValidCharacters("Hanoi <script>"); // False

```

### 2. Province Validation

Check if a province exists in the 2025 dataset.

```csharp
bool exists = AddressService.IsValidProvince("Thành phố Hà Nội"); // True
bool notFound = AddressService.IsValidProvince("Tỉnh Wakanda"); // False

```

### 3. Strict Address Pairing

The most critical check for data integrity - verify if a Ward/Commune strictly belongs to a specific Province.

```csharp
// Valid pair
bool validPair = AddressService.IsValidAddressPair("Thành phố Hà Nội", "Phường Ba Đình"); // True

// Mismatch check (Phường Bến Thành belongs to HCM City)
bool invalidPair = AddressService.IsValidAddressPair("Thành phố Hà Nội", "Phường Bến Thành"); // False

```

### 4. Data Retrieval

Access the internal datasets for UI components like dropdowns or search suggestions.
(Updated in version 1.0.1)
```csharp
// Get all province names
IEnumerable<string> provinces = AddressService.GetAllProvinces();

// Access the entire frozen database for advanced custom logic
var fullDatabase = AddressService.GetAllAddresses();

```

## Data Source

Administrative data is sourced and flattened from the [dvhcvn repository](https://github.com/thanhtrungit97/dvhcvn) (2025 Version).

## License

This project is licensed under the [MIT License](https://opensource.org/license/mit).
