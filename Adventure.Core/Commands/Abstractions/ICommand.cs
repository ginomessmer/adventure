namespace Adventure.Core.Commands.Abstractions
{
    public interface ICommand
    {
        void ExecuteClient(ICommandSender sender);

        void ExecuteServer(ICommandSender sender);
    }
}
