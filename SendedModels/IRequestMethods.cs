namespace SendedModels
{
    public interface IRequestMethods
    {
        Request Request { get; }

        byte[] DocsSend(PositionModel model);

        byte[] MachineModelSet(MachineSetModel model);
    }
}