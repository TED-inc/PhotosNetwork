using System.Collections.Generic;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public delegate void EditComment(int commentId, string oldMessage);
    public delegate void DeleteComment(int commentId);

    public sealed class CommentDisplayBuilder
    {
        private IServerConnection connection;
        private ClientUserService userService;
        private ClientCommentService commentService;
        private CommentDisplay commentPrefab;
        private Transform commentsParent;

        private List<CommentDisplay> instances = new List<CommentDisplay>();


        public void RefreshComments(int publicationId, Notify onComplete)
        {
            connection.CommentService.GetComments(publicationId, Callback);

            void Callback((Comment comment, User user)[] commentDatas, Result result)
            {
                int i = 0;
                for (; i < instances.Count; i++)
                {
                    if (i < commentDatas.Length)
                        instances[i].Setup(
                            commentDatas[i].comment, 
                            commentDatas[i].user, 
                            userService.CurrentUser.Id == commentDatas[i].user.Id, 
                            commentService.EditComment, 
                            commentService.DeleteComment);
                    else
                        GameObject.Destroy(instances[i].gameObject);
                }

                if (instances.Count > commentDatas.Length)
                    instances.RemoveRange(commentDatas.Length, instances.Count - commentDatas.Length);

                for (; i < commentDatas.Length; i++)
                {
                    CommentDisplay instance = GameObject.Instantiate(commentPrefab, commentsParent);
                    instances.Add(instance);
                    instance.Setup(
                        commentDatas[i].comment, 
                        commentDatas[i].user,
                        userService.CurrentUser.Id == commentDatas[i].user.Id, 
                        commentService.EditComment,
                        commentService.DeleteComment);
                }

                onComplete.Invoke();
            }
        }

        public CommentDisplayBuilder(IServerConnection connection, ClientUserService userService, ClientCommentService commentService, CommentDisplay commentPrefab, Transform commentsParent)
        {
            this.connection = connection;
            this.userService = userService;
            this.commentService = commentService;
            this.commentPrefab = commentPrefab;
            this.commentsParent = commentsParent;
        }
    }
}