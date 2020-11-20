using System.Collections.Generic;
using System.Linq;

namespace Adventure.Core.Domain
{
    public class Action
    {
        public string Verb { get; init; }

        private readonly IEnumerable<string> _allowedParameters;
        public IReadOnlyCollection<string> AllowedParameters => _allowedParameters.ToList();

        public Action(string verb, params string[] allowedParameters)
        {
            Verb = verb;
            _allowedParameters = allowedParameters;
        }
    }
}