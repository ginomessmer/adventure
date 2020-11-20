using Adventure.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Adventure.Core.Infrastructure
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<GameSession> _games = new();

        public Task<GameSession> AddGameAsync(GameSession game)
        {
            if (_games.Exists(x => x.Id == game.Id))
                throw new Exception("Game already exists");

            _games.Add(game);

            return Task.FromResult(game);
        }

        public Task<GameSession> GetGameAsync(string id) => Task.FromResult(_games.SingleOrDefault(x => x.Id == id));
    }
}