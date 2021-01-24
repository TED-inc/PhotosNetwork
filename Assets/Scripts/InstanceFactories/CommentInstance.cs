using System;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public delegate void IntDelegeate(int commentId);

    public class CommentInstance : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text label;
        [SerializeField]
        private TMP_Text dataLabel;
        [SerializeField]
        private TMP_Text usernameLabel;
        [SerializeField]
        private GameObject editMenu;
        [SerializeField]
        private GameObject editSubMenu;

        private int commentId;
        private IntDelegeate onEdit;
        private IntDelegeate onDelete;

        public void Setup(Comment comment, User user, bool enableEdit, IntDelegeate onEdit, IntDelegeate onDelete)
        {
            commentId = comment.Id;
            this.onEdit = onEdit;
            this.onDelete = onDelete;

            label.text = comment.Message;
            usernameLabel.text = user.Username;
            dataLabel.text = new DateTime(comment.DataTimeUTC - DateTime.UtcNow.Ticks + DateTime.Now.Ticks).ToString();

            editMenu.SetActive(enableEdit);
            editSubMenu.SetActive(false);

            name = $"Comment[{comment.Id},{user.Id}]";
        }

        public void Edit() =>
            onEdit.Invoke(commentId);

        public void Delete() =>
            onDelete.Invoke(commentId);
    }
}