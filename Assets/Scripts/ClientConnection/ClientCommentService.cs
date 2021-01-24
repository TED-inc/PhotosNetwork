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
        private CommentInstance commentPrefab;
        [SerializeField]
        private TMP_InputField commentInput;

        private int currentPublicationId;
        private CommentInstanceFactory commentFactory;

        private IEnumerator Start()
        {
            while (client.serverConnection == null)
                yield return new WaitForSecondsRealtime(0.1f);

            commentFactory = new CommentInstanceFactory(client.serverConnection, userService, this, commentPrefab, commentsParent);
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

        public void EditComment(int commentId) =>
            SendComment(commentId);

        public void DeleteComment(int id)
        {

        }

        private void SendComment(int commentId = -1)
        {
            if (commentId != -1)
                client.serverConnection.CommentService.EditComment(userService.CurrentUser.Id, commentId, commentInput.text, Callback);
            else
                client.serverConnection.CommentService.PostComment(userService.CurrentUser.Id, currentPublicationId, commentInput.text, Callback);
            commentInput.DeactivateInputField();

            void Callback(Result result)
            {
                commentInput.ActivateInputField();
                if (result != Result.Failed)
                {
                    Refresh();
                    commentInput.text = "";
                }
            }
        }

        private void Refresh()
        {
            commentFactory.RefreshComments(currentPublicationId);
            scrollRect.normalizedPosition = Vector2.zero;
        }
    }
}