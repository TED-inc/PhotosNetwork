using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public interface IServerConnection
    {
        IUserService UserService { get; }

        void Setup();
    }
}