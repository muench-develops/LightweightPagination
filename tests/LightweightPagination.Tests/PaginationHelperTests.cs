using FluentAssertions;

namespace LightweightPagination.Tests;

/// <summary>
/// Unit tests for the PaginationHelper class.
/// </summary>
public class PaginationHelperTests
{

    private static async IAsyncEnumerable<int> GenerateAsyncEnumerable(int count)
    {
        for (int i = 1; i <= count; i++)
        {
            await Task.Delay(1); // Simulate asynchronous operation
            yield return i;
        }
    }

    [Fact]
    public void PaginateShouldReturnCorrectPageWhenValidInput()
    {
        // Arrange
        IEnumerable<int> items = Enumerable.Range(1, 100);
        int pageNumber = 2;
        int pageSize = 10;

        // Act
        IEnumerable<int> result = PaginationHelper.Paginate(items, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(Enumerable.Range(11, 10));
    }

    [Fact]
    public void PaginateShouldThrowExceptionWhenPageNumberIsZero()
    {
        // Arrange
        IEnumerable<int> items = Enumerable.Range(1, 100);

        // Act
        Action act = () => PaginationHelper.Paginate(items, 0, 10);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Page number must be greater than zero.*");
    }

    [Fact]
    public void GetPaginationMetadataShouldReturnCorrectValues()
    {
        // Arrange
        IEnumerable<int> items = Enumerable.Range(1, 100);
        int pageSize = 10;

        // Act
        (int totalPages, int totalCount) = PaginationHelper.GetPaginationMetadata(items, pageSize);

        // Assert
        totalPages.Should().Be(10);
        totalCount.Should().Be(100);
    }

    [Fact]
    public void GetPaginationMetadataShouldThrowExceptionWhenPageSizeIsInvalid()
    {
        // Arrange
        IEnumerable<int> items = Enumerable.Range(1, 100);

        // Act
        Action act = () => PaginationHelper.GetPaginationMetadata(items, 0);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Page size must be greater than zero.*");
    }

    [Fact]
    public void PaginateIQueryableShouldReturnCorrectPageWhenValidInput()
    {
        // Arrange
        IQueryable<int> items = Enumerable.Range(1, 100).AsQueryable();
        int pageNumber = 3;
        int pageSize = 10;

        // Act
        IQueryable<int> result = PaginationHelper.Paginate(items, pageNumber, pageSize);

        // Assert
        result.Should().BeEquivalentTo(Enumerable.Range(21, 10));
    }

    [Fact]
    public void PaginateIQueryableShouldThrowExceptionWhenPageNumberIsZero()
    {
        // Arrange
        IQueryable<int> items = Enumerable.Range(1, 100).AsQueryable();

        // Act
        Action act = () => PaginationHelper.Paginate(items, 0, 10);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("*Page number must be greater than zero.*");
    }

    [Fact]
    public void GetPaginationMetadataIQueryableShouldReturnCorrectValues()
    {
        // Arrange
        IQueryable<int> items = Enumerable.Range(1, 50).AsQueryable();
        int pageSize = 10;

        // Act
        (int totalPages, int totalCount) = PaginationHelper.GetPaginationMetadata(items, pageSize);

        // Assert
        totalPages.Should().Be(5);
        totalCount.Should().Be(50);
    }

    [Fact]
    public async Task PaginateAsyncShouldReturnCorrectPageWhenValidInput()
    {
        // Arrange
        IAsyncEnumerable<int> items = GenerateAsyncEnumerable(100);
        int pageNumber = 3;
        int pageSize = 10;

        // Act
        var result = new List<int>();
        await foreach (int item in PaginationHelper.PaginateAsync(items, pageNumber, pageSize))
        {
            result.Add(item);
        }

        // Assert
        result.Should().BeEquivalentTo(Enumerable.Range(21, 10));
    }

    [Fact]
    public async Task PaginateAsyncShouldThrowExceptionWhenPageNumberIsZero()
    {
        // Arrange
        IAsyncEnumerable<int> items = GenerateAsyncEnumerable(100);

        // Act
        Func<Task> act = async () =>
        {
            await foreach (int _ in PaginationHelper.PaginateAsync(items, 0, 10))
            {
                // Do nothing
            }
        };

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Page number must be greater than zero.*");
    }

    [Fact]
    public async Task GetPaginationMetadataAsyncShouldReturnCorrectValues()
    {
        // Arrange
        IAsyncEnumerable<int> items = GenerateAsyncEnumerable(50);
        int pageSize = 10;

        // Act
        (int totalPages, int totalCount) = await PaginationHelper.GetPaginationMetadataAsync(items, pageSize);

        // Assert
        totalPages.Should().Be(5);
        totalCount.Should().Be(50);
    }

    [Fact]
    public async Task GetPaginationMetadataAsyncShouldThrowExceptionWhenPageSizeIsInvalid()
    {
        // Arrange
        IAsyncEnumerable<int> items = GenerateAsyncEnumerable(100);

        // Act
        Func<Task> act = async () => await PaginationHelper.GetPaginationMetadataAsync(items, 0);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("*Page size must be greater than zero.*");
    }
}
