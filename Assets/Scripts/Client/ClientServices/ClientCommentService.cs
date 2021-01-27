using System.Collections;
using UnityEngine;

namespace TEDinc.PhotosNetwork
{
    public class ClientCommentService : ClientServiceBase, IClientCommentService
    {
        public override ClientServiceType Type => ClientServiceType.Comment;

        private IClientUserService userService;
        private new ClientCommentServiceSerialization serviceSerialization;

        private int currentPublicationId;
        private CommentDisplayBuilder commentDisplayBuilder;
        private int editCommentId = -1;


        public void ShowPage(int publicationId)
        {
            SetActivePage(true);
            if (publicationId != currentPublicationId)
                serviceSerialization.commentInput.text = "";
            currentPublicationId = publicationId;
            Refresh();
        }

        public void PostComment() =>
            SendComment();

        public void EditComment(int commentId, string oldMessage)
        {
            editCommentId = commentId;
            serviceSerialization.commentInput.text = oldMessage;
        }

        public void DeleteComment(int commentId)
        {
            connection.CommentService.RemoveComment(userService.CurrentUser.Id, commentId, Callback);

            void Callback(Result result)
            {
                if (result != Result.Failed)
                    Refresh();
            }
        }

        private void SendComment()
        {
            if (editCommentId != -1)
                connection.CommentService.EditComment(userService.CurrentUser.Id, editCommentId, serviceSerialization.commentInput.text, Callback);
            else
                connection.CommentService.PostComment(userService.CurrentUser.Id, currentPublicationId, serviceSerialization.commentInput.text, Callback);
            editCommentId = -1;

            void Callback(Result result)
            {
                if (result != Result.Failed)
                {
                    Refresh();
                    serviceSerialization.commentInput.text = "";
                }
            }
        }

        private void Refresh()
        {
            commentDisplayBuilder.RefreshComments(currentPublicationId, CallBack);

            void CallBack()
            {
                CoroutineRunner.Instance.StartCoroutine(ResetScrollRect());
                
                IEnumerator ResetScrollRect()
                {
                    yield return new WaitForEndOfFrame();
                    serviceSerialization.scrollRect.normalizedPosition = Vector2.zero;
                }
            }
        }

        public ClientCommentService(IServerConnection connection, IClientUserService userService, ClientCommentServiceSerialization serviceSerialization) : base(connection, serviceSerialization)
        {
            this.userService = userService;
            this.serviceSerialization = serviceSerialization;

            commentDisplayBuilder = new CommentDisplayBuilder(connection, userService, this, serviceSerialization.commentPrefab, serviceSerialization.commentsParent);
        }
    }
}