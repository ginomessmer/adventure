using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adventure.Core.Domain;

namespace Adventure.Core.Infrastructure
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games = new();

        public Task<Game> AddGameAsync(Game game)
        {
            if (_games.Exists(x => x.Id == game.Id))
                throw new Exception("Game already exists");

            _games.Add(game);

            return Task.FromResult(game);
        }

        public Task<Game> GetGameAsync(string id) => Task.FromResult(_games.SingleOrDefault(x => x.Id == id));
    }
}