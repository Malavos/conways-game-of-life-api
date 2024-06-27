using conways_game_of_life_api.Models;

namespace conways_game_of_life_api.Repositories
{
    public interface IBoardRepository
    {
        Task SaveAsync(Board board);
        Task<Board> GetByIdAsync(Guid id);
    }
}
