using System.Threading.Tasks;

namespace Adventure.Core.Networking.Abstractions
{
    public interface IAsyncSocketAgent : ISocketAgent
    {
        /// <summary>
        /// Starts the agent asynchronously.
        /// </summary>
        /// <returns></returns>
        Task RunAsync();
    }
}