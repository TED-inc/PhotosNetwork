using System.Collections;

namespace TEDinc.PhotosNetwork
{
    public interface IServerConnection
    {
        event OnServerConnectStateChange OnServerConnectStateChange;
        ServerConnectionState CurrentState { get; }
        IUserService UserService { get; }
        IPublicationService PublicationService { get; }
        ICommentService CommentService { get; }

        IEnumerator Setup();
    }
}