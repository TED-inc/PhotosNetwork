using System.Linq;
using SQLite4Unity3d;

namespace TEDinc.PhotosNetwork
{
    public sealed class LocalCommentService : LocalServiceBase, ICommentService
    {
        public void GetComments(int publicationId, GetCommentsCallback callback)
        {
            Comment[] comments = connection.Table<Comment>().Where(comment => comment.PublicationId == publicationId).OrderBy(comment => comment.DataTimeUTC).ToArray();
            (Comment comment, User user)[] commentDatas = new (Comment comment, User user)[comments.Length];

            for (int i = 0; i < comments.Length; i++)
            {
                Comment comment = comments[i];
                User user = connection.Table<User>().Where(u => u.Id == comment.UserId).FirstOrDefault();
                commentDatas[i] = (comment, user);
            }

            callback.Invoke(commentDatas, Result.Complete);
        }

        public void PostComment(int userId, int publicationId, string message, ResultCallback callback)
        {
            connection.Insert(new Comment(userId, publicationId, message));
            callback.Invoke(Result.Complete);
        }

        public void EditComment(int userId, int commentId, string newMessage, ResultCallback callback)
        {
            Comment comment = connection.Table<Comment>().Where(c => c.Id == commentId).FirstOrDefault();
            if (comment != null && userId == comment.UserId)
            {
                comment.ChangeMassage(newMessage);
                connection.InsertOrReplace(comment);
                callback.Invoke(Result.Complete);
            }
            else
                callback.Invoke(Result.Failed);
        }

        public void RemoveComment(int userId, int commentId, ResultCallback callback)
        {
            Comment comment = connection.Table<Comment>().Where(c => c.Id == commentId).FirstOrDefault();
            if (comment != null && userId == comment.UserId)
            {
                connection.Delete(comment);
                callback.Invoke(Result.Complete);
            }
            else
                callback.Invoke(Result.Failed);
        }

        public LocalCommentService(SQLiteConnection connection) : base(connection) { }
    }
}