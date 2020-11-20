using System;
using System.Collections.Generic;
using System.Linq;
using Adventure.Core.Domain;

namespace Adventure.Core.Infrastructure
{
    public class InMemoryGameRepository : IGameRepository
    {
        private readonly List<Game> _games = new();

        public Game AddGame(Game game)
        {
            if (_games.Exists(x => x.Id == game.Id))
                throw new Exception("Game already exists");

            _games.Add(game);

            return game;
        }

        public Game GetGame(string id) => _games.SingleOrDefault(x => x.Id == id);
    }
}