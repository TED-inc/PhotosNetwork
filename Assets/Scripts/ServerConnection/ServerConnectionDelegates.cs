namespace TEDinc.PhotosNetwork
{
    public delegate void GetPublicationsCallback(Publication[] publications, Result result);
    public delegate void GetCommentsCallback(Comment[] comments, Result result);
    public delegate void UserCallback(User user, Result result);
    public delegate void ResultCallback(Result result);
}