namespace Adventure.Core.Domain
{
    public class Scene
    {
        public string Id { get; init; }

        public string Description { get; init; }

        public Scene(string id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}