using System;
using UnityEngine;
using TMPro;

namespace TEDinc.PhotosNetwork
{
    public delegate void EditComment(int commentId, string oldMessage);
    public delegate void DeleteComment(int commentId);

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
        private EditComment onEdit;
        private DeleteComment onDelete;

        public void Setup(Comment comment, User user, bool enableEdit, EditComment onEdit, DeleteComment onDelete)
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
            onEdit.Invoke(commentId, label.text);

        public void Delete() =>
            onDelete.Invoke(commentId);
    }
}