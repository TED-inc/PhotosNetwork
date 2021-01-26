using System;
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
                RefreshExistingCommentDisplays();
                RemoveUnusedCommentDisplays();
                AddAndRefresCommentDisplays();
                onComplete.Invoke();



                void RefreshExistingCommentDisplays()
                {
                    for (int i = 0; i < Math.Min(instances.Count, commentDatas.Length); i++)
                        SetupComment(i);
                }

                void RemoveUnusedCommentDisplays()
                {
                    if (instances.Count > commentDatas.Length)
                    {
                        for (int i = commentDatas.Length; i < instances.Count; i++)
                            GameObject.Destroy(instances[i].gameObject);

                        instances.RemoveRange(commentDatas.Length, instances.Count - commentDatas.Length);
                    }
                }

                void AddAndRefresCommentDisplays()
                {
                    for (int i = instances.Count; i < commentDatas.Length; i++)
                    {
                        CommentDisplay instance = GameObject.Instantiate(commentPrefab, commentsParent);
                        instances.Add(instance);
                        SetupComment(i);
                    }
                }

                void SetupComment(int index)
                {
                    instances[index].Setup(
                            commentDatas[index].comment,
                            commentDatas[index].user,
                            userService.CurrentUser.Id == commentDatas[index].user.Id,
                            commentService.EditComment,
                            commentService.DeleteComment);
                }
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