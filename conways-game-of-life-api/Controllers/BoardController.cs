using conways_game_of_life_api.Models;
using conways_game_of_life_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace conways_game_of_life_api.Controllers
{
    /// <summary>
    /// Controller responsible for managing the game board.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly IGameOfLifeService _gameOfLifeService;

        public BoardsController(IGameOfLifeService gameOfLifeService)
        {
            _gameOfLifeService = gameOfLifeService;
        }

        /// <summary>
        /// Uploads a new board state.
        /// </summary>
        /// <param name="board">The board state to upload.</param>
        /// <returns>The created board state.</returns>
        [HttpPost]
        public async Task<IActionResult> UploadBoard([FromBody] Board board)
        {
            var id = await _gameOfLifeService.UploadBoardStateAsync(board);
            return CreatedAtAction(nameof(GetBoard), new { id }, board);
        }

        /// <summary>
        /// Gets the current state of the board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The current state of the board.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoard(Guid id)
        {
            var board = await _gameOfLifeService.GetNextStateAsync(id);
            if (board == null)
                return NoContent();

            return Ok(board);
        }

        /// <summary>
        /// Gets the next state of the board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The next state of the board.</returns>
        [HttpGet("{id}/next")]
        public async Task<IActionResult> GetNextState(Guid id)
        {
            var board = await _gameOfLifeService.GetNextStateAsync(id);
            if (board == null)
                return NoContent();

            return Ok(board);
        }

        /// <summary>
        /// Gets the state of the board X generations away.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <param name="x">The number of generations.</param>
        /// <returns>The state of the board X generations away.</returns>
        [HttpGet("{id}/next/{x}")]
        public async Task<IActionResult> GetXStatesAway(Guid id, int x)
        {
            var board = await _gameOfLifeService.GetXStatesAwayAsync(id, x);
            if (board == null)
                return NoContent();

            return Ok(board);
        }

        /// <summary>
        /// Gets the final stable state of the board.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The final stable state of the board.</returns>
        [HttpGet("{id}/final")]
        public async Task<IActionResult> GetFinalState(Guid id)
        {
            try
            {
                var board = await _gameOfLifeService.GetFinalStateAsync(id);
                if (board == null)
                    return NoContent();

                return Ok(board);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
