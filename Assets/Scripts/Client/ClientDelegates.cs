namespace TEDinc.PhotosNetwork
{
    public delegate void EditComment(int commentId, string oldMessage);
    public delegate void DeleteComment(int commentId);
    public delegate void OpenComments(int commentId);

    public delegate void UserRequestCallback(Result result, string error = "");
    public delegate void Notify();
}