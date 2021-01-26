using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public class ClientCommentService : ClientServiceBase
    {
        [Header("Services")]
        [SerializeField]
        private ClientUserService userService;
        [Header("Comments Settings")]
        [SerializeField]
        private RectTransform commentsParent;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private CommentDisplay commentPrefab;
        [SerializeField]
        private TMP_InputField commentInput;

        private int currentPublicationId;
        private CommentDisplayBuilder commentDisplayBuilder;
        private int editCommentId = -1;

        protected override IEnumerator Start()
        {
            yield return base.Start();

            commentDisplayBuilder = new CommentDisplayBuilder(connection, userService, this, commentPrefab, commentsParent);
        }

        public void ShowPage(int publicationId)
        {
            ShowPage();
            if (publicationId != currentPublicationId)
                commentInput.text = "";
            currentPublicationId = publicationId;
            Refresh();
        }

        public void PostComment() =>
            SendComment();

        public void EditComment(int commentId, string oldMessage)
        {
            editCommentId = commentId;
            commentInput.text = oldMessage;
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
                connection.CommentService.EditComment(userService.CurrentUser.Id, editCommentId, commentInput.text, Callback);
            else
                connection.CommentService.PostComment(userService.CurrentUser.Id, currentPublicationId, commentInput.text, Callback);
            editCommentId = -1;

            void Callback(Result result)
            {
                if (result != Result.Failed)
                {
                    Refresh();
                    commentInput.text = "";
                }
            }
        }

        private void Refresh()
        {
            commentDisplayBuilder.RefreshComments(currentPublicationId, CallBack);

            void CallBack()
            {
                StartCoroutine(ResetScrollRect());
                
                IEnumerator ResetScrollRect()
                {
                    yield return new WaitForEndOfFrame();
                    scrollRect.normalizedPosition = Vector2.zero;
                }
            }
        }
    }
}