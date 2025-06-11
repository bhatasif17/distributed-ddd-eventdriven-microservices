using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Wakett.Application.Services;
using Wakett.Domain.Events;
using Wakett.Infrastructure.Persistence;

namespace Wakett.UnitTests;

//This unit test class does not cover all scenarios handled by the service; it is intended solely for demonstration purposes.

public class PositionServiceTests
{
    private readonly PositionsDbContext _mockContext;
    private readonly Mock<IMediator> _mockMediator;
    private readonly PositionService _service;

    public PositionServiceTests()
    {
        var options = new DbContextOptionsBuilder<PositionsDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var _mockContext = new PositionsDbContext(options);
        _mockMediator = new Mock<IMediator>();
        _service = new PositionService(_mockContext, _mockMediator.Object);
    }

    [Fact]
    public async Task HandleRateChange_NoOpenPositions_DoesNothing()
    {
        // Arrange
        var symbol = "BTC";
        var newRate = 50000m;

        // Act
        await _service.HandleRateChange(symbol, newRate);

        // Assert
        _mockMediator.Verify(m => m.Publish(It.IsAny<PositionValueUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task HandleRateChange_DifferentSymbol_DoesNotProcess()
    {
        // Arrange
        var symbol = "ETH";
        var newRate = 3000m;

        // Act
        await _service.HandleRateChange(symbol, newRate);

        // Assert
        _mockMediator.Verify(m => m.Publish(It.IsAny<PositionValueUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}