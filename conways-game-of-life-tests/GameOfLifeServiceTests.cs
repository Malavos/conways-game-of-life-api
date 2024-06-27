using conways_game_of_life_api.Models;
using conways_game_of_life_api.Repositories;
using conways_game_of_life_api.Services;
using Moq;

namespace conways_game_of_life_tests.Services
{
    /// <summary>
    /// Unit tests for the <see cref="GameOfLifeService"/> class.
    /// </summary>
    public class GameOfLifeServiceTests
    {
        private readonly Mock<IBoardRepository> _boardRepositoryMock;
        private readonly GameOfLifeService _gameOfLifeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOfLifeServiceTests"/> class.
        /// </summary>
        public GameOfLifeServiceTests()
        {
            _boardRepositoryMock = new Mock<IBoardRepository>();
            _gameOfLifeService = new GameOfLifeService(_boardRepositoryMock.Object);
        }

        /// <summary>
        /// Tests the UploadBoardStateAsync method.
        /// </summary>
        [Fact]
        public async Task UploadBoardStateAsync_ShouldReturnBoardId()
        {
            // Arrange
            var board = new Board(5, 5)
            {
                LiveCells = new List<(int X, int Y)> { (1, 1), (2, 2), (3, 3) }
            };
            _boardRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<Board>()));

            // Act
            var result = await _gameOfLifeService.UploadBoardStateAsync(board);

            // Assert
            Assert.NotEqual(Guid.Empty, result);
            _boardRepositoryMock.Verify(repo => repo.SaveAsync(board), Times.Once);
        }

        /// <summary>
        /// Tests the GetNextStateAsync method.
        /// </summary>
        [Fact]
        public async Task GetNextStateAsync_ShouldReturnNextState()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            var initialBoard = new Board(5, 5)
            {
                Id = boardId,
                LiveCells = new List<(int X, int Y)> { (1, 1), (2, 2), (3, 3) }
            };
            _boardRepositoryMock.Setup(repo => repo.GetByIdAsync(boardId)).ReturnsAsync(initialBoard);
            _boardRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<Board>())).Returns(Task.CompletedTask); // Mock save method

            // Act
            var result = await _gameOfLifeService.GetNextStateAsync(boardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(boardId, result.Id);
            _boardRepositoryMock.Verify(repo => repo.SaveAsync(result), Times.Once);
        }

        /// <summary>
        /// Tests the GetNextStateAsync method for non-existing board.
        /// </summary>
        [Fact]
        public async Task GetNextStateAsync_ShouldReturnNull_WhenBoardNotFound()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            _boardRepositoryMock.Setup(repo => repo.GetByIdAsync(boardId)).ReturnsAsync((Board)null);

            // Act
            var result = await _gameOfLifeService.GetNextStateAsync(boardId);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Tests the GetFinalStateAsync method.
        /// </summary>
        [Fact]
        public async Task GetFinalStateAsync_ShouldReturnFinalState()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            var initialBoard = new Board(5, 5)
            {
                Id = boardId,
                LiveCells = new List<(int X, int Y)> { (1, 1), (2, 2), (3, 3) }
            };
            _boardRepositoryMock.Setup(repo => repo.GetByIdAsync(boardId)).ReturnsAsync(initialBoard);
            _boardRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<Board>())).Returns(Task.CompletedTask); // Mock save method

            // Act
            var result = await _gameOfLifeService.GetFinalStateAsync(boardId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(boardId, result.Id);
            _boardRepositoryMock.Verify(repo => repo.SaveAsync(result), Times.Once);
        }

        /// <summary>
        /// Tests the GetFinalStateAsync method for non-existing board.
        /// </summary>
        [Fact]
        public async Task GetFinalStateAsync_ShouldReturnNull_WhenBoardNotFound()
        {
            // Arrange
            var boardId = Guid.NewGuid();
            _boardRepositoryMock.Setup(repo => repo.GetByIdAsync(boardId)).ReturnsAsync((Board)null);

            // Act
            var result = await _gameOfLifeService.GetFinalStateAsync(boardId);

            // Assert
            Assert.Null(result);
        }
    }
}
