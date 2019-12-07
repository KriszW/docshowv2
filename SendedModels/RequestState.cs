using System;

namespace SendedModels
{
    [Serializable]
    public enum RequestState
    {
        Unknow = -10,
        Created = 0,
        Pending,
        Executing,
        Finished
    }
}