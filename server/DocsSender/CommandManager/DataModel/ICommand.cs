namespace DocsShowServer
{
    interface ICommand
    {
        string CMD { get; }
        string[] Parameters { get; }

        void SendCommand(Client client);
    }
}