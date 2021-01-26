namespace TEDinc.PhotosNetwork
{
    public interface IClientCommentService : IClientServiceBase
    {
        void ShowPage(int publicationId);
        void PostComment();
        void EditComment(int commentId, string oldMessage);
        void DeleteComment(int commentId);
    }
}