using Adventure.Core.Domain;

namespace Adventure.Core.Infrastructure
{
    public interface IGameRepository
    {
        /// <summary>
        /// Adds a new game to the repository.
        /// </summary>
        /// <param name="game"></param>
        Game AddGame(Game game);

        /// <summary>
        /// Returns the game by its ID. Returns null if not found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Game GetGame(string id);
    }
}
