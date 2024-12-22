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
        if (pageNumber <= 0)
        {
            throw new ArgumentException("Page number must be greater than zero.", nameof(pageNumber));
        }

        if (pageSize <= 0)
        {
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        }

        return source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
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
}
