using conways_game_of_life_api.Models;

namespace conways_game_of_life_api.Services
{
    /// <summary>
    /// Interface for the Game of Life service.
    /// </summary>
    public interface IGameOfLifeService
    {
        Task<Guid> UploadBoardStateAsync(Board board);
        Task<Board> GetNextStateAsync(Guid id);
        Task<Board> GetXStatesAwayAsync(Guid id, int x);
        Task<Board> GetFinalStateAsync(Guid id);
    }
}