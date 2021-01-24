namespace TEDinc.PhotosNetwork
{
    public interface ICommentService
    {
        void GetComments(int publicationId, int count, GetCommentsCallback callback);
        void GetComments(int publicationId, int fromCommentId, int count, GetDataMode mode, GetCommentsCallback callback);
        void PostComment(int userId, int publicationId, string message, ResultCallback callback);
        void EditComment(int userId, int commentId, string newMessage, ResultCallback callback);
        void RemoveComment(int userId, int commentId, ResultCallback callback);
    }
}