namespace LightweightPagination;

/// <summary>
/// Provides helper methods for paginating collections.
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Paginates a collection based on the specified page number and page size.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The collection to paginate.</param>
    /// <param name="pageNumber">The number of the page to retrieve (1-based).</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>A paginated list of items.</returns>
    /// <exception cref="ArgumentException">Thrown if page number or page size are invalid.</exception>
    public static IEnumerable<T> Paginate<T>(IEnumerable<T> source, int pageNumber, int pageSize)
    {
        ValidateParameters(pageNumber, pageSize);

        return source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Paginates an IQueryable query for efficient database pagination.
    /// </summary>
    /// <typeparam name="T">The type of items in the query.</typeparam>
    /// <param name="query">The query to paginate.</param>
    /// <param name="pageNumber">The number of the page to retrieve (1-based).</param>
    /// <param name="pageSize">The size of the page to retrieve.</param>
    /// <returns>An IQueryable representing the paginated query.</returns>
    /// <exception cref="ArgumentException">Thrown if page number or page size are invalid.</exception>
    public static IQueryable<T> Paginate<T>(IQueryable<T> query, int pageNumber, int pageSize)
    {
        ValidateParameters(pageNumber, pageSize);

        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    /// <summary>
    /// Paginates an IAsyncEnumerable stream asynchronously based on the specified page number and page size.
    /// </summary>
    public static async IAsyncEnumerable<T> PaginateAsync<T>(IAsyncEnumerable<T> source, int pageNumber, int pageSize)
    {
        ValidateParameters(pageNumber, pageSize);
        await foreach (T? item in PaginateAsyncIterator(source, pageNumber, pageSize))
        {
            yield return item;
        }
    }

    /// <summary>
    /// Calculates pagination metadata for a given collection.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    /// <param name="source">The collection to paginate.</param>
    /// <param name="pageSize">The size of each page.</param>
    /// <returns>A tuple containing the total number of pages and the total count of items.</returns>
    public static (int TotalPages, int TotalCount) GetPaginationMetadata<T>(IEnumerable<T> source, int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        int totalCount = source.Count();
        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return (totalPages, totalCount);
    }

    /// <summary>
    /// Calculates pagination metadata for an IAsyncEnumerable source asynchronously.
    /// </summary>
    public static async Task<(int TotalPages, int TotalCount)> GetPaginationMetadataAsync<T>(IAsyncEnumerable<T> source, int pageSize)
    {
        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        int totalCount = 0;
        await foreach (T _ in source)
        {
            totalCount++;
        }

        int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return (totalPages, totalCount);
    }

    private static void ValidateParameters(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
        {
            throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
        }

        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        // Check for overflow when calculating skip count
        if ((long)pageNumber * pageSize > int.MaxValue)
        {
            throw new ArgumentException("The combination of page number and page size results in an overflow.");
        }

        // Optional: Define realistic upper limits
        const int maxPageSize = 100_000; // Example: Limit page size to 100,000 items
        if (pageSize > maxPageSize)
        {
            throw new ArgumentException($"Page size cannot exceed {maxPageSize}.", nameof(pageSize));
        }
    }

    private static async IAsyncEnumerable<T> PaginateAsyncIterator<T>(IAsyncEnumerable<T> source, int pageNumber, int pageSize)
    {
        int skipCount = (pageNumber - 1) * pageSize;
        int count = 0;

        await foreach (T? item in source)
        {
            if (skipCount > 0)
            {
                skipCount--;
                continue;
            }

            if (count++ < pageSize)
            {
                yield return item;
            }
            else
            {
                yield break;
            }
        }
    }

}
