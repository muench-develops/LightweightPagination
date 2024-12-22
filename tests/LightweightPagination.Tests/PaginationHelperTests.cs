using FluentAssertions;

namespace LightweightPagination.Tests;

/// <summary>
/// Unit tests for the PaginationHelper class.
/// </summary>
public class PaginationHelperTests
{
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
}
