namespace Adventure.Core.Networking.Abstractions
{
    public interface ISocketAgent
    {
        void Start();

        void Shutdown();
    }
}