namespace TEDinc.PhotosNetwork
{
    public delegate void GetPublicationsCallback((Publication publication, User user)[] publications, Result result);
    public delegate void GetPhotoCallback(PhotoData photo, Result result);
    public delegate void GetCommentsCallback(Comment[] comments, Result result);
    public delegate void UserCallback(User user, Result result);
    public delegate void ResultCallback(Result result);
}