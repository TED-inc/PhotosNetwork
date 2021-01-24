using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalCommentService : LocalServiceBase, ICommentService
    {
        public void EditComment(int userId, int commentId, string newMessage, ResultCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void GetComments(int publicationId, int count, GetCommentsCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void GetComments(int publicationId, int fromCommentId, int count, GetDataMode mode, GetCommentsCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void PostComment(int userId, int publicationId, string message, ResultCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveComment(int userId, int commentId, ResultCallback callback)
        {
            throw new System.NotImplementedException();
        }

        public LocalCommentService(SQLiteConnection connection) : base(connection) { }
    }
}