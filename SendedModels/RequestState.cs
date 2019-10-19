using System;

namespace SendedModels
{
    [Serializable]
    public enum RequestState
    {
        Unknow = -10,
        Created,
        Pending,
        Executing,
        Finished
    }
}