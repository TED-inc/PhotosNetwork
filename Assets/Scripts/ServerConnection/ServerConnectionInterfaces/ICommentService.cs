namespace TEDinc.PhotosNetwork
{
    public interface ICommentService
    {
        void GetComments(int publicationId, GetCommentsCallback callback);
        void PostComment(int userId, int publicationId, string message, ResultCallback callback);
        void EditComment(int userId, int commentId, string newMessage, ResultCallback callback);
        void RemoveComment(int userId, int commentId, ResultCallback callback);
    }
}