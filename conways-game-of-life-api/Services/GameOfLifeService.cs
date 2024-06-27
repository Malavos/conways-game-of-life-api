using conways_game_of_life_api.Models;
using conways_game_of_life_api.Repositories;

namespace conways_game_of_life_api.Services
{
    /// <summary>
    /// Implementation of the IGameOfLifeService interface.
    /// </summary>
    public class GameOfLifeService : IGameOfLifeService
    {
        private readonly IBoardRepository _boardRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOfLifeService"/> class.
        /// </summary>
        /// <param name="boardRepository">The board repository.</param>
        public GameOfLifeService(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        }

        /// <summary>
        /// Uploads a new board state.
        /// </summary>
        /// <param name="board">The board state to upload.</param>
        /// <returns>The ID of the uploaded board.</returns>
        public async Task<Guid> UploadBoardStateAsync(Board board)
        {
            board.Id = Guid.NewGuid();
            await _boardRepository.SaveAsync(board);
            return board.Id;
        }

        /// <summary>
        /// Gets the next state for the specified board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The next state of the board.</returns>
        public async Task<Board> GetNextStateAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board == null)
                return null;

            var nextState = GetNextState(board);
            board.LiveCells = nextState.LiveCells;

            await _boardRepository.SaveAsync(board);
            return board;
        }

        /// <summary>
        /// Gets the state of the board after the specified number of generations.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <param name="x">The number of generations.</param>
        /// <returns>The state of the board after x generations.</returns>
        public async Task<Board> GetXStatesAwayAsync(Guid id, int x)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board == null)
                return null;

            for (int i = 0; i < x; i++)
            {
                var nextState = GetNextState(board);
                board.LiveCells = nextState.LiveCells;
            }

            await _boardRepository.SaveAsync(board);
            return board;
        }

        /// <summary>
        /// Gets the final state of the board. If the board does not reach a stable state within a specified number of generations, returns an error.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The final state of the board or an error if not stable.</returns>
        public async Task<Board> GetFinalStateAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board == null)
                return null;

            bool hasChanged = false;
            int maxIterations = 1000;

            for (int i = 0; i < maxIterations; i++)
            {
                var nextState = GetNextState(board);
                hasChanged = !AreBoardsEqual(board, nextState);

                if (!hasChanged)
                    break;

                board.LiveCells = nextState.LiveCells;
            }

            if (hasChanged)
                throw new Exception("Board did not reach a stable state within the maximum number of iterations.");

            await _boardRepository.SaveAsync(board);
            return board;
        }

        /// <summary>
        /// Computes the next state for the given board.
        /// </summary>
        /// <param name="board">The current board state.</param>
        /// <returns>The next board state.</returns>
        private Board GetNextState(Board board)
        {
            var nextBoard = new Board(board.Width, board.Height);
            var cellsToConsider = new HashSet<(int X, int Y)>();

            foreach (var cell in board.LiveCells)
            {
                cellsToConsider.Add(cell);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (!(i == 0 && j == 0))
                        {
                            cellsToConsider.Add((cell.X + i, cell.Y + j));
                        }
                    }
                }
            }

            foreach (var cell in cellsToConsider)
            {
                int liveNeighbors = CountLiveNeighbors(board, cell.X, cell.Y);

                if (board.LiveCells.Contains(cell))
                {
                    if (liveNeighbors == 2 || liveNeighbors == 3)
                    {
                        nextBoard.LiveCells.Add(cell);
                    }
                }
                else
                {
                    if (liveNeighbors == 3)
                    {
                        nextBoard.LiveCells.Add(cell);
                    }
                }
            }

            return nextBoard;
        }

        /// <summary>
        /// Counts the live neighbors of a cell.
        /// </summary>
        /// <param name="board">The current board state.</param>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>The number of live neighbors.</returns>
        private int CountLiveNeighbors(Board board, int x, int y)
        {
            int liveNeighbors = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                        continue;

                    int nx = x + i;
                    int ny = y + j;

                    if (board.LiveCells.Contains((nx, ny)))
                    {
                        liveNeighbors++;
                    }
                }
            }

            return liveNeighbors;
        }

        /// <summary>
        /// Checks if two boards are equal.
        /// </summary>
        /// <param name="board1">The first board.</param>
        /// <param name="board2">The second board.</param>
        /// <returns>True if the boards are equal, otherwise false.</returns>
        private bool AreBoardsEqual(Board board1, Board board2)
        {
            return new HashSet<(int X, int Y)>(board1.LiveCells).SetEquals(board2.LiveCells);
        }
    }
}
