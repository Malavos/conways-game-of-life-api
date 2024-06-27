using conways_game_of_life_api.Data;
using conways_game_of_life_api.Models;

namespace conways_game_of_life_api.Repositories
{
    /// <summary>
    /// Repository for managing board states.
    /// </summary>
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _context;


        public BoardRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Saves the board state.
        /// </summary>
        /// <param name="board">The board to save.</param>
        public async Task SaveAsync(Board board)
        {
            _context.Boards.Update(board);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves the board state by ID.
        /// </summary>
        /// <param name="id">The ID of the board.</param>
        /// <returns>The board state.</returns>
        public async Task<Board> GetByIdAsync(Guid id)
        {
            return await _context.Boards.FindAsync(id);
        }
    }
}
