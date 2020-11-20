using System.Threading.Tasks;
using Adventure.Core.Domain;

namespace Adventure.Core.Infrastructure
{
    public interface IGameRepository
    {
        /// <summary>
        /// Adds a new game to the repository.
        /// </summary>
        /// <param name="gameSession"></param>
        Task<GameSession> AddGameAsync(GameSession gameSession);

        /// <summary>
        /// Returns the game by its ID. Returns null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GameSession> GetGameAsync(string id);
    }
}
