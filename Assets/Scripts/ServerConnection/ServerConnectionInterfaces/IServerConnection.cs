using System.Collections;

namespace TEDinc.PhotosNetwork
{
    public interface IServerConnection
    {
        IUserService UserService { get; }
        IPublicationService PublicationService { get; }
        ICommentService CommentService { get; }

        IEnumerator Setup();
    }
}