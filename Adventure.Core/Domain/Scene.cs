using System.Collections.Generic;
using System.Linq;

namespace Adventure.Core.Domain
{
    public class Scene
    {
        public string Id { get; init; }

        public string Description { get; init; }

        private readonly IEnumerable<Action> _actions;
        public IReadOnlyCollection<Action> Actions => _actions.ToList();

        public Scene(string id, string description, IEnumerable<Action> actions)
        {
            Id = id;
            Description = description;
            _actions = actions;
        }
    }
}