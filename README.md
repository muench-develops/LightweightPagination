# LightweightPagination

A lightweight helper library for efficient pagination in .NET. It provides utility methods to paginate collections and retrieve pagination metadata with minimal overhead. Perfect for APIs and applications that require simple and effective data pagination.


[![NuGet](https://img.shields.io/nuget/v/muench-develops..svg)](https://www.nuget.org/packages/muench-develops./) [![NuGet Downloads](https://img.shields.io/nuget/dt/muench-develops..svg)](https://www.nuget.org/packages/muench-develops./)
---

## Features

- Paginate any collection using `IEnumerable<T>`.
- Retrieve pagination metadata, such as total pages and total items.
- Lightweight and dependency-free.
- Clear error handling for invalid input.

---

## Installation

Install the package from NuGet:

```bash
dotnet add package LightweightPagination
```

## Usage
### Paginate a Collection
```csharp
using LightweightPagination;

// Example data
var items = Enumerable.Range(1, 100); // A collection of 100 items
int pageNumber = 2;
int pageSize = 10;

// Get the second page (items 11 to 20)
var paginatedItems = PaginationHelper.Paginate(items, pageNumber, pageSize);

Console.WriteLine(string.Join(", ", paginatedItems)); // Output: 11, 12, 13, ..., 20
```

### Get Pagination Metadata
```csharp
using LightweightPagination;

// Example data
var items = Enumerable.Range(1, 100); // A collection of 100 items
int pageSize = 10;

// Get metadata
var (totalPages, totalCount) = PaginationHelper.GetPaginationMetadata(items, pageSize);

Console.WriteLine($"Total Pages: {totalPages}, Total Count: {totalCount}");
// Output: Total Pages: 10, Total Count: 100
```

## API Reference
`PaginationHelper.Paginate<T>(IEnumerable<T> source, int pageNumber, int pageSize)`
- **Parameters**:
  - `source`: The collection to paginate.
  - `pageNumber`: The 1-based page number to retrieve.
  - `pageSize`: The number of items per page.
- **Returns**: A subset of the collection representing the specified page.
- **Throws**:
  - `ArgumentException`: If pageNumber or pageSize are less than or equal to 0.

`PaginationHelper.GetPaginationMetadata<T>(IEnumerable<T> source, int pageSize)`
- **Parameters**:
  - `source`: The collection to paginate.
  - `pageNumber`: The 1-based page number to retrieve.
  - `pageSize`: The number of items per page.
- **Returns**:
    - `TotalPages`: Total number of pages.
    - `TotalCount`: Total number of items in the collection.
- **Throws**:
    - `ArgumentException`: If pageSize is less than or equal to 0.

## Tests
The library includes comprehensive unit tests to ensure reliability. Run tests using:
```bash
dotnet test
```

## Contributing
Contributions are welcome! Please open an issue or submit a pull request to suggest improvements or report bugs.

## License
This project is licensed under the Apache License 2.0. See the [LICENSE](LICENSE) file for details.